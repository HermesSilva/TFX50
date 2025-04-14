using System.Text;
using System.Text.Json;

using TFX.Core.Access.Service;
using TFX.Core.IDs.Model;

namespace StressTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("EXECUTANDO");
            LoginParallel();
            Console.WriteLine("Stress Finalizado");
            Console.ReadLine();
        }

        static int _Count;
        static int _Error;
        static long _Start;
        private static XUser[] _Data;
        static int _Running = 0;
        private static bool _Wait = true;
        private static long _RAM;

        public static void LoginParallel()
        {
            _Start = DateTime.Now.Ticks;
            Console.WriteLine("Carregando Usuários");
            using var svc = new XLoginService();
            svc.RefreshCache();
            Thread.Sleep(5_000);
            Do(0);

            for (int i = 0; i < 30; i++)
            {
                _Running++;
                ThreadPool.QueueUserWorkItem((a) => Do(0));
            }
            Console.WriteLine($"Iniciando Stress {DateTime.Now}");
            Statistic();
        }

        private static void Statistic()
        {
            while (_Wait)
            {
                var et = new TimeSpan(DateTime.Now.Ticks - _Start);

                if (et.Seconds > 0)
                    Console.WriteLine($"Count {_Count.ToString("#,##0")} Login/s {(_Count / et.TotalSeconds).ToString("#,##0")} RAM Use {_RAM.ToString("#,##0")}MB Errors {_Error.ToString("#,##0")} ");
                Thread.Sleep(5000);
                if (_Running == 0)
                {
                    Console.WriteLine($"Finalizado {DateTime.Now} Duração {et}");
                    _Wait = false;
                }
            }
        }

        private static void Do(int i)
        {
            try
            {
                using var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };
                using HttpClient client = new HttpClient(handler);
                for (int j = 0; j < 1000000; j++)
                {
                    try
                    {

                        using var st = new StringContent("{ \"Login\": \"Administrador\" }", Encoding.UTF8, "application/json");
                        using HttpResponseMessage response = client.PostAsync("https://tootegaws:7000/Access/Login", st).Result;
                        response.EnsureSuccessStatusCode();
                        var data = response.Content.ReadAsStringAsync().Result;
                        XUserSession res = JsonSerializer.Deserialize<XUserSession>(data);
                        if (res == null)
                            _Error++;
                        else
                            _Count++;
                    }
                    catch
                    {
                        _Error++;
                    }
                }
            }
            finally
            {
                _Running--;
            }
        }
    }
}
