using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ContactForm.Sample.Services
{
    public interface IPostService
    {
        bool Post(string uri, HttpContent content);
    }

    public class PostService: IPostService
    {
        private readonly ILogger<PostService> logger;
        private readonly HttpClient client;

        public PostService(ILogger<PostService> logger, HttpClient client)
        {
            this.logger = logger;
            this.client = client;
        }

        public bool Post(string uri, HttpContent content)
        {
            if (string.IsNullOrEmpty(uri)) return true;
            try
            {
                var result = client.PostAsync(uri, content).GetAwaiter().GetResult();
                return result.IsSuccessStatusCode;
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "PostService exception");
                return false;
            }
        }
    }
}
