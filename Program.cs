using System;
using System.Linq;
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
    public const string webPartId = "4a972d93-2a95-46f8-87e2-c5c7fb2551f5";
    static void Main(string[] args)
    {
      //var webPartId = "4a972d93-2a95-46f8-87e2-c5c7fb2551f5";
      var settings = GetConfig();
      string _siteUrl = settings["siteUrl"];
      string _clientId = settings["clientId"];
      string _clientSecret = settings["clientSecret"];

      AuthenticationManager am = new AuthenticationManager();
      ClientContext ctx = am.GetACSAppOnlyContext(_siteUrl, _clientId, _clientSecret);
      string arg = args[0];
      bool isParseble = int.TryParse(arg, out int submissionId);
      if (!isParseble)
      {
        System.Environment.Exit(0);
      }
      using (ctx)
      {
        try
        {
          Web web = ctx.Site.RootWeb;
          ListItem li = web.GetListByTitle("Nominations").GetItemById(submissionId);
          ctx.Load(web);
          ctx.Load(li);
          ctx.ExecuteQueryRetry();
          string s = "Entry-{0:D3}";
          var entryPrefix = string.Format(s, li.Id);
          var pageName = entryPrefix + ".aspx";
          IPage page = web.LoadClientSidePage("Templates/PageTemplate.aspx");
          page.PageTitle = li["Title"].ToString();
          page.Save(pageName);
          Console.WriteLine(page.PageId);
          page.PageHeader.TopicHeader = li["PcCategory"].ToString();


          string txt = "Update number 2";
          IPageText textWebPart = page.NewTextPart(txt);
          ICanvasSection textSection = page.Sections[0];
          page.AddControl(textWebPart);
          page.Save(pageName);
          page.Publish(pageName);
          // Update page metadata
          List sitepages = ctx.Site.RootWeb.GetListByTitle("Site Pages");
          ContentType ctByName = ctx.Web.GetContentTypeByName("Submission", true);
          ListItem submissionpage = ctx.Site.RootWeb.GetListByTitle("Site Pages").GetItemById(page.PageId.ToString());

          submissionpage["pcNominator"] = li["pcNominator"];
          submissionpage["PcCategory"] = li["PcCategory"].ToString();
          submissionpage["ContentTypeId"] = ctByName.Id.ToString();
          submissionpage.Update();
          ctx.ExecuteQueryRetry();
          page.Publish(pageName);
        }
        catch (Exception e)
        {
          Console.WriteLine(e.Message);
          Console.Read();
        }

      }
    }

    private static void UpdateWebPartProperties(IPage page)
    {
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
        propertiesObj["script"] = "<p>Added from console program<p>";
        wp.PropertiesJson = propertiesObj.ToString();
        page.Save();
        page.Publish();
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
