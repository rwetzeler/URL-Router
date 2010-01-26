
namespace ReRouter.Config
{
    using System.Configuration;

    public class UrlRouteElement : ConfigurationElement
    {
        [ConfigurationProperty("url", IsRequired = true)]
        //[StringValidator()] // todo.. add regex for url validation
        public string Url
        {
            get { return (string) this["url"]; }
            set { this["url"] = value; }
        }

        private string _ipAddress;

        [ConfigurationProperty("ipAddress")]
        //[StringValidator()] // todo.. add regex for IP address validation
        public string IpAddress
        {
            get
            {
                if(string.IsNullOrEmpty(this["ipAddress"] as string))
                {
                    if (string.IsNullOrEmpty(_ipAddress))
                    {
                        _ipAddress = Utility.FindIpAddress(Url);
                    }
                    return _ipAddress;
                }

                return (string)this["ipAddress"];
            }
            set { this["ipAddress"] = value; }
        }
    }
}