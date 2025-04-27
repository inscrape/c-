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
using System.Threading;
using System.Threading.Tasks;

namespace MiMFa.Scraper.CLL.Scrapers
{
    public class RecordsConvertor : ScraperBase
    {
        public override Image Logo { get;  set; } = Properties.Resources.TableRow;
        public override string Title { get; set; } = "Records Convertor";
        public override string Description { get; set; } = "Convert all files records to the available export formats";
        public override bool CurrentRowToResult { get; set; } = true;

        public override ActionMode ErrorTableActionMode { get; set; } = ActionMode.Interrupt;
        public override ActionMode ErrorRowActionMode { get; set; } = ActionMode.Skip;
        public override ActionMode ErrorCellActionMode { get; set; } = ActionMode.Skip;
        public override ActionMode ErrorDataActionMode { get; set; } = ActionMode.Interrupt;
        public override ActionMode ErrorActionMode { get; set; } = ActionMode.Skip;

        public override bool ConstructBrowser(string browserAddress = null)
        {
            return true;
        }
        public override void ClearHistory() { }

        public override void FetchData()
        {
            SetField(CurrentColumnLabel, CurrentCell);
            FlushRecord();
        }

        public override ChainedDocument FileToChain(string path)
        {
            var cf = base.FileToChain(path);
            if(cf != null) cf.OptimizedTypes = false;
            return cf;
        }
    }
}
