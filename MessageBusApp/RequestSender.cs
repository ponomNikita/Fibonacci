using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using log4net;

namespace MessageBusApp
{
    public class RequestSender
    {
        private readonly string _baseUrl;
        private static readonly ILog Logger = LogManager.GetLogger(typeof(RequestSender));


        public RequestSender(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public object SentGetRequest<TResult>(string url)
        {
            try
            {
                Logger.DebugFormat("Отправка GET запроса по адресу {0}/{1}", _baseUrl, url);

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseUrl);

                    var response = client.GetAsync(url).Result;

                    response.EnsureSuccessStatusCode();

                    var result = response.Content.ReadAsAsync<TResult>().Result;

                    Logger.Debug("GET запрос успешно отправлен");

                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Ошибка при отправке GET запроса {0}", ex);
                return null;
            }
        }


    }
}
