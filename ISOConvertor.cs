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
    public class ISOConvertor : RecordsConvertor
    {
        public override Image Logo { get;  set; } = Properties.Resources.Archive;
        public override string Title { get;  set; } = "ISO Convertor";
        public override string Description { get;  set; } = "Convert all ISO files to Table format";
        public override bool CurrentRowToResult { get; set; } = false;

        public override bool Initialize()
        {
            var b = base.Initialize();
            CurrentFile.LinesSplitter = "";
            CurrentFile.WarpsSplitters = new string[]{"",""};
            return b;
        }
        public override bool Finalize()
        {
            return base.Finalize();
        }

        public override void FetchDataByTable(IEnumerable<IEnumerable<string>> table)
        {
            int pb = 0;
            if (Arguments.Length < 1)
                foreach (var row in table)
                {
                    if (row.First().Length > 24)
                    {
                        int ind = 0;
                        List<string> heads = new List<string>();
                        foreach (Match item in Regex.Matches(row.First().Trim().Substring(24), "[\\d\\D]{3}"))
                            if(!item.Value.StartsWith("00")) heads.Add(item.Value);
                        foreach (var cell in row.Skip(1).Take(heads.Count)) 
                            AddField(heads[ind++], cell, true);
                        FlushRecord();
                        Progress(++pb, null);
                    }
                }
            else
                foreach (var row in table)
                {
                    if (row.First().Length > 24)
                    {
                        int ind = 0;
                        List<string> heads = new List<string>();
                        foreach (Match item in Regex.Matches(row.First().Trim().Substring(24), "[\\d\\D]{3}"))
                            if (!item.Value.StartsWith("00")) heads.Add(item.Value);
                        foreach (var cell in row.Skip(1).Take(heads.Count))
                            if (Arguments.Contains(heads[ind++]))
                                AddField(heads[ind], cell, true);
                        FlushRecord();
                        Progress(++pb, null);
                    }
                }
        }

    }
}
