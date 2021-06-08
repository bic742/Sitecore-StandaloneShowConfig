# escape=`
ARG BASE_IMAGE
ARG BUILD_IMAGE

FROM ${BUILD_IMAGE} as build
SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

WORKDIR C:\build
COPY SitecoreShowConfigStandalone ./

RUN nuget restore -PackagesDirectory /packages -ConfigFile ./nuget.config
RUN msbuild .\SitecoreShowConfigStandalone.csproj /p:Configuration=Release /p:DeployOnBuild=True /p:DeployDefaultTarget=WebPublish /p:WebPublishMethod=FileSystem /p:PublishUrl=C:\out\website

FROM ${BASE_IMAGE} as base

RUN powershell -Command Add-WindowsFeature Web-Server
ADD https://dotnetbinaries.blob.core.windows.net/servicemonitor/2.0.1.10/ServiceMonitor.exe C:\ServiceMonitor.exe

WORKDIR /inetpub/wwwroot
COPY --from=build C:\out\website .

# Remove Sitecore Assemblies, so they aren't distributed
RUN Remove-Item -Path C:/inetpub/wwwroot/bin/Sitecore.*.dll

WORKDIR C:\tools
RUN curl.exe -sS -L -o c:\\tools\\nuget.exe https://dist.nuget.org/win-x86-commandline/v5.2.0/nuget.exe
COPY docker/tools/ ./
COPY SitecoreShowConfigStandalone/nuget.config ./
COPY SitecoreShowConfigStandalone/packages.config ./

EXPOSE 80
ENTRYPOINT ["powershell", "C:\\tools\\Startup.ps1"]
