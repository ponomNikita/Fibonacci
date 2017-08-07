using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.SelfHost;
using log4net;
using log4net.Core;

namespace RestApiApp
{
    class Program
    {
        private static readonly ILog Logger = log4net.LogManager.GetLogger("RestApiAppProgram");

        static void Main(string[] args)
        {
            try
            {
                RunServer();
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Ошибка при запуске сервера. {0}", ex);
                Console.ReadLine();
            }
        }

        private static void RunServer()
        {
            var hostUrl = ConfigurationManager.AppSettings["baseUrl"];
            hostUrl = !string.IsNullOrEmpty(hostUrl) ? hostUrl : "http://localhost:8888";

            var config = new HttpSelfHostConfiguration(hostUrl);

            config.Routes.MapHttpRoute(
                "API Default", "api/{controller}/{id}",
                new { id = RouteParameter.Optional });

            using (HttpSelfHostServer server = new HttpSelfHostServer(config))
            {
                Logger.Info("Запуск сервера");
                server.OpenAsync().Wait();
                Logger.Info("Сервер успешно запущен");
                Console.ReadLine();
            }
        }
    }
}
