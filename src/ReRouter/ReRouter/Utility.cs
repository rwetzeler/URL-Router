namespace ReRouter
{
    using System;
    using System.Net;

    public static class Utility
    {
        public static string FindIpAddress(string routeUrl)
        {
            var ip = Dns.GetHostEntry(CleanRouteUrl(routeUrl));
            if(ip == null)
                throw new NullReferenceException("Could not find host using: " + routeUrl);

            var IpA = ip.AddressList;

            if(IpA[0] == null)
            {
                throw new Exception("Error no IP could be found for: " + routeUrl);
            }

            return IpA[0].ToString();
        }

        public static string CleanRouteUrl(string routeUrl)
        {
            return routeUrl.Replace("/", "");
        }
    }
}