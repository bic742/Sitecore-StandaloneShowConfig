using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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

            var configurationRules = Sitecore.Context.ConfigurationRules;
            var strArray = configurationRules?.GetRuleDefinitionNames()?.Select(name =>
                name?.ToUpperInvariant()
            ).ToArray() ?? new string[0];

            foreach (var allKey in this.Request.QueryString.AllKeys)
            {
                if (allKey == "layer")
                    empty = this.Request.QueryString[allKey];
                else if (strArray.Contains(allKey.ToUpperInvariant()))
                    ruleCollection.Add($"{allKey}:{RuleBasedConfigReader.RuleDefineSuffix}", this.Request.QueryString[allKey]);
            }

            // var stuff = Factory.GetConfiguration();
            //var xmlDocument = GetXmlDocument(ruleCollection, empty);
            //var xmlDocument = ruleCollection.Keys.Count != 0 || empty != string.Empty ? GetXmlDocument(ruleCollection, empty) : Factory.GetConfiguration();
            var config = GetXmlDocument(ruleCollection, empty);

            return config;
        }

        private XmlDocument GetXmlDocument(NameValueCollection ruleCollection, string layers)
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