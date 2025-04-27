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
    public class Downloader : ScraperBase
    {
        public override Image Logo { get;  set; } = Properties.Resources.Downloader;
        public override string Title { get; set; } = "Downloader";
        public override string Description { get; set; } = "Download all websites or addresses!";
        public override bool CurrentRowToResult { get; set; } = true;

        public override bool Capture()
        {
            ArgumentBox.Label.Text = "Message";
            ((dynamic)HelpCenter).SetToolTip(ArgumentBox.Label, "A message if there needs to hav an interrupt between process, otherwise leave it empty");
            return base.Capture();
        }
        public override bool Initialize()
        {
            bool b = base.Initialize();
            if (b)
            {
                _Browser.FinishDownload += _Browser_FinishDownload;
            }
            return b;
        }

        public override void ClearHistory() => Browser.CloseAllTabs();

        public override void FetchData()
        {
            RunAction(
                () =>
                {
                    if (!Browser.WaitToDownload(15000)) throw new Exception("Could not download file!");
                    if (HasArgument) DialogService.Alert(Argument);
                },
                ex => Logger.Error(ex),
                () => Logs.Save()
            );
        }

        private void _Browser_FinishDownload(WebBrowser sender, string url, bool success, string path, object e)
        {
            RunAction(
                () =>
                {
                    if (!success) throw new Exception("Could not download successfully!");
                    SetField("URL", url);
                    SetField("PATH", path);
                    SetField("DATETIME", DateTime.UtcNow.ToShortDateString() + " " + DateTime.UtcNow.ToLongTimeString());
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
