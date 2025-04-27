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
    public class RegExScraper : ScraperBase
    {
        public override Image Logo { get;  set; } = Properties.Resources.Web;
        public override string Title { get; set; } = "RegEx Scraper";
        public override string Description { get; set; } = "Scrape all websites to Table format, By Regular Expression Patterns";
        
        public override bool CurrentRowToResult { get; set; } = true;

        public override bool Capture()
        {
            ArgumentBox.Label.Text = "RegEx Pattern";
            ((dynamic)HelpCenter).SetToolTip(ArgumentBox.Label, "A Regular Expression pattern to select main parts to fetch");
            return base.Capture();
        }

        public override void ClearHistory() => Browser.CloseAllTabs();

        public override IEnumerable<string> GetResultsLabels()
        {
            return base.GetResultsLabels().Concat(new string[] { "DATA" });
        }

        public override void FetchData()
        {
            RunAction(
                () =>
                {
                    if(HasArgument) SetField("DATA", Regex.Match(Browser.GetHTML(), Argument, RegexOptions.IgnoreCase | RegexOptions.Multiline).Value);
                    else SetField("DATA", Browser.GetHTML());
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
