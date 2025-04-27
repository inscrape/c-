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
    public class DataNormalizer : RecordsConvertor
    {
        public override Image Logo { get;  set; } = Properties.Resources.Normalize;
        public override string Title { get;  set; } = "Data Normalizer";
        public override string Description { get;  set; } = "Normalize all data or selected columns";
        public override string CurrentRoute { get; set; } = null;

        public override string Argument { get; set; } = "[^\\s\\w-:\\;'\"+\\\\\\/=~!@#$%^&*().,<>\\[\\]|]+|(\\s+); ";

        public override bool Capture()
        {
            ArgumentBox.Label.Text = "Normalization RegEx Pattern";
            return base.Capture();
        }

        string replacement ="";
        Regex re = new Regex("[^\\s\\w-:\\;'\"+\\\\\\/=~!@#$%^&*().,<>\\[\\]|]+|(\\s+)");
        public override bool Initialize()
        {
            bool b = base.Initialize();
            replacement = Arguments.Length > 1 ? Arguments.Last() : "";
            re = new Regex(Arguments.Length > 0 ? Arguments.First() : "[^\\s\\w-:\\;'\"+\\\\\\/=~!@#$%^&*().,<>\\[\\]|]+|(\\s+)");
            return b;
        }
        public override void FetchData()
        {
            SetField(CurrentColumnLabel, re.Replace(CurrentCell, replacement));
            FlushRecord();
        }
    }
}
