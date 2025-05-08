using System.Diagnostics;
using System.Text;
using WebApplication1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using MySqlConnector;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Privacy1()
        {
            return View();
        }

        public IActionResult Privacy2()
        {
            if(Already.AlreadyLogin==false)
            return View();
            return View("~/Views/Home/user.cshtml");
        }
        public IActionResult Index1()
        {
            return View();
        }
        public IActionResult Index2()
        {
            return View();
        }
        public IActionResult Index3()
        {
            return View();
        }
        public IActionResult contact()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }



    public class DeepSeekController : Controller
    {
        private readonly HttpClient _httpClient;

        public DeepSeekController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpPost]
        public async Task<IActionResult> CallDeepSeek([FromForm] string inputText)
        {
            string apiKey = "sk-413587e8be36415089d2e198ac7315b5";
            // 替换为DeepSeek官方提供的具体端点（如聊天API）
            string apiUrl = "https://api.deepseek.com/chat/completions";

            // 修改请求体结构，包含 messages 字段
            var requestBody = new
            {
                model = "deepseek-chat",
                messages = new[]
                {
                new
                {
                    role = "user",
                    content = inputText
                }
            }
            };

            var jsonBody = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var response = await _httpClient.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                // 解析 JSON 响应
                var responseObject = JsonConvert.DeserializeObject<dynamic>(responseContent);
                // 提取需要的内容
                string usefulContent = responseObject.choices[0].message.content;
                return Ok(usefulContent);
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, $"错误状态码：{response.StatusCode}\n错误信息：{errorContent}");
            }
        }
    }




   
}