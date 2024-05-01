using back_1._2.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Text.Json;
using back_1._2.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System;
using Microsoft.AspNetCore.Authorization;

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

    [Route("ItemRegistration2")]
    public class RegistrationController2 : Controller
    {
        [HttpPost]
        public string Index62()
        {
            UserContext context = new UserContext();
            Item item = new Item() { Name = "Голубика", Price = "549", Description = "Нежное мыло с ароматом голубики - воплощение летней свежести и яркости. Очищает кожу, оставляя приятное ощущение мягкости и аромата свежесорванной ягоды. Придайте своему уходу особый шарм с этим ароматическим мылом!", Image = "https://localhost:7287/blueberry.png" }; //ID устанавливается сам
            context.Items.Add(item);
            Item itemw = new Item() { Name = "Кофе", Price = "549", Description = "Ароматное мыло с натуральным экстрактом кофе - бодрящее утро для вашей кожи. Очищает и тонизирует, придавая энергию и освежающий аромат свежесваренного кофе. Начните день с приятного кофейного настроения благодаря этому уникальному мылу!", Image = "https://localhost:7287/coffee.png" }; //ID устанавливается сам
            context.Items.Add(itemw);
            Item iteme = new Item() { Name = "Клубника", Price = "549", Description = "Натуральное мыло с ароматом спелой клубники - роскошное увлажнение и свежесть для вашей кожи. Очищает и питает, придавая нежный аромат ягодного лета. Превратите ритуал умывания в настоящее удовольствие с этим ароматическим мылом!", Image = "https://localhost:7287/strawberry.png" }; //ID устанавливается сам
            context.Items.Add(iteme);
            context.SaveChanges();
            return $"Регистрация прошла успешно";

        }
    }
    //Web2item pine = new Web2item(1, "Хвоя", "549", "Натуральное мыло со вкусом хвои - вдохновленное свежестью лесных прогулок. Очищает кожу, оставляя нежный аромат свежести и спокойствия. Погрузитесь в атмосферу природы каждый раз, когда пользуетесь этим мылом.", "https://localhost:7287/pine.png");
    //Web2item blue = new Web2item(2, "Голубика", "549", "Нежное мыло с ароматом голубики - воплощение летней свежести и яркости. Очищает кожу, оставляя приятное ощущение мягкости и аромата свежесорванной ягоды. Придайте своему уходу особый шарм с этим ароматическим мылом!", "https://localhost:7287/blueberry.png");
    //Web2item cofe = new Web2item(3, "Кофе", "549", "Ароматное мыло с натуральным экстрактом кофе - бодрящее утро для вашей кожи. Очищает и тонизирует, придавая энергию и освежающий аромат свежесваренного кофе. Начните день с приятного кофейного настроения благодаря этому уникальному мылу!", "https://localhost:7287/coffee.png");
    //Web2item stra = new Web2item(4, "Клубника", "549", "Натуральное мыло с ароматом спелой клубники - роскошное увлажнение и свежесть для вашей кожи. Очищает и питает, придавая нежный аромат ягодного лета. Превратите ритуал умывания в настоящее удовольствие с этим ароматическим мылом!", "https://localhost:7287/strawberry.png");


    /*app.Map("/web2additem", async(HttpContext context) =>
{
    Web2item pine = new Web2item(1, "Хвоя", "549", "Натуральное мыло со вкусом хвои - вдохновленное свежестью лесных прогулок. Очищает кожу, оставляя нежный аромат свежести и спокойствия. Погрузитесь в атмосферу природы каждый раз, когда пользуетесь этим мылом.", "https://localhost:7287/pine.png");
    Web2item blue = new Web2item(2, "Голубика", "549", "Нежное мыло с ароматом голубики - воплощение летней свежести и яркости. Очищает кожу, оставляя приятное ощущение мягкости и аромата свежесорванной ягоды. Придайте своему уходу особый шарм с этим ароматическим мылом!", "https://localhost:7287/blueberry.png");
    Web2item cofe = new Web2item(3, "Кофе", "549", "Ароматное мыло с натуральным экстрактом кофе - бодрящее утро для вашей кожи. Очищает и тонизирует, придавая энергию и освежающий аромат свежесваренного кофе. Начните день с приятного кофейного настроения благодаря этому уникальному мылу!", "https://localhost:7287/coffee.png");
    Web2item stra = new Web2item(4, "Клубника", "549", "Натуральное мыло с ароматом спелой клубники - роскошное увлажнение и свежесть для вашей кожи. Очищает и питает, придавая нежный аромат ягодного лета. Превратите ритуал умывания в настоящее удовольствие с этим ароматическим мылом!", "https://localhost:7287/strawberry.png");
    List<Web2item> itemsList = new List<Web2item>();
    itemsList.Add(pine );
    itemsList.Add(blue );
    itemsList.Add(cofe );
    itemsList.Add(stra );
    var jsonResult = JsonSerializer.Serialize(itemsList);
    context.Response.ContentType = "application/json";
    await context.Response.WriteAsync(jsonResult);
    return;
});*/

    /*[Route("GetItemList")]
    public class ItemListController : Controller
    {
        [HttpGet]
        public IActionResult Index7(HttpContext context)
        {
            UserContext context1 = new UserContext();
            List<Item> itemList = new List<Item>();
            //for (int i = 0; i < context.Items.Count(); i++)
            itemList = context1.Items.ToList();
            var jsonResult = JsonSerializer.Serialize(itemList);
            context.Response.ContentType = "application/json";
            context.Response.WriteAsync(jsonResult);
            return Ok(jsonResult);
        }
    }*/

    [Route("UserRegistrationWeb")]
    public class RegistrationWebController : Controller
    {
        [HttpPost]
        public string Index7([FromBody] RegisterModel model)
        {
            if (model.Password == model.Password2)
            {
                UserContext context = new UserContext();
                User user = new User() { Name = model.Name, Email = model.Email, Password = model.Password };
                context.Users.Add(user);
                context.SaveChanges();
                return $"Регистрация прошла успешно";
            }
            else
            {
                return $"Что-то пошло не так";
            }
        }
    }

    [Route("UserLoginWeb")]
    public class LiginWebController : Controller
    {
        [HttpPost]
        public IActionResult Index8([FromBody] LoginModel model)
        {
            UserContext context = new UserContext();
            User user = new User() { Name = "", Email = model.Email, Password = model.Password };
            User? dbuder = context.Users.FirstOrDefault(u => ((u.Email == user.Email) & (u.Password == user.Password)));
            if (dbuder == null) return BadRequest("Пользователя с такой почтой не существует. Совершите регистрацию");
            if (dbuder.Password != user.Password) BadRequest("Пароль не совпадает");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, dbuder.Id.ToString()),
                new Claim(ClaimTypes.Name, dbuder.Email),
            };

            var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(20)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var jwtr = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(5)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwtr = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                refresh_token = encodedJwtr,
                username = dbuder.Email,
            };
            return Ok(response);
        }
    }

    [Route("CheckWeb")]
    public class CheckController : Controller
    {
        [HttpPost]
        [Authorize]
        public IActionResult  Index9()
        {
            Console.WriteLine("Кто-то зашел");
            return Ok();
        }
    }

    public class RegisterModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Password2 { get; set; }
    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
