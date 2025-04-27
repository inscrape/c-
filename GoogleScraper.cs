using MiMFa.Engine.Web;
using MiMFa.Model.IO;
using MiMFa.Scraper.MFL.Scrapers;
using MiMFa.Service;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiMFa.Scraper.CLL.Scrapers
{
    public class GoogleScraper : ScraperBase
    {
        public override Image Logo { get;  set; } = Properties.Resources.Google;
        public override string Title { get;  set; } = "Google Scraper";
        public override string Description { get;  set; } = "Scrape Data from Google Search Results Pages";
        public override string InitialUrl { get; set; } = "https://www.google.com";
        public override string UrlFormat { get; set; } = "https://www.google.com/search?q={0}";

        public override ActionMode ErrorTableActionMode { get; set; } = ActionMode.Interrupt;
        public override ActionMode ErrorRowActionMode { get; set; } = ActionMode.Skip;
        public override ActionMode ErrorCellActionMode { get; set; } = ActionMode.Skip;
        public override ActionMode ErrorDataActionMode { get; set; } = ActionMode.Interrupt;
        public override ActionMode ErrorActionMode { get; set; } = ActionMode.Skip;
    
        public override bool CurrentRowToResult { get; set; } = true;

        public override void FetchData()
        {
            var pointer = Browser.GetPointerJS();
            int FromItem = 0;
            if (!pointer.SelectById("rso").IsExists().Wait(30000)
                ) throw new Exception("The page is not loaded successfully!");
            else
                do {
                    var fitem = FromItem;
                    foreach (var item in pointer.SelectByXPath("//*[@id='rso']//div[@data-snc]")
                        .All().Slice(FromItem)
                    )
                    {
                        SetField("TITLE", item.Select("a h3").GetValue());
                        SetField("DESCRIPTION", item.Select("div[data-sncf]>div>span").GetValue());
                        SetField("URL", item.Select("div[data-snhf]>div>div>span>a").GetAttribute("HREF"));
                        FromItem++;
                        FlushRecord();
                    }
                    if(fitem==FromItem) break;
                    if (pointer.SelectById("pnnext").IsExists().TryPerform(false))
                    {
                        pointer.SelectById("pnnext").Click();
                        FromItem = 0;
                    }
                    else
                    {
                        Browser.ExecuteScript("scrollTo(0, document.body.scrollHeight)");
                        Statement.Wait(5000);
                    }
                } while (pointer.SelectById("rso").IsExists().Wait(30000) || DialogService.Confirm("Could not find the search results!\r\nDo you want to try again?"));
        }

    }
}
