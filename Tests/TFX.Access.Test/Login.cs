using System.Net;
using System.Text;

using Launcher;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

using TFX.Core.Cache;


namespace TAF.Access.Test
{
    public class LoginSetup
    {
        public LoginSetup()
        {
        }

        public WebApplication App;
        public void Open()
        {
            Program.Main(null);
            App = Program.App;
        }

    }

    public class Login : IClassFixture<LoginSetup>
    {
        private LoginSetup _Setup;

        public Login(LoginSetup pSetup)
        {
            _Setup = pSetup;
        }

        [Fact]
        public void LoginParallel()
        {
            _Setup.Open();
            Do(0);
            //for (int i = 0; i < 10; i++)
            //    ThreadPool.QueueUserWorkItem((a) => Do(0));
            Console.ReadLine();
        }

        private void Do(int i)
        {
            for (int j = 0; j < 1000000; j++)
            {

                try
                {
                    using var st = new StringContent("{ \"Login\": \"Maria\" }", Encoding.UTF8, "application/json");
                    using HttpClient client = new HttpClient();

                    using HttpResponseMessage response = client.PostAsync("http://localhost:7000/Access/Login", st).Result;
                    response.EnsureSuccessStatusCode();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }
            }
        }

        [Fact]
        public void Authorization()
        {

            using IServiceScope scop = _Setup.App.Services.CreateScope();

            var context = new DefaultHttpContext();
            context.RequestServices = scop.ServiceProvider;
            var session = XSessionManager.DoLogin(null);
            Assert.True(XSessionManager.CheckLogin(context));
        }

        //private AssignmentController CreateTarget()
        //{
        //    var context = new DefaultHttpContext
        //    {
        //        User = new GenericPrincipal(new GenericIdentity("username"), Array.Empty<string>())
        //    };
        //    var contextController = new ControllerContext() { HttpContext = context };
        //    target.ControllerContext = contextController;
        //    return target;
        //}
    }
}
