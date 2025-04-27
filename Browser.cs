using MiMFa;
using MiMFa.Engine.Web;
using MiMFa.Model;
using MiMFa.Model.IO;
using MiMFa.Scraper.MFL.Scrapers;
using MiMFa.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace MiMFa.Scraper.CLL.Scrapers
{
    public class Browser : ScraperBase
    {
        public override Image Logo { get;  set; } = Properties.Resources.Browser;
        public override string Title { get; set; } = "Browser";
        public override string Description { get; set; } = "Browse all websites or addresses!";
        public override bool CurrentRowToResult { get; set; } = true;

        public override bool Capture()
        {
            ArgumentBox.Label.Text = "Message";
            ((dynamic)HelpCenter).SetToolTip(ArgumentBox.Label, "A message if there needs to hav an interrupt between process, otherwise leave it empty");
            return base.Capture();
        }
        public override void ClearHistory() => Browser.CloseAllTabs();

        public override void FetchData()
        {
            RunAction(
                () =>
                {
                    SetField("URL", Browser.Url);
                    string path = PathService.PathCreator(Destination, Browser.Title, ".html");
                    Browser.Save(path);
                    SetField("PATH", path);
                    SetField("DATETIME", DateTime.UtcNow.ToShortDateString() + " " + DateTime.UtcNow.ToLongTimeString());
                    if (HasArgument) DialogService.Alert(Argument);
                    FlushRecord();
                },
                ex =>
                {
                    ClearRecord();
                    throw ex;
                },
                () => Logs.Save()
            );
        }

    }
}
