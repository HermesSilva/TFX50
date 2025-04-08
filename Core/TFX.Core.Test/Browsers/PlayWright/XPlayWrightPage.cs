using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Playwright;

using TFX.Core.Test.Interfaces;

namespace TFX.Core.Test.PlayWright
{
    public abstract class BasePlayWrightPage : XIWSPage
    {
        public const Int32 GlobalShortTimeout = 10 * 1000;
        public const Int32 GlobalTimeout = 30 * 1000;
        public const Int32 GlobalTimeout2x = GlobalTimeout * 2;
        public const Int32 GlobalTimeout4x = GlobalTimeout * 4;
        public const Int32 GlobalTimeout10x = GlobalTimeout * 10;

        internal BasePlayWrightPage(XPlayWrightBrowser pBrowser, Object pPageFrame)
        {
            _Browser = pBrowser;
            _Page = pPageFrame as IPage;
            _Frame = pPageFrame as IFrame;
        }
        private XPlayWrightBrowser _Browser;
        private IPage _Page;
        private IFrame _Frame;
        private Random _Random = new Random();

        public event EventHandler<XIWSPage> Loaded;
        public void Evaluate(string pScrit)
        {
            var x = _Page != null ? _Page.EvaluateAsync(pScrit).Result : _Frame.EvaluateAsync(pScrit).Result;
        }

        public async void ScreenshotToFile(String pFileName)
        {
            if (_Page != null)
                await _Page.ScreenshotAsync(new PageScreenshotOptions { Path = pFileName, FullPage = true });
            if (_Frame != null)
                await _Frame.Page.ScreenshotAsync(new PageScreenshotOptions { Path = pFileName, FullPage = true });
        }

        public Task Press(string pCommand)
        {
            return _Page.Keyboard.PressAsync(pCommand);
        }

        private void FireLoaded(XIWSPage pPage)
        {
            Loaded?.Invoke(this, pPage);
        }

        public Boolean? CheckSelectItems(String pTag, Int32 pMillisecondsTimeout = 10000, params XAtt[] pAttributes)
        {
            return CheckSelectItems(pTag, null, pMillisecondsTimeout, pAttributes);
        }
        public Boolean? CheckSelectItems(String pTag, String pMessage = null, Int32 pMillisecondsTimeout = 10000, params XAtt[] pAttributes)
        {
            try
            {
                IReadOnlyList<ILocator> list;
                while (pMillisecondsTimeout > 0)
                {
                    if (XPlayWrightBrowser.CancelCheck)
                        return false;
                    if (_Page != null)
                        list = _Page.Locator(pTag).AllAsync().Result;
                    else
                        list = _Frame.Locator(pTag).AllAsync().Result;
                    var ret = list.FirstOrDefault(s => ValidateAttribute(pAttributes, s));
                    if (ret != null)
                    {
                        if (pAttributes.Any(a => a.IsVisible) && !ret.IsVisibleAsync().Result)
                            return false;
                        if (ret.Locator("option").AllAsync().Result.Count() > 1)
                            return true;
                    }
                    Thread.Sleep(1000);
                    pMillisecondsTimeout -= 1000;
                }
            }
            catch (Exception pEx)
            {
                if (!(pEx?.InnerException?.Message?.Contains("Execution context was destroyed") == true))
                    throw new Exception($"Falha ao procurar elemento {pTag}", pEx);
            }
            if (pMessage.IsFull())
                throw new TimeoutException(pMessage);
            return false;
        }


        public Boolean? GetSelect(String pTag, out XIWSElement pElement, Int32 pMillisecondsTimeout = 10000, params XAtt[] pAttributes)
        {
            return GetSelect(pTag, out pElement, null, pMillisecondsTimeout, pAttributes);
        }

