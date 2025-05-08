using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Data;
using System.Net.Http;
namespace WebApplication1.Controllers
{
    public class PrivacyController : Controller
    {
        private readonly string connectionString = "Server=localhost;Database=travel_city_information;Uid=root;Pwd=123456;";
        private readonly HttpClient _httpClient = new HttpClient();

        [HttpPost]
        public IActionResult SaveQARecord([FromBody] QARecord record)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO QARecords (Question, Answer) VALUES (@Question, @Answer)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Question", record.question);
                command.Parameters.AddWithValue("@Answer", record.answer);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });
                }
            }
        }

        // 获取问答记录
        [HttpGet]
        public IActionResult GetQARecords()
        {
            List<QARecord> records = new List<QARecord>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM QARecords";
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    QARecord record = new QARecord
                    {
                        id = (int)reader["Id"],
                        question = reader["Question"].ToString(),
                        answer = reader["Answer"].ToString()
                    };
                    records.Add(record);
                }

                reader.Close();
            }

            return Json(records);
        }
    }

    public class SearchRecord
    {
        public string question { get; set; }
    }

    public class QARecord
    {
        public int id { get; set; }
        public string question { get; set; }
        public string answer { get; set; }
    }

}



