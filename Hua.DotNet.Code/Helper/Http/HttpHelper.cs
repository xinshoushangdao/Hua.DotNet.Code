using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Hua.DotNet.Code.Helper.Http
{
    /// <summary>
    /// Http帮助类
    /// </summary>
    public class HttpHelper : IHttpHelper
    {
        public HttpClientHandler Handler;
        private readonly ILogger<HttpHelper> _logger;

        public HttpHelper(ILogger<HttpHelper>? logger)
        {
            _logger = logger;
            Handler = new HttpClientHandler();
        }

        public virtual HttpClient GetHttpClient()
        {
            return new HttpClient(Handler) { };
        }

        /// <summary>
        /// PostAsync
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="url"></param>
        /// <param name="input"></param>
        /// <param name="timeOut">超时时间</param>
        /// <returns></returns>
        public async Task<TOut?> PostAsync<TIn, TOut>(string? url, TIn input, int timeOut = 10)
        {
            try
            {
                var client = GetHttpClient();
                client.Timeout = new TimeSpan(0, 10, timeOut);
                _logger.LogDebug($"Post:{url}\n[{JsonConvert.SerializeObject(input)}]");
                var result = await client.PostAsync(url, JsonContent.Create(input));
                var strResult = await result.Content.ReadAsStringAsync();
                if (result.StatusCode != HttpStatusCode.OK)
                {
                    _logger.LogDebug($"Error[{result.StatusCode}]:{url}\t{strResult}");
                }

                return await result.Content.ReadFromJsonAsync<TOut>();
            }
            catch (Exception e)
            {
                _logger.LogError(url);
                _logger.LogError(e.Message);
                throw;
            }
        }

        /// <summary>
        /// PostAsync
        /// </summary>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="url"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public async Task<TOut?> GetAsync<TOut>(string? url, int timeOut = 10)
        {
            try
            {
                var client = GetHttpClient();
                client.Timeout = new TimeSpan(0, 10, timeOut);
                _logger.LogDebug($"Get:{url}");
                var result = await client.GetAsync(url);
                var strResult = await result.Content.ReadAsStringAsync();
                if (result.StatusCode != HttpStatusCode.OK)
                {
                    _logger.LogDebug($"Error[{result.StatusCode}]:{url}\t{strResult}");
                }

                return await result.Content.ReadFromJsonAsync<TOut>();
            }
            catch (Exception e)
            {
                _logger.LogError(url);
                _logger.LogError(e.Message);
                throw;
            }
        }


        #region 下载文件

        /// <summary>
        /// http下载文件 (仅支持小文件)
        /// </summary>
        /// <param name="url">下载文件地址</param>
        /// <param name="localPath">文件存放地址，包含文件名</param>
        /// <returns></returns>
        public bool DownloadFile(string url, string localPath)
        {
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;

            var request = WebRequest.Create(url) as HttpWebRequest;
            Stream stream = new FileStream(localPath, FileMode.CreateNew);
            try
            {
                // 设置参数
                //发送请求并获取相应回应数据
                var response = request?.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                var responseStream = response?.GetResponseStream();
                //创建本地文件写入流
                stream.Close();
                responseStream?.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion
    }

    public interface IHttpHelper
    {
        public Task<TOut?> PostAsync<TIn, TOut>(string? url, TIn input, int timeOut = 10);
        public Task<TOut?> GetAsync<TOut>(string? url, int timeOut = 10);
        public bool DownloadFile(string url, string fileFullName);
        public HttpClient GetHttpClient();
    }
}