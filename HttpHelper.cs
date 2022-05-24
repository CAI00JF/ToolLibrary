using Newtonsoft.Json;
using NLog;
using NLog.Fluent;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JFLibrary
{
    /// <summary>
    /// Http网络请求类
    /// Author:CJF
    /// Describe:使用Http网络请求
    /// </summary>
    public class HttpHelper
    {
        public HttpHelper()
        {
            httpClient.DefaultRequestHeaders.Add("Accept", "text/html, application/xhtml+xml, */*");
            httpClient.DefaultRequestHeaders.Add("ContentType", "application/json");
        }

        /// <summary>
        /// 创建HttpClient实例
        /// </summary>
        private static readonly HttpClient httpClient = new HttpClient(new SocketsHttpHandler()
        {
            AllowAutoRedirect = true,// 默认为true,是否允许冲顶定向
            MaxAutomaticRedirections = 50,//最多重定向几次,默认50次
            // MaxConnectionsPerServer = 100,//连接池中统一TcpServer的最大连接数
            UseCookies = false,// 是否自动处理cookie
        });

        #region[正式GET请求-V1.0]

        /// <summary>
        ///     正式GET请求-V1.0
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="headers">请求头</param>
        /// <returns></returns>
        public static string run_get_request(string url, IDictionary<string, string>? headers = null)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse? response = null;
            Stream? myResponseStream = null;
            StreamReader? myStreamReader = null;
            var _jsonVal = string.Empty;
            try
            {
                request.Method = "GET";
                request.Accept = "text/html, application/xhtml+xml, */*";
                request.ContentType = "application/json";
                request.Timeout = 10 * 1000;
                if (headers != null)
                    foreach (var item in headers)
                        request.Headers[item.Key] = item.Value;

                response = (HttpWebResponse)request.GetResponse();
                myResponseStream = response.GetResponseStream();
                myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
                _jsonVal = myStreamReader.ReadToEnd();
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (myStreamReader != null) myStreamReader.Close();

                if (myResponseStream != null) myResponseStream.Close();

                if (response != null) response.Close();

                if (request != null) request.Abort();
            }

            return _jsonVal;
        }

        #endregion

        #region [正式POST请求-V1.0]

        /// <summary>
        ///     正式POST请求-V1.0
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="param">参数</param>
        /// <param name="headers">请求头</param>
        /// <returns></returns>
        public static string run_post_request(string url, string param, IDictionary<string, string> headers = null)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse? response = null;
            Stream? myResponseStream = null;
            StreamReader? myStreamReader = null;
            var _jsonVal = string.Empty;
            try
            {
                //基础请求设置
                request.Method = "POST";
                request.Accept = "text/html, application/xhtml+xml, */*";
                request.ContentType = "application/json;charset=UTF-8";
                request.Timeout = 10 * 1000;
                //参数处理
                byte[] payload;
                payload = Encoding.UTF8.GetBytes(param);

                var writer = request.GetRequestStream();
                writer.Write(payload, 0, payload.Length);
                writer.Close();
                //网络请求
                response = (HttpWebResponse)request.GetResponse();
                myResponseStream = response.GetResponseStream();
                myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
                _jsonVal = myStreamReader.ReadToEnd();
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (myStreamReader != null) myStreamReader.Close();

                if (myResponseStream != null) myResponseStream.Close();

                if (response != null) response.Close();

                if (request != null) request.Abort();
            }

            return _jsonVal;
        }

        #endregion

        #region [Http 同步网络请求]
        /// <summary>
        /// HttpClient同步网络请求
        /// Author:蔡嘉福
        /// Describe:同步网络请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="param">请求参数</param>
        /// <param name="header">请求头(可为空)</param>
        /// <returns></returns>
        public static string HttpClientRequest(string url, string param,
            Dictionary<string, object>? header = null)
        {
            // 校验url是否为空
            if (string.IsNullOrEmpty(url)) { return string.Empty; }
            var id = Guid.NewGuid().ToString("N");
            Stopwatch? Stopwatch = new Stopwatch();
            Logs.LogWriter($"请求ID：{id} 【请求开始】请求地址：{url}");
            Logs.LogWriter($"请求ID：{id} 【请求开始】请求参数：{param}");
            string result = string.Empty;
            HttpResponseMessage? responceResult = null;
            try
            {
                Stopwatch.Start();
                // 发出HTTP请求并获取响应数据
                var response = httpClient.PostAsync(url, new StringContent(param, Encoding.UTF8, "application/json"));
                Logs.LogWriter($"请求ID：{id} 【请求开始】请求头：{JsonConvert.SerializeObject(httpClient.DefaultRequestHeaders)}");
                // 在这里会等待task返回。
                responceResult = response.Result;
                var httpResult = responceResult.Content.ReadAsStringAsync();
                result = httpResult.Result;
            }
            catch (Exception ex)
{
                Logs.Error(ex.ToString());
            }
            finally
            {
                if (responceResult != null) { responceResult.Dispose(); }
                Stopwatch.Stop();
}
            Logs.LogWriter($"请求ID：{id} 【请求结束】响应参数：{JsonConvert.SerializeObject(result)}");
            Logs.LogWriter($"请求ID：{id} 【请求结束】请求结束-相应时间：{Stopwatch.ElapsedMilliseconds}毫秒");
            return result;
        }
        #endregion

        #region [Http 异步网络请求]
        /// <summary>
        /// HttpClient异步网络请求
        /// Author:蔡嘉福
        /// Describe:异步网络请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="param">请求参数</param>
        /// <param name="header">请求头(可为空)</param>
        /// <returns></returns>
        public static async void HttpClientRequestAsync(string url, string param,
            Dictionary<string, object>? header = null)
        {
            // 校验url是否为空
            if (string.IsNullOrEmpty(url)) { return; }
            string result = string.Empty;
            HttpResponseMessage? response = null;
            try
            {
                // 发出HTTP请求并获取响应数据
                response = await httpClient.PostAsync(url, new StringContent(param, Encoding.UTF8, "application/json"));
                var httpResult = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Logs.Error(ex.ToString());
}
            finally
            {
                if (response != null)
                {
                    response.Dispose();
                }
            }
        }
        #endregion

        #region [HttpClient文件传输]
        /// <summary>
        /// HttpClient文件传输
        /// Author:蔡嘉福
        /// Describe:HttpClient文件传输
        /// </summary>
        /// <param name="data">文件字节数组</param>
        /// <param name="url">请求地址</param>
        /// <param name="fileName">文件名称</param>
        /// <returns></returns>
        public static string HttpClientFile(byte[] data, string url, string fileName)
        {
            if (data.Length == 0 || data == null)
            {
                Logs.Error("文件字节数组不能为空");
                return string.Empty;
            }
            if (string.IsNullOrEmpty(fileName))
            {
                Logs.Error("文件名不能为空");
                return string.Empty;
            }
            HttpResponseMessage? response = null;
            StreamContent? content = null;
            string result = string.Empty;
            try
            {
                var formData = new MultipartFormDataContent();
                formData.Add(new ByteArrayContent(data), "file", fileName);
                response = httpClient.PostAsync(url, formData).Result;
                response.EnsureSuccessStatusCode();
                result = response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                Logs.Error(ex.ToString());
}
            finally
            {
                if (response != null) { response.Dispose(); }
                if (content != null) { content.Dispose(); }
            }
            return result;
        }
        #endregion
    }
}
