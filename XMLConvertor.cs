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
    public class XMLConvertor : RecordsConvertor
    {
        public override Image Logo { get;  set; } = Properties.Resources.ExtendedMarkupLanguage;
        public override string Title { get; set; } = "XML Convertor";
        public override string Description { get; set; } = "Convert all Extended Markup Language files (XML) to a Table format";
        public override bool CurrentRowToResult { get; set; } = false;

        public virtual XmlDocument CurrentXML { get; set; }

        public override bool Initialize()
        {
            var b = base.Initialize();
            CurrentXML = new XmlDocument();
            CurrentXML.Load(CurrentFile.Path);
            return b;
        }

        public override void FetchDataByFile(ChainedDocument file)
        {
            Progress(0, CurrentXML.DocumentElement.ChildNodes.Count);
            int ind = 0;
            if (Arguments.Length < 2)
                foreach (XmlNode row in Arguments.Length == 1 ? CurrentXML.DocumentElement.SelectNodes(Argument) : CurrentXML.DocumentElement.ChildNodes)
                {
                    if (row.HasChildNodes)
                        foreach (XmlNode cell in row.ChildNodes)
                            AddXmlNode(cell);
                    else
                        AddXmlNode(row);
                    FlushRecord();
                    Progress(++ind, null);
                }
            else
                foreach (XmlNode row in CurrentXML.DocumentElement.ChildNodes)
                {
                    if (row.HasChildNodes)
                        foreach (XmlNode cell in row.ChildNodes)
                        {
                            if (Argument.Contains(cell.Name))
                                AddXmlNode(cell);
                        }
                    else if (Argument.Contains(row.Name))
                        AddXmlNode(row);
                    FlushRecord();
                    Progress(++ind, null);
                }
        }

        public int AddXmlNode(XmlNode node, string parent = "/")
        {
            var elem = ConvertService.ToXMLElements(node.OuterXml).FirstOrDefault();
            if (elem != null) parent += $"/{elem.TagName}{string.Join("", from v in elem.Attributes let k = NormalizeKey(v.Key) orderby k select $"[@{k}=\"{NormalizeValue(v.Value)}\"]")}";
            if (node.HasChildNodes) return AddXmlNode(node.ChildNodes, parent);
            string name = node.Attributes != null? node.Attributes.GetNamedItem("name").Value:null;
            return SetField(name??parent.TrimStart('/'), node.InnerText)?1:0;
        }
        public int AddXmlNode(XmlNodeList nodes, string parent = "/")
        {
            int num = 0;
            foreach (XmlNode cell in nodes) num += AddXmlNode(cell, parent);
            return num;
        }
    }
}
