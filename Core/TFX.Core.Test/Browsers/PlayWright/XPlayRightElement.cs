
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using HtmlAgilityPack;

using Microsoft.Playwright;

using TFX.Core.Test.Interfaces;

namespace TFX.Core.Test.PlayWright
{
    internal class XPlayRightElement : XIWSElement
    {
        public XPlayRightElement(XIWSPage pPage, ILocator pLocator)
        {
            _Page = pPage;
            _Locator = pLocator;
        }

        private XIWSPage _Page;
        private ILocator _Locator;

        public Boolean IsValid => _Locator != null;

        public async Task Focus()
        {
            await _Locator.FocusAsync();
        }

        public async Task ClickAsync()
        {
            await _Locator.ClickAsync();
        }

        public async Task ClickToVisible()
        {
            await _Locator.ClickAsync();
            var cnt = 1000;
            while (cnt > 0)
            {
                if (_Locator.IsVisibleAsync().Result)
                    break;
                Thread.Yield();
                Thread.Sleep(100);
                cnt -= 100;
            }
        }

        public async Task FillAsync(String pValue)
        {
            var isVisible = await _Locator.IsVisibleAsync();
            if (isVisible)
                await _Locator?.FillAsync(pValue);
            else
                await _Locator.EvaluateAsync($"el => el.setAttribute(\"value\", \"{pValue}\")");
        }

        public string GetValue()
        {
            var xx = _Locator?.AllInnerTextsAsync().Result;
            return _Locator?.InputValueAsync().Result;
        }

        public string GetInnerTexts()
        {
            var txt = _Locator?.AllInnerTextsAsync().Result;
            if (txt?.Count == 0)
                return "";
            return string.Join(" ", txt);

        }

        public async Task EvaluateAsync(string pScrit) => await _Locator?.EvaluateAsync(pScrit);
    }

    internal class PlayRightElementHandle : ISCElementHandle
    {
        public PlayRightElementHandle(XIWSPage pPage, IElementHandle pHandle)
        {
            _Page = pPage;
            _Handle = pHandle;
        }

        private XIWSPage _Page;
        private IElementHandle _Handle;


        public Boolean IsValid => _Handle != null;


        public List<ISCElementHandle> GetHandlers(String pTag)
        {
            var lst = new List<ISCElementHandle>();
            var linhas = _Handle.QuerySelectorAllAsync(pTag).Result;
            foreach (var hd in linhas)
                lst.Add(new PlayRightElementHandle(_Page, hd));
            return lst;
        }
    }

    public class SCElementValuable : XIWSElementValuable
    {
        public SCElementValuable(HtmlNode pNode)
        {
            _Node = pNode;
        }

        private HtmlNode _Node;
        public String Value => _Node?.InnerText;

        public Boolean IsValid => _Node != null;

        public string GetAttribute(string pAttribute)
        {

            return _Node.GetAttributeValue(pAttribute, "");
        }
    }

    public class SCTableColumn : SCElementValuable, XIWSTableColumn
    {

        public SCTableColumn(HtmlNode pNode)
            : base(pNode)
        {
            _Node = pNode;
        }

        private HtmlNode _Node;

        public XIWSTableColumn Hader
        {
            get;
            internal set;
        }

        public XIWSElementValuable GetElement(params XAtt[] pAttributes)
        {

            foreach (var ndl in _Node.ChildNodes)
            {
                var nd = GetNode(ndl, pAttributes);
                if (nd != null)
                    return new SCElementValuable(nd);
            }
            static HtmlNode GetNode(HtmlNode pNode, XAtt[] pAttributes)
            {
                foreach (var ndl in pNode.ChildNodes)
                {
                    if (pAttributes.All(a => ndl.GetAttributeValue(a.Attribute, "") == a.Value || (a.Contains && ndl.GetAttributeValue(a.Attribute, "").Contains(a.Value))))
                        return ndl;
                    var nd = GetNode(ndl, pAttributes);
                    if (nd != null)
                        return nd;
                }
                return null;
            }
            return null;
        }
    }

    public class SCTableRow : XIWSTableRow
    {

        public SCTableRow(HtmlNode pNode)
        {
            _Node = pNode;
            _Columns = new List<XIWSTableColumn>();
        }

        private HtmlNode _Node;
        private List<XIWSTableColumn> _Columns;

        public List<XIWSTableColumn> Column => _Columns;

        public string GetAttribute(string pAttribute)
        {
            return _Node.GetAttributeValue(pAttribute, "");
        }
    }

    public class SCTableElement : XIWSTableElement
    {
        public SCTableElement(XIWSPage pPage, IElementHandle pHandle)
        {
            _Page = pPage;
            _Handle = pHandle;
        }

        private HtmlDocument _Doc;
        private XIWSPage _Page;
        private IElementHandle _Handle;
        private List<XIWSTableRow> _Rows = new List<XIWSTableRow>();
        private List<XIWSTableColumn> _Header = new List<XIWSTableColumn>();

        public void Parse(String pBody = "tbody", String pHeader = "th", String pHeaderData = "td")
        {
            if (_Handle == null)
                return;
            try
            {
                _Doc = new HtmlDocument();
                var html = _Handle.InnerHTMLAsync().Result;
                _Doc.LoadHtml(html);
                var body = _Doc.DocumentNode.ChildNodes.FirstOrDefault(n => n.Name == pBody) ?? _Doc.DocumentNode;
                for (int i = 0; i < body.ChildNodes.Count; i++)
                {
                    var cn = body.ChildNodes[i];
                    if (i == 0 && cn.Name == pHeader)
                    {
                        ParseHeader(cn, pHeaderData);
                        continue;
                    }
                    if (cn.Name == "tr")
                        ParseRow(cn);
                }
            }
            catch (Exception pEx)
            {
                XLog.AddException(pEx);
                _Doc = null;
            }
        }

        private void ParseRow(HtmlNode pNode)
        {
            var th = new SCTableRow(pNode);
            _Rows.Add(th);
            for (int i = 0; i < pNode.ChildNodes.Count; i++)
            {

                var item = pNode.ChildNodes[i];
                if (item.Name != "td")
                    continue;
                var col = new SCTableColumn(item);
                if (th.Column.Count <= Header.Count)
                    col.Hader = Header[th.Column.Count];
                th.Column.Add(col);
            }
        }

        private void ParseHeader(HtmlNode pNode, String pHeaderData)
        {
            foreach (var item in pNode.ChildNodes)
            {
                if (item.Name != "td" && item.Name != pHeaderData)
                    continue;
                var th = new SCTableColumn(item);
                _Header.Add(th);
            }
        }

        public List<XIWSTableRow> Rows => _Rows;

        public List<XIWSTableColumn> Header => _Header;

        public Boolean IsValid => _Handle != null && _Doc != null;
    }
}
