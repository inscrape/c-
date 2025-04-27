using MiMFa.Engine.Web;
using MiMFa.Model;
using MiMFa.Model.IO;
using MiMFa.Scraper.MFL.Scrapers;
using MiMFa.Service;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace MiMFa.Scraper.CLL.Scrapers
{
    public class ColumnsMerger : DataNormalizer
    {
        public override Image Logo { get;  set; } = Properties.Resources.Combine;
        public override string Title { get; set; } = "Columns Merger";
        public override string Description { get;  set; } = "Merge all same columns in one";

        public override bool Capture()
        {
            ArgumentBox.Label.Text = "Column RegEx Pattern";
            return base.Capture();
        }
        public override IEnumerable<string> GetResultsLabels()
        {
            var indices = CurrentRouteIndices.ToList();
            var replacement = Arguments.Length > 1 ? Arguments.Last() : " ";
            Regex re = new Regex(Arguments.Length > 0 ? Arguments.First() : "\\s+");
            if(indices.Count == 0) return (from v in CurrentFile.ColumnsLabels select re.Replace(v, replacement)).Distinct();
            int ind = 0;
            return (from v in CurrentFile.ColumnsLabels select indices.Contains(ind++)?re.Replace(v, replacement):v).Distinct();
        }

        public override void FetchData()
        {
            Progress(0, (int)CurrentFile.Count());
            int pb = 0;
            foreach (var row in CurrentFile.ReadRecords())
            {
                foreach (var item in row)
                    AddField(item.Key, item.Value, true, true);
                FlushRecord();
                Progress(++pb, null);
            }
        }
    }
}
