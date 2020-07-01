using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IogServices.Util;

namespace IogServices.Services.Impl
{
    public class ForwarderSenderService : IForwarderSenderService
    {
        public async Task <HttpResponseMessage> SendPost(ForwarderMessage forwarderMessage)
        {
            try
            {
                var content = MakeMessageBody(forwarderMessage.MessageContent);
                using (var handler = new HttpClientHandler())
                {
                    handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                    var httpClient = new HttpClient(handler);
                    var response = await httpClient.PostAsync(forwarderMessage.Uri, content);
                    var teste = response.Content.ReadAsStringAsync().Result;

                    return response;
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public HttpResponseMessage SendPut(ForwarderMessage forwarderMessage)
        {
            try
            {
                var content = MakeMessageBody(forwarderMessage.MessageContent);
                using (var handler = new HttpClientHandler())
                {
                    handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                    Task<HttpResponseMessage> response;
                    using (var client = new HttpClient(handler))
                    {
                        response = client.PutAsync(forwarderMessage.Uri, content);
                        response.Wait();
                    }

                    return response.Result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public HttpResponseMessage SendPatch(ForwarderMessage forwarderMessage)
        {
            try
            {
                var content = MakeMessageBody(forwarderMessage.MessageContent);
                using (var handler = new HttpClientHandler())
                {
                    handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                    Task<HttpResponseMessage> response;
                    using (var client = new HttpClient(handler))
                    {
                        response = client.PatchAsync(forwarderMessage.Uri, content);
                        response.Wait();
                    }

                    return response.Result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public HttpResponseMessage SendDelete(ForwarderMessage forwarderMessage)
        {
            try
            {
                using (var handler = new HttpClientHandler())
                {
                    handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                    Task<HttpResponseMessage> response;
                    using (var client = new HttpClient(handler))
                    {
                        response = client.DeleteAsync(forwarderMessage.Uri);
                        response.Wait();
                    }

                    return response.Result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static StringContent MakeMessageBody(string payload)
        {
            var stringContent = new StringContent(payload, Encoding.UTF8, "application/json");
            return stringContent;
        }
    }
}