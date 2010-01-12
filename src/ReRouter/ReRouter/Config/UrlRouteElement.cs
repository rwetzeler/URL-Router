
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

        [ConfigurationProperty("ipAddress")]
        //[StringValidator()] // todo.. add regex for IP address validation
        public string IpAddress
        {
            get { return (string)this["ipAddress"]; }
            set { this["ipAddress"] = value; }
        }
    }
}