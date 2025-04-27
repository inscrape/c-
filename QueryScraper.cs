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
    public class QueryScraper : Scraper
    {
        public override Image Logo { get;  set; } = Properties.Resources.TableCol;
        public override string Title { get;  set; } = "Query Scraper";
        public override string Description { get;  set; } = "Scrape Data from Documents by Query Selector";
        public override string Argument { get; set; } = "table tr";

        public override bool Capture()
        {
            ArgumentBox.Label.Text = "Query";
            ((dynamic)HelpCenter).SetToolTip(ArgumentBox.Label, "A CSS selector to select main parts to fetch");
            return base.Capture();
        }

        public override PointerJS Selector(PointerJS pointer) => pointer.SelectByQuery(Argument);
    }
}