        public Boolean? GetSelect(String pTag, out XIWSElement pElement, String pMessage = null, Int32 pMillisecondsTimeout = 10000, params XAtt[] pAttributes)
        {
            pElement = null;
            ILocator elm = null;
            try
            {
                IReadOnlyList<ILocator> list;
                while (pMillisecondsTimeout > 0)
                {
                    if (XPlayWrightBrowser.CancelCheck)
                        break;
                    if (_Page != null)
                        list = _Page.Locator(pTag).AllAsync().Result;
                    else
                        list = _Frame.Locator(pTag).AllAsync().Result;
                    var ret = list.FirstOrDefault(s => ValidateAttribute(pAttributes, s));
                    if (ret != null)
                    {
                        elm = ret;
                        if (pAttributes.Any(a => a.IsVisible) && !ret.IsVisibleAsync().Result)
                        {
                            elm = null;
                            break;
                        }
                        break;
                    }
                    Thread.Sleep(1000);
                    pMillisecondsTimeout -= 1000;
                }
                if (elm != null)
                {
                    elm = elm.Locator("..").Locator("span").AllAsync().Result.FirstOrDefault(e => e.GetAttributeAsync("class").Result.Contains("select2-container"));
                    pElement = new XPlayRightElement(this, elm);
                    return true;
                }
            }
            catch (Exception pEx)
            {
                if (!(pEx?.InnerException?.Message?.Contains("Execution context was destroyed") == true))
                    throw new Exception($"Falha ao procurar elemento {pTag}", pEx);
            }
            if (pMessage.IsFull())
                throw new TimeoutException(pMessage);
            return false;
        }

        public Boolean? CheckElement(String pTag, out XIWSElement pElement, String pMessage = null, Int32 pMillisecondsTimeout = 10000, params XAtt[] pAttributes)
        {
            pElement = null;
            try
            {
                IReadOnlyList<ILocator> list;
                while (pMillisecondsTimeout > 0)
                {
                    if (XPlayWrightBrowser.CancelCheck)
                        return null;
                    if (_Page != null)
                        list = _Page.Locator(pTag).AllAsync().Result;
                    else
                        list = _Frame.Locator(pTag).AllAsync().Result;
                    var ret = list.FirstOrDefault(s => ValidateAttribute(pAttributes, s));
                    if (ret != null)
                    {
                        pElement = new XPlayRightElement(this, ret);
                        if (pAttributes.Any(a => a.IsVisible) && !ret.IsVisibleAsync().Result)
                            return false;
                        return true;
                    }
                    Thread.Sleep(1000);
                    pMillisecondsTimeout -= 1000;
                }
            }
            catch (Exception pEx)
            {
                if (!(pEx?.InnerException?.Message?.Contains("Execution context was destroyed") == true))
                    throw new Exception($"Falha ao procurar elemento {pTag}", pEx);
            }
            if (pMessage.IsFull())
                throw new TimeoutException(pMessage);
            return false;
        }

        private static bool ValidateAttribute(XAtt[] pAttributes, ILocator pElement)
        {
            foreach (var a in pAttributes)
            {
                string att;
                if (a.Attribute == "value")
                    att = (pElement.InputValueAsync().Result ?? pElement.GetAttributeAsync(a.Attribute).Result) ?? string.Empty;
                else
                    att = pElement.GetAttributeAsync(a.Attribute).Result ?? string.Empty;
                att = att.SafeLower();
                bool containsInAtt = att == a.Value || (a.Contains && att.Contains(a.Value));
                if (!containsInAtt)
                    return false;

                if (a.InnexText.IsFull())
                {
                    var itxt = pElement.InnerTextAsync().Result ?? string.Empty;
                    itxt = itxt.SafeLower();
                    bool containsInInnerText = itxt == a.InnexText || (a.Contains && itxt.Contains(a.InnexText));
                    if (!containsInInnerText)
                        return false;
                }
            }
            return true;
        }

        public async Task WaitForNetworkIdle()
        {
            await _Page.WaitForLoadStateAsync(LoadState.NetworkIdle, new PageWaitForLoadStateOptions() { Timeout = GlobalTimeout2x });
        }

        public async Task HumanTypingAsync(String pValue, bool pUseTab = false)
        {
            for (int i = 0; i < pValue.Length; i++)
            {
                await _Page.Keyboard.TypeAsync(new String(pValue[i], 1));
                await Task.Delay(_Random.Next(10, 100));
            }
            if (pUseTab)
                await _Page.Keyboard.PressAsync("Tab");
        }

        public async Task TypingAsync(String pValue, bool pUseTab = false)
        {
            await _Page.Keyboard.InsertTextAsync(pValue);
            if (pUseTab)
                await _Page.Keyboard.PressAsync("Tab");
        }

        public Boolean? CheckElement(String pTag, out XIWSElement pElement, String pMessage = null, params XAtt[] pAttributes)
        {
            return CheckElement(pTag, out pElement, pMessage, 10000, pAttributes);
        }

