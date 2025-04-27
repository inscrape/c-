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
    public class LocationScraper : Scraper
    {
        public override Image Logo { get;  set; } = Properties.Resources.TableCol;
        public override string Title { get;  set; } = "Location Scraper";
        public override string Description { get;  set; } = "Scrape Data from Documents by Location Address";
        public override string Argument { get; set; } = "500,700";

        public override bool Capture()
        {
            ArgumentBox.Label.Text = "X,Y";
            ((dynamic)HelpCenter).SetToolTip(ArgumentBox.Label, "The exact location of the main element to fetch");
            return base.Capture();
        }

        public override PointerJS Selector(PointerJS pointer) => pointer.SelectByLocation(Argument);
    }
}
