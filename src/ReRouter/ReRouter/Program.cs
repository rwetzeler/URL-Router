using System.Net;

namespace ReRouter
{
    using System.Configuration;
    using Config;
    using System;
    using System.Diagnostics;
    using System.Collections.Generic;

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
        }

        private static void DeleteConfiguredRoutesBeforeAdding(UrlRouteElementCollection routes)
        {
            foreach(UrlRouteElement route in routes)
            {
                DeleteRount(route);
            }
        }

        private static void DeleteRount(UrlRouteElement route)
        {
            string ipAddress = route.IpAddress;

            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = FindIpAddress(route.Url);
            }

            var proc = new Process();
            proc.StartInfo.FileName = "route";
            proc.StartInfo.Arguments = string.Format("delete {0}", ipAddress);
            proc.Start();
        }


        private static void AddRoute(UrlRouteElement route, string gateway)
        {
            string ipAddress = route.IpAddress;

            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = FindIpAddress(route.Url);
            }

            var proc = new Process();
            proc.StartInfo.FileName = "route";
            proc.StartInfo.Arguments = string.Format("add {0} mask 255.255.255.255 {1} metric 1", ipAddress, gateway);
            proc.Start();
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
    }
}