        public Boolean? CheckElement(String pTag, out XIWSElement pElement, Int32 pMillisecondsTimeout = 10000, params XAtt[] pAttributes)
        {
            return CheckElement(pTag, out pElement, null, 10000, pAttributes);
        }

        public Boolean? CheckElement(String pTag, String pMessage = null, Int32 pMillisecondsTimeout = 10000, params XAtt[] pAttributes)
        {
            return CheckElement(pTag, out _, pMessage, pMillisecondsTimeout, pAttributes);
        }

        public Boolean? CheckElement(String pTag, Int32 pMillisecondsTimeout = 10000, params XAtt[] pAttributes)
        {
            return CheckElement(pTag, out _, null, pMillisecondsTimeout, pAttributes);
        }

        public Boolean? CheckElement(String pTag, params XAtt[] pAttributes)
        {
            return CheckElement(pTag, out _, null, 10000, pAttributes);
        }

        public XIWSFrame GetFrameByName(String pName, Int32 pTimeout = GlobalTimeout)
        {
            var to = pTimeout;
            while (to > 0)
            {
                foreach (var frame in _Page.Frames)
                {
                    var frm = GetFrame(frame, pName);
                    if (frm != null)
                        return new XPlayWrightFrame(_Browser, frm);
                }
                static IFrame GetFrame(IFrame pFrame, String pName)
                {
                    foreach (var fm in pFrame.ChildFrames)
                    {
                        if (fm.Name == pName)
                            return fm;
                        var frm = GetFrame(fm, pName);
                        if (frm != null)
                            return frm;
                    }
                    return null;
                }
                Thread.Sleep(1000);
                to -= 1000;
            }
            throw new TimeoutException($"Não foi encontrado o Frame \"{pName}\".");
        }

        public async Task<FileStream> DownloadFromJS(XIWSPage pSource, string pScript, string pName = null)
        {
            var downloadtask = _Page.WaitForDownloadAsync();
            pSource.Evaluate(pScript);
            var download = downloadtask.Result;
            Console.WriteLine(download.PathAsync().Result);
            var path = Path.Combine(Path.GetTempPath(), _Browser.Config.GroupProfile, _Browser.Config.UserProfile);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            String fname = !download.SuggestedFilename.IsFull() ? Path.Combine(path, $"{Guid.NewGuid()}-{pName}.bin") : Path.Combine(path, $"{Guid.NewGuid()}-{pName}-{download.SuggestedFilename}");
            await download.SaveAsAsync(fname);
            return File.OpenRead(fname);
        }

        public async Task<FileStream> DownloadFrom(String pTag, params XAtt[] pAttributes)
        {
            var locator = _Page != null ? _Page.Locator(pTag).AllAsync().Result.FirstOrDefault(s => pAttributes.All(a => (s.GetAttributeAsync(a.Attribute).Result is String att) &&
                                                                                     ((a.Contains && att?.Contains(a.Value) == true) || (!a.Contains && att == a.Value)))) :
                                      _Frame.Locator(pTag).AllAsync().Result.FirstOrDefault(s => pAttributes.All(a => (s.GetAttributeAsync(a.Attribute).Result is String att) &&
                                                                                     ((a.Contains && att?.Contains(a.Value) == true) || (!a.Contains && att == a.Value))));
            if (locator == null)
                throw new Exception($"Elemento com Tag[{pTag}] e Atributos[{String.Join(",", pAttributes.Select(a => a.Attribute + "=" + a.Value))}] não foi encontrado.");

            var downloadtask = _Page.WaitForDownloadAsync();
            locator.ClickAsync().Wait();
            var download = downloadtask.Result;
            Console.WriteLine(download.PathAsync().Result);
            var path = Path.Combine(Path.GetTempPath(), _Browser.Config.GroupProfile, _Browser.Config.UserProfile);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            String fname = !download.SuggestedFilename.IsFull() ? Path.Combine(path, Guid.NewGuid() + ".bin") : Path.Combine(path, $"{Guid.NewGuid()}-{download.SuggestedFilename}");
            await download.SaveAsAsync(fname);
            return File.OpenRead(fname);
        }

        public XIWSElement GetByTagAttributes(String pTag, params XAtt[] pAttributes)
        {
            ILocator locator = GetElement(pTag, pAttributes);
            return new XPlayRightElement(this, locator);

        }

