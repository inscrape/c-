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
    public class Scraper : ScraperBase
    {
        public override Image Logo { get;  set; } = Properties.Resources.Scraper;
        public override string Title { get;  set; } = "Scraper";
        public override string Description { get;  set; } = "Scrape Data from Documents by Query, XPath, Location, Pure Script, etc.";
        public override string InitialUrl { get; set; } = "https://www.google.com";
        public override string UrlFormat { get; set; } = "{0}";
        public override string Argument { get; set; } = "table tr";
        public override int CurrentRowIndex { get; set; } = 0;

        public override ActionMode ErrorFileActionMode { get; set; } = ActionMode.Skip;
        public override ActionMode ErrorTableActionMode { get; set; } = ActionMode.Interrupt;
        public override ActionMode ErrorRowActionMode { get; set; } = ActionMode.Skip;
        public override ActionMode ErrorCellActionMode { get; set; } = ActionMode.Skip;
        public override ActionMode ErrorDataActionMode { get; set; } = ActionMode.Interrupt;
        public override ActionMode ErrorActionMode { get; set; } = ActionMode.Skip;
     
        public override bool CurrentRowToResult { get; set; } = true;

        public override bool Capture()
        {
            ArgumentBox.Label.Text = "Fetch";
            ((dynamic)HelpCenter).SetToolTip(ArgumentBox.Label, "Query, XPath, Location, Pure Script, etc. to select main parts to fetch");
            return base.Capture();
        }

        public override void FetchData()
        {
            int c = HasCurrentFile? CurrentFile.WarpsCount>-1?(int)CurrentFile.WarpsCount:1: 0;
            var pointer = Browser.GetPointerJS();
            do
            {
                if (!Selector(pointer).IsExists().Wait(15000))
                    throw new Exception("The page is not loaded successfully!");
                else
                    foreach (var row in Selector(pointer).All())
                    {
                        var i = c;
                        foreach (var item in row.Clone().Children())
                            SetField(i++, item.Clone().GetContent().TryPerform(""));
                        if (i <= c) SetField(c, row.Clone().GetContent().TryPerform(""));
                        else if (i <= c +1) SetField(--i, row.Clone().GetContent().TryPerform(""));
                        FlushRecord();
                    }
            } while (!HasCurrentFile && (string.IsNullOrWhiteSpace(ContinueScript+BreakScript) || DialogService.Warn("Do you want to fetch data again!")));
        }

        public virtual PointerJS Selector(PointerJS pointer) => PointerJS.DetectPointerMode(Argument) == PointerMode.Pure ? pointer.From(Argument, PointerMode.Pure) : pointer.Select(Argument);
    }
}
