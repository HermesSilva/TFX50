using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Playwright;

using TFX.Core.Test.Browsers;
using TFX.Core.Test.Interfaces;

namespace TFX.Core.Test.PlayWright
{
    public sealed class XPlayWrightBrowser : XIWSBrowser
    {
        public static bool CancelCheck = false;
        internal void Initialize(XSCBrowserConfig pConfig)
        {
            Config = pConfig;
            Instance = Playwright.CreateAsync().Result;
            _EventTimeout = new Timer(DoEventTimeout);
        }

        internal IPlaywright Instance;
        private IPage _Page;
        private XIWSPage _CurrentPage;
        private IBrowserContext _Context;
        private Action _Loaded;
        private string _LastEventTitle;
        public XSCBrowserConfig Config;
        private Timer _EventTimeout;
        public event Action<String> GlobalTimeout;

        public void AddLoadEvent(Action pAction, Int32 pTimeout = BasePlayWrightPage.GlobalTimeout, string pTitle = "")
        {
            CancelCheck = false;
            _Loaded = pAction;
            _LastEventTitle = pTitle;
            _EventTimeout.Change(pTimeout, Timeout.Infinite);
        }

        public async void GoBackAsync()
        {
            await _Page.GoBackAsync();
        }

        private void DoEventTimeout(object state)
        {
            try
            {
                CancelCheck = true;
                XLog.AddLog($"Aconteceu timeout ao aguardar o evento [{_LastEventTitle}].");
            }
            catch
            {
            }
            finally
            {
                try
                {
                    GlobalTimeout?.Invoke(_LastEventTitle);
                }
                catch
                {
                }
            }
        }

        private void FireLoaded()
        {
            if (_Loaded == null)
                return;
            _EventTimeout.Change(Timeout.Infinite, Timeout.Infinite);
            ThreadPool.QueueUserWorkItem((a) =>
            {
                try
                {
                    var l = _Loaded;
                    _Loaded = null;
                    l.Invoke();
                }
                catch (Exception ex)
                {
                    try
                    {
                        XLog.AddException(ex);
                    }
                    catch
                    {
                    }
                }
            });
        }

        public XIWSPage CurrentPage => _CurrentPage;

        public XIWSPage SetCurrentPage(XIWSPage pPage)
        {
            _CurrentPage = pPage;
            return pPage;
        }

        public XIWSPage SelectPage(string pTitle, int pMillisecondsTimeout = 30 * 1000)
        {
            while (pMillisecondsTimeout > 0)
            {
                var pg = _Context.Pages.FirstOrDefault(p => p.TitleAsync().Result == pTitle);
                if (pg != null)
                    return new XPlayWrightPage(this, pg);
                Thread.Sleep(1000);
                pMillisecondsTimeout -= 1000;
            }
            return null;
        }

        public async Task GotoAsync(String pURL)
        {
            if (_Page == null)
            {
                if (_Context.Pages.Count == 1)
                    _Page = _Context.Pages[0];
                else
                    _Page = await _Context.NewPageAsync();
                _Page.Load -= LocalLoaded;
                _CurrentPage = new XPlayWrightPage(this, _Page);
                _Page.Load += LocalLoaded;
                _Page.PageError += PageError;
            }
            await _Page.GotoAsync(pURL, new()
            {
                Timeout = 2 * 60 * 1000
            });
        }

        private void PageError(object sender, string pError)
        {
            Debug.WriteLine(pError);
        }

        private void LocalLoaded(object sender, IPage e)
        {
            FireLoaded();
        }

        public async Task StartAsync()
        {
            var path = Config.GroupProfile.IsFull() ? Path.Combine(XWSUtils.AppPath, Config.GroupProfile, Config.UserProfile) : "";
            if (path.IsFull())
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                if (Config.Channel.SafeLower() == "chromium" || Config.Channel.SafeLower() == "chrome")
                {
                    _Context = await Instance.Chromium.LaunchPersistentContextAsync(path, new()
                    {
                        Headless = Config.Headless,
                        Channel = Config.Channel,
                        SlowMo = 0,
                    });

                }
                else
                {
                    if (Config.Channel.SafeLower() == "firefox")
                    {
                        _Context = await Instance.Firefox.LaunchPersistentContextAsync(path, new()
                        {
                            Headless = Config.Headless,
                            //Channel = Config.Channel,
                            SlowMo = 0,
                        });

                    }
                    else
                    {
                        _Context = await Instance.Webkit.LaunchPersistentContextAsync(path, new()
                        {
                            Headless = Config.Headless,
                            //Channel = Config.Channel,
                            SlowMo = 0,
                        });
                    }
                }
            }
            else
            {
                IBrowser browser;
                if (Config.Channel.SafeLower() == "chromium" || Config.Channel.SafeLower() == "chrome")
                {
                    browser = await Instance.Chromium.LaunchAsync(new()
                    {
                        Headless = Config.Headless,
                        Channel = Config.Channel,
                        SlowMo = 0,
                    });
                }
                else
                {
                    if (Config.Channel.SafeLower() == "firefox")
                    {
                        browser = await Instance.Firefox.LaunchAsync(new()
                        {
                            Headless = Config.Headless,
                            //Channel = Config.Channel,
                            SlowMo = 0,
                        });
                    }
                    else
                    {
                        browser = await Instance.Webkit.LaunchAsync(new()
                        {
                            Headless = Config.Headless,
                            //Channel = Config.Channel,
                            SlowMo = 0,
                        });
                    }
                }
                await browser.NewPageAsync();
                _Context = browser.Contexts.FirstOrDefault();
            }

        }

        public void Dispose()
        {
            Instance?.Dispose();
            _Context?.DisposeAsync();
        }
    }
}
