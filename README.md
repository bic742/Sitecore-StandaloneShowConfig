# Sitecore-StandaloneShowConfig

## Description

Load the fully compiled configuration for a Sitecore server without Sitecore! Configuration issues can cause Sitecore to fail to startup. The normal process of reviewing your configuration via `/sitecore/admin/showconfig.aspx` does not work if Sitecore is not running. This can make troubleshooting config issues difficult.

Enter the Standalone ShowConfig. This app will compile the showconfig without running any of the initialization processes, thus allowing you to see what Sitecore is working with.

Read this to determine the process: https://www.xcentium.com/blog/2018/02/25/standalone-sitecore-showconfig

## How To Use

### IIS

1. Checkout the code
1. Setup an IIS website targeting the `{repoRoot}/SitecoreShowConfigStandalone` folder
1. Drop your App_Config files into the website root
1. Navigate to the `/showconfig.aspx` of your site

### Docker

**NOTE: This assumes you have Docker for Windows running. Check out [Rob's post](http://rockpapersitecore.com/2019/10/yet-another-sitecore-docker-series-part-2-running-docker/) to get set up**

1. Checkout the code
1. Copy/rename the `.env.example` --> `.env`
1. Update `WINDOWSSERVERCORE_VERSION` in the `.env` (if necessary)
1. Drop your App_Config files into the `/docker/src` folder
1. Run `docker-compose up -d`
1. Navigate to [http://standalone.dev.local/showconfig.aspx](http://standalone.dev.local/showconfig.aspx)

## Rule Based Configuration Supported!

By default, the page will examine the app settings in your `web.config` file to determine the rules to apply. You can override the rules in the web.config by using the query string:

    `/showconfig.aspx?role=ContentDelivery&search=solr`

If you're using Docker, then you can specify the values as environment variables in your compose:

```
environment:
  SITECORE_APPSETTINGS_SEARCH:DEFINE: "Azure"
```
    
![image](https://user-images.githubusercontent.com/11169161/76663255-78fe2f00-654e-11ea-8f98-8563eda76721.gif)
