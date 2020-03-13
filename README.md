# Sitecore-StandaloneShowConfig

## Description

Load the fully compiled configuration for a Sitecore server without Sitecore! Configuration issues can cause Sitecore to fail to startup. The normal process of reviewing your configuration via `/sitecore/admin/showconfig.aspx` does not work is Sitecore is not running. This can make troubleshooting config issues difficult.

Enter the Standalone ShowConfig. This app will compile the showconfig without running any of the initialization processes, thus allowing you to see what Sitecore is working with.

Read this to determine the process: https://www.xcentium.com/blog/2018/02/25/standalone-sitecore-showconfig

## How To Use

1. Checkout the code
1. Setup an IIS website targeting the `{repoRoot}/SitecoreShowConfigStandalone` folder
1. Drop your App_Config files into the website root
1. Navigate to the `/showconfig.aspx` of your site

## Rule Based Configuration Supported!

By default, the page will examine the app settings in your `web.config` file to determine the rules to apply. You can override the rules in the web.config by using the query string:

    `/showconfig.aspx?role=ContentDelivery&search=solr`
    
![image](https://user-images.githubusercontent.com/11169161/76663255-78fe2f00-654e-11ea-8f98-8563eda76721.gif)
