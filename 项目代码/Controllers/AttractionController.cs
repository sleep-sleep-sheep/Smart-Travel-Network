using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
namespace WebApplication1.Controllers
{
    public class AttractionController : Controller
    {
        private readonly string _connectionString = "Server=localhost;Database=travel_city_information;Uid=root;Pwd=123456;";
        private readonly HttpClient _httpClient = new HttpClient();

        // 预定义的城市列表
        private static List<string> cityList = new List<string>
        {
            "北京", "上海", "广州", "深圳", "成都", "杭州", "武汉", "南京", "西安", "重庆"
            // 可以继续添加更多城市
        };

        [HttpPost]
        [Route("/Attraction")]
        public async Task<IActionResult> GetAttractions([FromForm] string inputText)
        {
            try
            {
                // 从输入文本中提取城市名称
                List<string> cities = ExtractCities(inputText);

                string queryCondition = BuildQueryCondition(cities);

                // 在数据库中查询景点信息
                 var attractions = await QueryAttractions(queryCondition);
                Console.WriteLine($"返回的 attractions 类型: {attractions.GetType()}");
                foreach (var attraction in attractions)
                {
                    Console.WriteLine($"Attraction Name: {attraction.Name}, PhotoPath: {attraction.PhotoPath}, TravelNote: {attraction.TravelNote}");
                }
                return Ok(attractions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"内部服务器错误: {ex.Message}");
            }
        }

        private List<string> ExtractCities(string text)
        {
            return cityList.Where(city => text.Contains(city)).ToList();
        }

        private string BuildQueryCondition(List<string> cities)
        {
            if (cities.Count == 0)
            {
                return "1 = 0"; // 无匹配城市，返回无结果的条件
            }

            string conditions = string.Join(" OR ", cities.Select(city => $"name LIKE '%{city}%'"));
            return conditions;
        }

        private async Task<List<Attraction>> QueryAttractions(string queryCondition)
        {
            List<Attraction> attractions = new List<Attraction>();
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = $"SELECT name, photo_path, travel_note FROM travel_note WHERE {queryCondition}";
                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            attractions.Add(new Attraction
                            {
                                Name = reader.GetString("name"),
                                PhotoPath = reader.GetString("photo_path"),
                                TravelNote = reader.GetString("travel_note")
                            });
                        }
                    }
                }
            }
            return attractions;
        }
    }



public class Attraction
    {
        [JsonPropertyName("Name")]
        public string Name { get; set; }
        [JsonPropertyName("PhotoPath")]
        public string PhotoPath { get; set; }
        [JsonPropertyName("TravelNote")]
        public string TravelNote { get; set; }
    }
}

