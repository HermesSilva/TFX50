using System;
using System.Threading.Tasks;

using TFX.Core.Test.PlayWright;

namespace TFX.Core.Test.Interfaces
{

    public class XSCBrowserConfig
    {
        public string UserProfile
        {
            get; set;
        }
        public string GroupProfile
        {
            get; set;
        }
        public bool? Headless
        {
            get; set;
        }
        public string Channel
        {
            get; set;
        }
    }
    public interface XIWSBrowser : IDisposable
    {
        event Action<String> GlobalTimeout;
        void AddLoadEvent(Action pAction, Int32 pTimeout = BasePlayWrightPage.GlobalTimeout, string pTitle = "");
        Task GotoAsync(String pURL);
        void GoBackAsync();
        Task StartAsync();
        XIWSPage CurrentPage
        {
            get;
        }
        XIWSPage SetCurrentPage(XIWSPage pPage);
        XIWSPage SelectPage(string pTitle, int pMillisecondsTimeout = 30 * 1000);

    }
}
