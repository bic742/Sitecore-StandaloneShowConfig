using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Xml;
using Sitecore.Configuration;

namespace SitecoreShowConfigStandalone
{
    public partial class ShowConfig : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var xmlDocument = GetDocument();

            this.Response.ContentType = "text/xml";
            this.Response.Write(xmlDocument.OuterXml);
        }

        public XmlDocument GetDocument()
        {
            var ruleCollection = new NameValueCollection();
            var empty = string.Empty;

            // if there are query string params, read the query string.
            if (this.Request.QueryString.HasKeys())
            {
                // injecting the configRulesContext isn't working, so we are just always relying on rules from the query string
                foreach (var allKey in this.Request.QueryString.AllKeys)
                {
                    ruleCollection.Add($"{allKey}:{RuleBasedConfigReader.RuleDefineSuffix}",
                        this.Request.QueryString[allKey]);
                }
            }
            // otherwise, read the web.config
            else
            {
                var appSettings = ConfigurationManager.AppSettings.AllKeys.Where(key => key.Contains(":define"));

                foreach (var key in appSettings)
                {
                    ruleCollection.Add(key, ConfigurationManager.AppSettings[key]);
                }
            }

            var xmlDocument = GetRuleBasedConfiguration(ruleCollection, empty);

            return xmlDocument;
        }

        private XmlDocument GetRuleBasedConfiguration(NameValueCollection ruleCollection, string layers)
        {
            var reader = new RuleBasedConfigReader(GetIncludeFiles(layers.Split(new[]
            {
                '|'
            }, StringSplitOptions.RemoveEmptyEntries)), ruleCollection);

            var doGetConfigurationMethod = reader.GetType()
                .GetMethod("DoGetConfiguration", BindingFlags.NonPublic | BindingFlags.Instance);

            var configuration = (XmlDocument)doGetConfigurationMethod?.Invoke(reader, new object[] { });

            return configuration;
        }

        protected IEnumerable<string> GetIncludeFiles(string[] layers)
        {
            var source = GetLayeredConfiguration().ConfigurationLayerProviders.SelectMany(x => x.GetLayers());
            return layers.Length != 0
                ? source.Where(x => layers.Contains(x.Name))
                    .SelectMany(x => x.GetConfigurationFiles())
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                : source.SelectMany(x => x.GetConfigurationFiles())
                    .Distinct(StringComparer.OrdinalIgnoreCase);
        }

        protected LayeredConfigurationFiles GetLayeredConfiguration()
        {
            return new LayeredConfigurationFiles();
        }
    }
}