using back_1._2.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Text.Json;
using back_1._2.Models;

namespace back_1._2.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        List<string> UserNames = new List<string> { };


        [HttpPost(Name = "SetMainPage")]  //передает данные в теле запроса, а не в адресной строке, что обеспечивает безопасность и передачу большего объема данных
        public string Set(string a)
        {
            if (System.IO.File.Exists("UserNames.json"))
            {
                if (System.IO.File.ReadAllText("UserNames.json").ToString() != ("[]\r\n"))
                {
                    using (StreamReader sr = new("UserNames.json"))
                    {
                        UserNames = JsonSerializer.Deserialize<List<string>>(sr.ReadLine().ToString());
                    }
                }
                UserNames.Add(a);

                using (StreamWriter sr = new("UserNames.json"))
                {
                    sr.WriteLine(JsonSerializer.Serialize(UserNames));
                }
                return $"Вы ввели '{a}'";
            }
            else { return "ошибка"; }
        }


        [HttpGet(Name = "GetMainPage")]
        public string Get() //возвращает данные, может передать данные в адресной строке
        {
            return "Добро пожаловать! Это Main.";
        }


        [HttpPut(Name = "PutMainPage")]
        public string Put(string[] a) //заменяет данные
        {
            if (System.IO.File.Exists("UserNames.json"))
            {
                UserNames = a.ToList<string>();
                using (StreamWriter sr = new("UserNames.json"))
                {
                    sr.WriteLine(JsonSerializer.Serialize(UserNames));
                }
                return "Информация была заменена";
            }
            else { return "ошибка"; }
        }


        [HttpDelete(Name = "DeleteMainPage")]
        public string Delete() //удаляет данные
        {
            if (System.IO.File.Exists("UserNames.json"))
            {
                //System.IO.File.Delete("UserNames.json");
                using (StreamWriter sr = new("UserNames.json"))
                {
                    sr.WriteLine(JsonSerializer.Serialize(new List<string> { }));
                }
                return "Информация была удалена";
            }
            else { return "ошибка"; }
        }
    }

    [Route("TextReturn")]
    public class ResponseController1 : Controller
    {
        [HttpGet]
        public IResult Index1()
        {
            return Results.Text("Возвращение текста");
        }
    }

    [Route("RedirectReturn")]
    public class ResponseController2 : Controller
    {
        [HttpGet]
        public IResult Index2()
        {
            return Results.Redirect("/TextReturn");
        }
    }

    [Route("JsonReturn")]
    public class ResponseController3 : Controller
    {
        [HttpGet]
        public IResult Index4()
        {
            return Results.Json(new { name = "Neco Arc", age = "∞", weight = 8 });
        }
    }

    [Route("FileReturn")]
    public class ResponseController4 : Controller
    {
        [HttpGet]
        public IResult Index4()
        {
            string path = "UserNames.json";
            FileStream fileStream = new FileStream(path, FileMode.Open);
            string contentType = "file/json";
            string downloadName = "UserNames.json";
            return Results.File(fileStream, contentType, downloadName);
        }
    }

    [Route("HTMLReturn")]
    public class ResponseController5 : Controller
    {
        [HttpGet]
        public ContentResult Index5()
        {
            var html = System.IO.File.ReadAllText(@"C:\Users\sacho\source\repos\back 1.2\back 1.2\htmlpage.cshtml");
            return base.Content(html, "text/html");
        }
    }

    [Route("Auth")]
    public class AuthController : Controller
    {
        [HttpGet]
        public ContentResult Auth()
        {
            var html = System.IO.File.ReadAllText(@"C:\Users\sacho\source\repos\back 1.2\back 1.2\Authorization.html");
            return base.Content(html, "text/html");
        }
    }

    [Route("UserRegistration")]
    public class RegistrationController : Controller
    {
        [HttpPost]
        public string Index6(string name, string email, string password)
        {
            UserContext context = new UserContext();
            User user = new User() {Name = name, Email = email, Password = password }; //ID устанавливается сам
            context.Users.Add(user);
            context.SaveChanges();
            return $"Регистрация прошла успешно\nID: {user.Id}\nИмя: {user.Name}\nПочта: {user.Email}\nПароль: {user.Password}";
            
        }
    }
}
