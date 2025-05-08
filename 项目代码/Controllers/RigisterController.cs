using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    static class Already
    {
      public static  bool AlreadyLogin = false;
    }
    public class RigisterController : Controller
    {
        
        // 模拟用户数据库
        private static List<User> users = new List<User>();
        // 模拟订单数据库
        private static List<Booking> bookings = new List<Booking>();
        // 模拟旅游线路数据库
        private static List<TravelRoute> travelRoutes = new List<TravelRoute>
        {
            new TravelRoute { Id = 1, Name = "浪漫海滨之旅", Description = "享受阳光沙滩，体验海滨风情。此线路包含多个海滨景点，您可以尽情享受海浪、沙滩和美食。" },
            new TravelRoute { Id = 2, Name = "深山探险之旅", Description = "探索深山秘境，感受大自然的魅力。此线路会带领您穿越茂密森林，攀登高峰，体验刺激的探险活动。" },
            new TravelRoute { Id = 3, Name = "历史文化之旅", Description = "追寻历史足迹，领略文化底蕴。此线路会带您参观古老的建筑、博物馆，了解当地的历史文化。" }
        };

        public IActionResult Index()
        {
            return View();
        }

        // 处理登录请求
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                // 登录成功，设置会话或其他验证机制
                Already.AlreadyLogin = true;
                return RedirectToAction("TravelList","Rigister");
            }
            else
            {
                ViewBag.LoginError = "用户名或密码错误";
                return View("~/Views/Home/Privacy2.cshtml");
            }
        }

        // 处理注册请求
        [HttpPost]
        public IActionResult Register(string username, string password)
        {
            if (users.Any(u => u.Username == username))
            {
                ViewBag.RegisterError = "用户名已存在";
                return View("~/Views/Home/Privacy2.cshtml");
            }
            else
            {
                users.Add(new User { Username = username, Password = password });
                // 注册成功，设置会话或其他验证机制
               Already.AlreadyLogin = true;
                return RedirectToAction("TravelList","Rigister");
            }
        }

        // 显示行程列表
        public IActionResult TravelList()
        {
            ViewBag.TravelRoutes = travelRoutes;
            return View("~/Views/Home/user.cshtml");
        }

        // 显示线路详情
        public IActionResult RouteDetails(int id)
        {
            var route = travelRoutes.FirstOrDefault(r => r.Id == id);
            if (route != null)
            {
                return Ok(new { Title = route.Name, Description = route.Description });
            }
            return NotFound();
        }

        // 处理订票请求
        [HttpPost]
        public IActionResult Book(int routeId, string name, string phone, string date, int number)
        {
            var route = travelRoutes.FirstOrDefault(r => r.Id == routeId);
            if (route != null)
            {
                bookings.Add(new Booking
                {
                    RouteName = route.Name,
                    Name = name,
                    Phone = phone,
                    Date = date,
                    Number = number
                });
                return Ok("预订成功");
            }
            return NotFound();
        }
    }

    // 用户类
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    // 订单类
    public class Booking
    {
        public string RouteName { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Date { get; set; }
        public int Number { get; set; }
    }

    // 旅游线路类
    public class TravelRoute
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
