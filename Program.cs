using System;
using Microsoft.Extensions.Configuration;
using Microsoft.SharePoint.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PnP.Core.Model.SharePoint;
using PnP.Framework;

namespace CreateSitePage
{
  class Program
  {
    static void Main(string[] args)
    {
      var webPartId = "4a972d93-2a95-46f8-87e2-c5c7fb2551f5";
      var settings = GetConfig();
      string _siteUrl = settings["siteUrl"];
      string _clientId = settings["clientId"];
      string _clientSecret = settings["clientSecret"];

      string propertyValue = "<p>Added from console program<p>";
      string propertyName = "script";
      AuthenticationManager am = new AuthenticationManager();
      ClientContext ctx = am.GetACSAppOnlyContext(_siteUrl, _clientId, _clientSecret);
      using (ctx)
      {
        Web web = ctx.Site.RootWeb;
        ctx.Load(web);
        ctx.ExecuteQueryRetry();
        IPage page = ctx.Web.LoadClientSidePage("DemoPage.aspx");
        IPageWebPart wp = null;
        foreach (var control in page.Controls)
        {
          if (control is IPageWebPart && (control as IPageWebPart).WebPartId == webPartId)
          {
            wp = control as IPageWebPart;
            break;
          }
        }

        if (wp != null)
        {
          var propertiesObj = JsonConvert.DeserializeObject<JObject>(wp.PropertiesJson);
          propertiesObj[propertyName] = propertyValue;
          wp.PropertiesJson = propertiesObj.ToString();
          page.Save();
          page.Publish();
        }

      }
    }

    private static IConfiguration GetConfig()
    {
      var builder = new ConfigurationBuilder()
      .SetBasePath(System.AppContext.BaseDirectory)
      .AddJsonFile("appsettings.json", true, true);
      return builder.Build();
    }
  }
}
