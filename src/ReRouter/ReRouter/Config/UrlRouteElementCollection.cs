namespace ReRouter.Config
{
    using System;
    using System.Configuration;

    [ConfigurationCollection(typeof(UrlRouteElement))]
    public class UrlRouteElementCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        public UrlRouteElement this[int index]
        {
            get
            {
                return (UrlRouteElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new UrlRouteElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((UrlRouteElement)element).Url;
        }

        public void Add(UrlRouteElement element)
        {
            BaseAdd(element);
        }

        public void Clear()
        {
            BaseClear();
        }

        public void Remove(UrlRouteElement element)
        {
            BaseRemove(element.Url);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }
        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }
    }
}