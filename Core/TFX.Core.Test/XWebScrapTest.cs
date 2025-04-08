using System;
using System.Threading;

using TFX.Core.Model;
using TFX.Core.Test.Factories;
using TFX.Core.Test.Interfaces;

using Xunit;

namespace TFX.Access
{
    public sealed class XWebScrapSetup : IDisposable
    {

        public Action<XIWSBrowser> OnDispose;
        public XIWSBrowser Browser;
        private ManualResetEvent _Event;

        public void Dispose()
        {
            OnDispose?.Invoke(Browser);
        }

        public void Prepare()
        {
        }

        internal void Initialize(Action<XIWSBrowser> pDisposeSetup, Func<XIWSBrowser> pInitializeSetup)
        {
            if (OnDispose == null)
            {
                OnDispose = pDisposeSetup;
                Browser = pInitializeSetup?.Invoke();
            }
            if (_Event == null)
                _Event = new ManualResetEvent(false);
            else
            {
                _Event.WaitOne();
                _Event.Reset();
            }
        }
        public void Wait()
        {
            _Event?.WaitOne();
        }

        public void Continue()
        {
            _Event.Set();
        }
    }

    public abstract class XWebScrapTest<AppModel, Rule, Tuple> : XTest<Rule, Tuple>, IClassFixture<XWebScrapSetup>
        where AppModel : XSAMApplication, new()
        where Tuple : XServiceDataTuple
        where Rule : XTestRule<Tuple>, new()
    {
        protected XWebScrapTest(XWebScrapSetup pSetup)
        {
            Setup = pSetup;
            Setup.Initialize(DoDisposeSetup, DoInitializeSetup);
            App = new AppModel();
        }

        protected virtual void DoDisposeSetup(XIWSBrowser pBrowser)
        {
        }

        protected virtual XIWSBrowser DoInitializeSetup()
        {
            var browser = XWSFactory.CreateBrowser(XSCDriver.PalyWright, new XSCBrowserConfig { Channel = "chrome", Headless = false });
            return browser;
        }

        public XWebScrapSetup Setup
        {
            get;
        }
        public AppModel App
        {
            get;
        }
    }
}
