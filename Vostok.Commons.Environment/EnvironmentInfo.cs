using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Vostok.Commons.Environment
{
    /// <summary>
    /// Provides information about environment where vostok-instrumented application is hosted.
    /// </summary>
    [PublicAPI]
    internal static class EnvironmentInfo
    {
        public const string LocalHostnameVariable = "VOSTOK_LOCAL_HOSTNAME";
        public const string LocalFQDNVariable = "VOSTOK_LOCAL_FQDN";
        public const string LocalServiceDiscoveryIPv4 = "VOSTOK_LOCAL_SERVICE_DISCOVERY_IPV4";

        private static Lazy<string> application = new Lazy<string>(ObtainApplicationName);
        private static Lazy<string> host = new Lazy<string>(ObtainHostname);
        private static Lazy<string> fqdn = new Lazy<string>(ObtainFQDN);
        private static Lazy<string> processName = new Lazy<string>(GetProcessNameOrNull);
        private static Lazy<string> homeDirectory = new Lazy<string>(ObtainHomeDirectory);
        private static Lazy<int?> processId = new Lazy<int?>(GetProcessIdOrNull);

        private static readonly TimeSpan CacheTtl = TimeSpan.FromSeconds(3);

        /// <summary>
        /// Returns the name of the application.
        /// </summary>
        public static string Application => application.Value;

        /// <summary>
        /// Returns name of the machine which runs the application. 
        /// </summary>
        public static string Host => host.Value;

        /// <summary>
        /// Returns the fully qualified domain name of the machine running the application.
        /// </summary>
        public static string FQDN => fqdn.Value;

        /// <summary>
        /// Returns the IPv4 of the machine running the application.
        /// </summary>
        public static volatile string ServiceDiscoveryIPv4;

        /// <summary>
        /// Returns the name of current process. 
        /// </summary>
        public static string ProcessName => processName.Value;

        /// <summary>
        /// Returns the id of current process. 
        /// </summary>
        public static int? ProcessId => processId.Value;

        /// <summary>
        /// Returns the base directory of current assembly.
        /// </summary>
        public static string BaseDirectory => GetBaseDirectory();

        /// <summary>
        /// Returns the home directory of current user.
        /// </summary>
        public static string HomeDirectory => homeDirectory.Value;

        static EnvironmentInfo()
        {
            UpdateAndSchedule();
        }

        private static void UpdateAndSchedule()
        {
            ServiceDiscoveryIPv4 = ObtainServiceDiscoveryIPv4();

            Task.Delay(CacheTtl).ContinueWith(_ => UpdateAndSchedule());
        }

        private static string ObtainApplicationName()
        {
            try
            {
                if (RuntimeDetector.IsDotNetCore)
                    return GetEntryAssemblyNameOrNull();

                var processNameOrNull = GetProcessNameOrNull();

                if (!string.IsNullOrEmpty(processNameOrNull) && processNameOrNull.ToLowerInvariant() != "w3wp" && processNameOrNull.ToLowerInvariant() != "iisexpress")
                    return processNameOrNull;

                var assemblyNameOrNull = GetEntryAssemblyNameOrNull();
                if (!string.IsNullOrEmpty(assemblyNameOrNull))
                    return assemblyNameOrNull;

                var iisNameOrNull = GetIisApplication();
                if (!string.IsNullOrEmpty(iisNameOrNull))
                    return iisNameOrNull;

                var directory = GetBaseDirectory() ?? string.Empty;
                var segments = directory.Split(new[] {Path.DirectorySeparatorChar}, StringSplitOptions.RemoveEmptyEntries).Skip(1).ToList();
                if (!string.IsNullOrEmpty(processNameOrNull))
                    segments.Add(processNameOrNull);
                if (segments.Any())
                    return string.Join("/", segments);

                return null;
            }
            catch
            {
                return null;
            }
        }

        private static string GetProcessNameOrNull()
        {
            try
            {
                return Process.GetCurrentProcess().ProcessName;
            }
            catch
            {
                return null;
            }
        }

        private static int? GetProcessIdOrNull()
        {
            try
            {
                return Process.GetCurrentProcess().Id;
            }
            catch
            {
                return null;
            }
        }

        private static string GetEntryAssemblyNameOrNull()
        {
            try
            {
                return Assembly.GetEntryAssembly()?.GetName().Name;
            }
            catch
            {
                return null;
            }
        }

        private static string GetBaseDirectory()
        {
            try
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
            catch
            {
                return null;
            }
        }

        private static string ObtainHostname()
        {
            try
            {
                return System.Environment.GetEnvironmentVariable(LocalHostnameVariable)
                       ?? Dns.GetHostName();
            }
            catch
            {
                return "unknown";
            }
        }

        private static string ObtainFQDN()
        {
            try
            {
                var localFqdn = System.Environment.GetEnvironmentVariable(LocalFQDNVariable);
                if (localFqdn != null)
                    return localFqdn;

                var localHostname = System.Environment.GetEnvironmentVariable(LocalHostnameVariable);
                if (localHostname != null)
                    return Dns.GetHostEntry(localHostname).HostName;

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    var domainName = GetWindowsDomainName();

                    if (!string.IsNullOrEmpty(domainName))
                    {
                        domainName = "." + domainName.TrimStart('.');

                        var hostName = ObtainHostname();
                        if (hostName.EndsWith(domainName))
                            return hostName;

                        return hostName + domainName;
                    }
                }

                return Dns.GetHostEntry(ObtainHostname()).HostName;
            }
            catch
            {
                return ObtainHostname();
            }
        }

        private static string ObtainServiceDiscoveryIPv4()
        {
            try
            {
                var localIpV4 = System.Environment.GetEnvironmentVariable(LocalServiceDiscoveryIPv4);
                if (localIpV4 != null)
                    return localIpV4;

                var dnsAddresses = Dns.GetHostAddresses(ObtainHostname());

                var interfaceAddresses = NetworkInterface
                    .GetAllNetworkInterfaces()
                    .Where(iface => iface.OperationalStatus == OperationalStatus.Up)
                    .Where(iface => iface.GetIPProperties().GatewayAddresses.Any(info => !info.Address.Equals(IPAddress.Any)))
                    .SelectMany(iface => iface.GetIPProperties().UnicastAddresses)
                    .Select(info => info.Address)
                    .ToList();

                var address = dnsAddresses
                    .Where(address => address.AddressFamily == AddressFamily.InterNetwork)
                    .Where(address => !IPAddress.IsLoopback(address))
                    .Where(address => interfaceAddresses.Contains(address))
                    .Select(address => address.ToString())
                    .FirstOrDefault();

                return address;
            }
            catch
            {
                return null;
            }
        }

        private static string GetWindowsDomainName()
        {
            try
            {
                return IPGlobalProperties.GetIPGlobalProperties().DomainName;
            }
            catch
            {
                return null;
            }
        }

        private static string ObtainHomeDirectory()
        {
            try
            {
                var home = System.Environment.GetEnvironmentVariable("HOME");

                if (!string.IsNullOrEmpty(home))
                    return home;

                var homeDrive = System.Environment.GetEnvironmentVariable("HOMEDRIVE");
                var homePath = System.Environment.GetEnvironmentVariable("HOMEPATH");

                if (!string.IsNullOrEmpty(homeDrive) && !string.IsNullOrEmpty(homePath))
                    return homeDrive + homePath;

                var userProfile = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile);
                if (!string.IsNullOrEmpty(userProfile))
                    return userProfile;

                return null;
            }
            catch
            {
                return null;
            }
        }

        private static string GetIisApplication()
        {
            try
            {
                var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().Name == "System.Web");
                if (assembly == null)
                    return null;

                var type = assembly.GetType("System.Web.Hosting.HostingEnvironment");
                if (type == null)
                    return null;

                if (!Invoke<bool>("IsHosted"))
                    return null;

                var vPath = Invoke<string>("ApplicationVirtualPath");
                var siteName = Invoke<string>("SiteName");
                if (vPath == null || vPath == "/")
                    return siteName;

                return siteName + vPath;

                // ReSharper disable once PossibleNullReferenceException
                T Invoke<T>(string name) => (T)type
                    .GetProperty(name, BindingFlags.Public | BindingFlags.Static)
                    .GetMethod.Invoke(null, Array.Empty<object>());
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}