        private ILocator GetElement(string pTag, XAtt[] pAttributes)
        {
            ILocator locator = null;
            var to = GlobalTimeout;
            do
            {
                locator = _Page != null ? _Page.Locator(pTag).AllAsync().Result.FirstOrDefault(s => pAttributes.All(a => ((a.Attribute == "value" ? s.InputValueAsync().Result : s.GetAttributeAsync(a.Attribute).Result) is String att) &&
                                                                    ((a.Contains && att?.Contains(a.Value) == true) || (!a.Contains && att == a.Value)))) :
                                          _Frame.Locator(pTag).AllAsync().Result.FirstOrDefault(s => pAttributes.All(a => ((a.Attribute == "value" ? s.InputValueAsync().Result : s.GetAttributeAsync(a.Attribute).Result) is String att) &&
                                                                    ((a.Contains && att?.Contains(a.Value) == true) || (!a.Contains && att == a.Value))));
                if (locator != null)
                    break;
                Thread.Sleep(1000);
                to -= 1000;
            } while (to > 0);
            return locator;
        }

        public XIWSElement TestArea(params String[] pClassName)
        {
            if (_Page != null)
            {
                var waitForDownloadTask = _Page.WaitForDownloadAsync();
                _Page.GetByText("Download file").ClickAsync().RunSynchronously();
                var download = waitForDownloadTask.Result;
                Console.WriteLine(download.PathAsync().Result);
                download.SaveAsAsync("C:/Temp/at.txt").Wait();

                var xx = _Page.Locator(pClassName[0]).AllAsync().Result.FirstOrDefault(s => s.GetAttributeAsync(pClassName[1]).Result == pClassName[2]);
                return new XPlayRightElement(this, xx);
            }
            else
                return null;
        }

        public XIWSElement GetById(Guid pID)
        {
            return GetById(pID.ToString());
        }

        public async Task ShowTabAsync(Guid pID)
        {
            var elm = GetById($"E{pID}-hd");
            await elm.ClickToVisible();
        }

        public XIWSElement GetById(String pID)
        {
            ILocator element;
            var to = GlobalTimeout;
            do
            {
                element = _Page != null ? _Page.Locator($"[id='{pID}']") : _Frame.Locator($"[id='{pID}']");
                if (element != null)
                    break;
                Thread.Sleep(1000);
                to -= 1000;
            } while (to > 0);
            return new XPlayRightElement(this, element);
        }

        public XIWSElement GetByTitle(String pTitle)
        {
            if (_Page != null)
                return new XPlayRightElement(this, _Page.GetByLabel(pTitle));
            else
                return new XPlayRightElement(this, _Frame.GetByLabel(pTitle));
        }

        public XIWSElement GetByClass(String pClassName, String pText)
        {
            if (_Page != null)
                return new XPlayRightElement(this, _Page.Locator($"[class='{pClassName}']").Filter(new()
                {
                    HasText = pText
                }));
            else
                return new XPlayRightElement(this, _Frame.Locator($"[class='{pClassName}']").Filter(new()
                {
                    HasText = pText
                }));
        }

        public XIWSElement GetByClass(String pClassName)
        {
            if (_Page != null)
                return new XPlayRightElement(this, _Page.Locator($"[class='{pClassName}']"));
            else
                return new XPlayRightElement(this, _Frame.Locator($"[class='{pClassName}']"));
        }

        public async Task<XIWSTableElement> GetTableByAttributes(params XAtt[] pAttributes)
        {
            ILocator locator = GetElement("table", pAttributes);
            var handle = await locator.ElementHandleAsync();
            return new SCTableElement(this, handle);
        }

        public async Task<XIWSTableElement> GetTableBySelectorAsync(String pSelector)
        {
            IElementHandle element;
            var to = GlobalTimeout;
            do
            {
                element = _Page != null ? await _Page.QuerySelectorAsync(pSelector) : await _Frame.QuerySelectorAsync(pSelector);
                if (element != null)
                    break;
                Thread.Sleep(1000);
                to -= 1000;
            } while (to > 0);
            return new SCTableElement(this, element);

        }
    }

    internal class XPlayWrightPage : BasePlayWrightPage
    {
        internal XPlayWrightPage(XPlayWrightBrowser pBrowser, Object pPageFrame)
            : base(pBrowser, pPageFrame)
        {
        }
    }

    internal class XPlayWrightFrame : BasePlayWrightPage, XIWSFrame
    {
        internal XPlayWrightFrame(XPlayWrightBrowser pBrowser, Object pPageFrame)
            : base(pBrowser, pPageFrame)
        {
        }
    }
}

