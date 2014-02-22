using System.Configuration;

namespace EpiCdnHandler
{
    public class CdnConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("enabled", IsRequired = false, DefaultValue = "true")]
        public bool Enabled
        {
            get { return (bool)base["enabled"]; }
        }

        [ConfigurationProperty("url", IsRequired = false, DefaultValue = "")]
        public string Url
        {
            get { return (string)base["url"]; }
        }

        [ConfigurationProperty("itemkey", IsRequired = false, DefaultValue = "image-cdn-request")]
        public string ItemKey
        {
            get { return (string)base["itemkey"]; }
        }

        public static CdnConfigurationSection GetConfiguration()
        {
            var configuration = ConfigurationManager.GetSection("epiCdnHandler") as CdnConfigurationSection;
            return configuration ?? new CdnConfigurationSection();
        }
    }
}