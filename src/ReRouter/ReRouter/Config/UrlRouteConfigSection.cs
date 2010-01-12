namespace ReRouter.Config
{
    using System.Configuration;

    public class UrlRouteConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("overrideGatewayIp", IsRequired = true)] // doto.. make this optional and lookup by specifying nic
        public string OverrideGatewayIp
        {
            get
            {
                return (string)this["overrideGatewayIp"];
            }
            set
            {
                this["overrideGatewayIp"] = value;
            }
        }

        [ConfigurationProperty("Routes", IsDefaultCollection = false),
        ConfigurationCollection(typeof(UrlRouteElementCollection), 
            AddItemName = "addRoute",
            ClearItemsName = "clearRoutes", 
            RemoveItemName = "removeRoute")]
        public UrlRouteElementCollection Routes
        {
            get { return this["Routes"] as UrlRouteElementCollection; }
        }
    }
}