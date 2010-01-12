using System.Net;

namespace ReRouter
{
    using System.Configuration;
    using Config;
    using System;
    using System.Diagnostics;
    using System.Collections.Generic;
    using System.IO;

    public class Program
    {
        static void Main(string[] args)
        {
            var listArgs = new List<string>(args);
            var configSection = (UrlRouteConfigSection)ConfigurationManager.GetSection(
                ConfigurationManager.AppSettings["ReRouterConfigSection"]);

            if (configSection == null)
                throw new NullReferenceException("Could not find configSection by the name: " + ConfigurationManager.AppSettings["ReRouterConfigSection"]);

            if (listArgs.Contains("cleanFirst") || listArgs.Contains("cleanOnly"))
            {
                DeleteConfiguredRoutesBeforeAdding(configSection.Routes);
            }

            if (listArgs.Contains("cleanOnly"))
            {
                return;
            }

            foreach (UrlRouteElement route in configSection.Routes)
            {
                AddRoute(route, configSection.OverrideGatewayIp);
            }
            Console.WriteLine("Finished...");
            Console.ReadLine();
        }

        private static void DeleteConfiguredRoutesBeforeAdding(UrlRouteElementCollection routes)
        {
            foreach(UrlRouteElement route in routes)
            {
                DeleteRoute(route);
            }
        }

        private static void DeleteRoute(UrlRouteElement route)
        {
            string ipAddress = route.IpAddress;

            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = FindIpAddress(route.Url);
            }
            Console.WriteLine("Deleting route: " + ipAddress);
            RunRouteProc(string.Format("delete {0}", ipAddress));
        }

        private static void AddRoute(UrlRouteElement route, string gateway)
        {
            string ipAddress = route.IpAddress;

            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = FindIpAddress(route.Url);
            }

            Console.WriteLine("Adding route for: " + route.Url);
            RunRouteProc(string.Format("add {0} mask 255.255.255.255 {1} metric 1", ipAddress, gateway));
        }

        private static string FindIpAddress(string routeUrl)
        {
            IPHostEntry ip = Dns.GetHostByName(routeUrl);
            if(ip == null)
                throw new NullReferenceException("Could not find host using: " + routeUrl);

            IPAddress[] IpA = ip.AddressList;

            if(IpA[0] == null)
            {
                throw new Exception("Error no IP could be found for: " + routeUrl);
            }

            return IpA[0].ToString();
        }

        private static void RunRouteProc(string args)
        {
            var proc = new Process();
            var si = new ProcessStartInfo("route");
            si.RedirectStandardOutput = true;
            si.RedirectStandardError = true;
            si.Arguments = args;
            si.UseShellExecute = false;
            si.CreateNoWindow = true;
            proc.StartInfo = si;
            proc.Start();

            StreamReader sr = proc.StandardOutput;
            StreamReader err = proc.StandardError;

            Console.WriteLine(sr.ReadToEnd());
            Console.Write(err.ReadToEnd());
        }
    }
}
