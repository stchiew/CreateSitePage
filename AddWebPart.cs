using System.Linq;
using Microsoft.SharePoint.Client;
using PnP.Core.Model.SharePoint;

namespace CreateSitePage
{
  public class AddWebPart
  {
    public void AddPeopleWebPart(ClientContext ctx)
    {
      Web web = ctx.Site.RootWeb;
      ctx.Load(web);
      // TODO
      // Get item from list
      var newpage = web.AddClientSidePage();
      newpage.AddSection(CanvasSectionTemplate.OneColumn, 1);
      var availableComponents = newpage.AvailablePageComponents();
      var peopleWebPartComponent = availableComponents.FirstOrDefault(p => p.Id == newpage.DefaultWebPartToWebPartId(DefaultWebPart.People));
      IPageWebPart peopleWp = newpage.NewWebPart(peopleWebPartComponent);
      string peopleProps = peopleWp.PropertiesJson;
      newpage.AddControl(peopleWp, newpage.Sections[0].Columns[0]);
      newpage.Save("Peoplepage.aspx");
      newpage.Publish();
    }

  }

}
