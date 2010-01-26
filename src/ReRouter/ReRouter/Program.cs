namespace ReRouter
{
    using System.Configuration;
    using Config;
    using System;
    using System.Diagnostics;
    using System.Collections.Generic;
    using System.Net;

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
                Console.WriteLine("Clean routes");
                DeleteConfiguredRoutesBeforeAdding(configSection.Routes);
            }

            if (listArgs.Contains("cleanOnly"))
            {
                Console.WriteLine("Finished cleaning up only...");
                Console.ReadLine();
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
            Console.WriteLine(string.Format("Deleting route {0} for {1}:", route.Url, route.IpAddress));
            RunRouteProc(string.Format("delete {0}", route.IpAddress));
        }

        private static void AddRoute(UrlRouteElement route, string gateway)
        {
            Console.WriteLine(string.Format("Adding route for: {0} to {1} ", route.Url, route.IpAddress));
            RunRouteProc(string.Format("add {0} mask 255.255.255.255 {1} metric 1", route.IpAddress, gateway));
        }

        private static void RunRouteProc(string args)
        {
            var proc = new Process();
            var si = new ProcessStartInfo("route")
                         {
                             RedirectStandardOutput = true,
                             RedirectStandardError = true,
                             Arguments = args,
                             UseShellExecute = false,
                             CreateNoWindow = true
                         };
            proc.StartInfo = si;
            proc.Start();

            var sr = proc.StandardOutput;
            var err = proc.StandardError;

            Console.WriteLine(sr.ReadToEnd());
            Console.Write(err.ReadToEnd());
        }
    }
}
