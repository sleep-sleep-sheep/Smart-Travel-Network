using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Diagnostics;
using System.Text;

namespace WebApplication1.Controllers
{
    public class TouristAttractionsController : Controller
    {
        private readonly string connectionString = "Server=localhost;Database=travel_city_information;Uid=root;Pwd=123456;";

        public IActionResult Index()
        {
            var attractions = GetTouristAttractions(0, 3);
            Debug.WriteLine($"查询到的景点数量: {attractions.Count}");
            var htmlContent = BuildAttractionsHtml(attractions);
            return Content(htmlContent, "text/html");
        }

        public IActionResult GetAttractions(int skip, int take)
        {
            var attractions = GetTouristAttractions(skip, take);
            Debug.WriteLine($"查询到的景点数量: {attractions.Count}");
            var htmlContent = BuildAttractionsHtml(attractions);
            return Content(htmlContent, "text/html");
        }

        private List<TouristAttraction> GetTouristAttractions(int skip, int take)
        {
            var attractions = new List<TouristAttraction>();
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = $"SELECT * FROM TouristAttractions LIMIT {skip}, {take}";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                attractions.Add(new TouristAttraction
                                {
                                    Id = reader.GetInt32("Id"),
                                    Name = reader.GetString("Name"),
                                    Description = reader.GetString("Description"),
                                    ImageUrl = reader.GetString("ImageUrl")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"数据库查询出错: {ex.Message}");
            }
            return attractions;
        }

        private string BuildAttractionsHtml(List<TouristAttraction> attractions)
        {
            var sb = new StringBuilder();
            foreach (var attraction in attractions)
            {
                sb.Append(@$"
                    <div class='col-md-4 fadeInUp'>
                        <div class='card h-100'>
                            <img src='{attraction.ImageUrl}' class='card-img-top' alt='景点图片'>
                            <div class='card-body'>
                                <h3 class='card-title'>{attraction.Name}</h3>
                                <p class='card-text'>{attraction.Description}</p>
                            </div>
                        </div>
                    </div>
                ");
            }
            return sb.ToString();
        }
    }

    public class TouristAttraction
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}