using System.Net.Http.Headers;
using System.Reflection;

using TFX.Core;
using TFX.Core.Model;

using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace TFX.Core.Test.Setup
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class XTestPriorityAttribute : Attribute
    {
        public int Priority
        {
            get;
        }
        public XTestPriorityAttribute(int priority) => Priority = priority;
    }


    public class XPriorityOrderer : ITestCaseOrderer
    {

        public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases) where TTestCase : ITestCase
        {
            var ar = testCases.ToArray();
            return testCases.OrderBy(tc => NewMethod(tc));
        }

        private static int NewMethod(ITestCase tc)
        {
            var m = tc.TestMethod.Method;
            MethodInfo mi = XUtils.GetValue(m, "MethodInfo") as MethodInfo;
            var att = mi?.GetCustomAttribute<XTestPriorityAttribute>();
            return att == null ? 0 : att.Priority;
        }
    }


    [TestCaseOrderer("TFX.Core.Test.Setup.PriorityOrderer", "TFX.Core.Test.Setup")]
    public class XBaseTest : IClassFixture<XServerSidePrepare>
    {
        private IServiceScope _Scope;

        public XBaseTest(XServerSidePrepare pPrepare)
        {
            pPrepare.Prepare += InternalPrepare;
            pPrepare.Unprepare += InternalUnprepare;
            pPrepare.DoPrepare(this);
            _Scope = XEnvironment.Services.CreateScope();
        }

        protected IServiceProvider ServiceProvider => _Scope.ServiceProvider;

        public async Task<T> DoCall<T>((HttpClient Client, HttpRequestMessage Request) pObject) where T : new()
        {
            using (var response = await pObject.Client.SendAsync(pObject.Request))
            {
                //response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsByteArrayAsync();
                var dst = XUtils.Deserialize<T>(body);
                return dst;
            }
        }

        public (HttpClient Client, HttpRequestMessage Request) PrepareClient(string pURL, Object pFilter)
        {
            var client = new HttpClient();
            var json = "null";
            if (pFilter != null)
                json = XUtils.SerializeString(pFilter);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(pURL),
                Content = new StringContent(json)
                {
                    Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
                }
            };
            return (client, request);
        }

        private void InternalPrepare()
        {
            Prepare();
        }

        private void InternalUnprepare()
        {
            Unprepare();
        }

        protected virtual void Unprepare()
        {

        }

        protected virtual void Prepare()
        {

        }

        internal protected virtual void BeforePrepare(XServerSidePrepare pServerSide)
        {
            pServerSide.OnAppBuild = AppBuild;
            pServerSide.OnAppPrepare = AppPrepare;
        }

        protected virtual void AppPrepare(WebApplicationBuilder pBuilder)
        {
        }

        protected virtual void AppBuild(WebApplication pApp)
        {
        }
    }

    public class XServerSidePrepare : IDisposable
    {

        public Action Prepare;
        public Action Unprepare;
        public Action<WebApplicationBuilder> OnAppPrepare;
        public Action<WebApplication> OnAppBuild;
        public void Dispose()
        {
            DoUnprepare();
        }

        internal protected virtual void DoPrepare(XBaseTest pTest)
        {
            pTest.BeforePrepare(this);
            Program.Main(OnAppPrepare, OnAppBuild);
            Prepare?.Invoke();
        }

        protected virtual void DoUnprepare()
        {
            Unprepare?.Invoke();
        }
    }

    public class Program : Launcher.Program
    {

        private static bool _Initialized;
        public static void Main(Action<WebApplicationBuilder> pOnAppPrepare, Action<WebApplication> OnAppBuild)
        {
            if (_Initialized)
                return;
            _Initialized = true;
            Launcher.Program.IsAsync = true;
            Launcher.Program.Main(null);
        }
    }
}
