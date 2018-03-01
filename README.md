# Azure Blob Cleaner

Plugin for One.NET to remove old files from Azure blob backup. Actually it is for Nikonda.Cron, however I need to change the reference.

## App.config template

```
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
    <startup> 
        <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.6.1" />
    </startup>
  <appSettings>
    <add key="AzureBlobCleanerConnectionString" value=""/>
    <add key="AzureBlobCleanerContainer" value="" />
  </appSettings>
</configuration>
```
