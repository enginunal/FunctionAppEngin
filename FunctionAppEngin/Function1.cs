using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace FunctionAppEngin
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger fonksiyonuna istek geldi.");
            string result;

            switch (req.Method.Method)
            {
                case "GET":
                    result = GetMethodSample(req.GetQueryNameValuePairs());
                    break;
                case "POST":
                    result = PostMethodSample(await req.Content.ReadAsAsync<object>());
                    break;
                default:
                    //zaten HttpTrigger attribute'unda geet ve post tanýmý yapýldýðý için buraya düþmeyecektir. 
                    return req.CreateResponse(HttpStatusCode.NotImplemented, new
                    {
                        durum = "sorun var",
                        mesaj = "Sadece get ve post metodlarý desteklenmekte."
                    });
            }
            
            return result == null
                ? req.CreateResponse(HttpStatusCode.BadRequest, new
                {
                    durum = "sorun var",
                    mesaj = "ad girilmesi gerekmektedir. Düzeltip tekrar deneyin."
                })
                : req.CreateResponse(HttpStatusCode.OK, new
                {
                    durum = "Tamam",
                    mesaj = "Merhaba, " + result + " test fonksiyonuna hoþ geldiniz."
                });
        }

        private static string PostMethodSample(object contentVal)
        {
            return ((dynamic)contentVal)?.ad;
        }

        private static string GetMethodSample(IEnumerable<KeyValuePair<string, string>> valuePairs)
        {
            return valuePairs.FirstOrDefault(q => String.Compare(q.Key, "ad", true) == 0).Value;
        }


    }
}
