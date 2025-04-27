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
    public class ScriptScraper : Scraper
    {
        public override Image Logo { get;  set; } = Properties.Resources.Script;
        public override string Title { get;  set; } = "Script Scraper";
        public override string Description { get;  set; } = "Scrape Data from Documents by custom Script Selector";
        public override string Argument { get; set; } = "document.querySelectorAll('table tr')";

        public override bool Capture()
        {
            ArgumentBox.Label.Text = "Script";
            ((dynamic)HelpCenter).SetToolTip(ArgumentBox.Label, "A Script script to select main parts to fetch");
            return base.Capture();
        }

        public override PointerJS Selector(PointerJS pointer) => pointer.SelectPure(Argument);
    }
}
