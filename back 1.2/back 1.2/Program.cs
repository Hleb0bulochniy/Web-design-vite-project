using Azure;
using back_1._2;
using back_1._2.Data;
using back_1._2.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;


//string[] commandLineArgs = { "course=BackEnd", "number=123" };
//var builder = WebApplication.CreateBuilder(commandLineArgs);

var builder = WebApplication.CreateBuilder(args);

//var builder = WebApplication.CreateBuilder(new WebApplicationOptions { WebRootPath = "Content" });

var adminRole = new Role("admin");
var userRole = new Role("user");

var people = new List<Person>
{
    new Person("tom@gmail.com", "12345", adminRole),
    new Person("bob@gmail.com", "55555", userRole),
};

builder.Services.AddAuthorization();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = AuthOptions.ISSUER,
            ValidateAudience = true,
            ValidAudience = AuthOptions.AUDIENCE,
            ValidateLifetime = true,
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true,
        };

    });


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

//маршруты из прошлых лабораторных
//app.Map("/hello", SendHello);
//app.Map("/", () => Results.Content("test  тест", "text/plain", System.Text.Encoding.UTF8));
//app.Map("/hello", () => Results.Text("Hello World"));
app.Map("/json1", () => Results.Json(new { name = "Tom", age = 37 }));
app.Map("/old", () => Results.Redirect("/new"));
app.Map("/new", () => "New Address");
app.Map("/hello2", async context => await context.Response.WriteAsync("Hello!"));
app.Map("/json2", async () =>
{
    string path = "UserNames.json";
    byte[] fileContent = await File.ReadAllBytesAsync(path);
    string contentType = "file/json";
    string downloadName = "UserNames.json";
    return Results.File(fileContent, contentType, downloadName);
});
app.MapGet("/login1", async (HttpContext context) =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
    // html-форма для ввода логина/пароля
    string loginForm = @"<!DOCTYPE html>
    <html>
    <head>
        <meta charset='utf-8' />
        <title>METANIT.COM</title>
    </head>
    <body>
        <h2>Login Form</h2>
        <form method='post'>
            <p>
                <label>Email</label><br />
                <input name='email' />
            </p>
            <p>
                <label>Password</label><br />
                <input type='password' name='password' />
            </p>
            <input type='submit' value='Login' />
        </form>
    </body>
    </html>";
    await context.Response.WriteAsync(loginForm);
});
app.MapPost("/login1", async (string? returnUrl, HttpContext context) =>
{
    var form = context.Request.Form;
    if (!form.ContainsKey("email") || !form.ContainsKey("password"))
        return Results.BadRequest("Email и/или пароль не установлены");
    string email = form["email"];
    string password = form["password"];

    Person? person = people.FirstOrDefault(p => p.Email == email && p.Password == password);
    if (person is null) return Results.Unauthorized();
    var claims = new List<Claim>
    {
        new Claim(ClaimsIdentity.DefaultNameClaimType, person.Email),
        new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Role.Name)
    };

    var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
    var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

    var response = new
    {
        access_token = encodedJwt,
        username = person.Email
    };
    return Results.Json(response);
});
app.Map("/data", [Authorize] () => new { message = "Every user can access this" }) ;
app.Map("/admin", [Authorize(Roles = "admin")] () => "Admin Panel");
app.Map("/", [Authorize(Roles = "admin, user")] (HttpContext context) =>
{
    var login = context.User.FindFirst(ClaimsIdentity.DefaultNameClaimType);
    var role = context.User.FindFirst(ClaimsIdentity.DefaultRoleClaimType);
    return $"Name: {login?.Value}\nRole: {role?.Value}";
});



//новые маршруты
app.Map("/items/{id}", (string id) => $"Item Id: {id}"); //можно передать параметры
app.Map("/items2/{id}:on:{description}", (string id, string description) => $"Item description: {description}"); //можно передать несколько параметров (их можно связать чем угодно)
app.Map("/items3/{id}.{comments?}", (string id, string? comments) => $"Item comments: {comments ?? "No comments"}"); //параметры могут быть необязательные
app.Map("/items4/{id=null}", (string? id) => $"Id: {id}"); //можно задавать параметры по умолчанию
app.Map("/items5/{**info}", (string info) => $"User Info: {info}"); //можно передавать произвольное количество параметров через "/"

app.Map("/items6/{id:int}", (int id) => $"Item Id: {id}"); //можно ограничить типом,
app.Map("/items7/{id:int:range(1,100)}", (int id) => $"Item Id: {id}"); //диапазоном,
app.Map("/items8/{id:int:range(1,100)}/{description:minlength(5)}", (int id, string description) => $"Item description: {description}"); //длинной
app.Map("/items9/{id:int:range(1,100)}/{description:required}", (int id, string description) => $"Item description: {description}"); //и т.д.


app.Map("/{items}/message", (string items) => $"1"); //из-за разной приоритетности шаблонов будут вызываться разные маршруты
app.Map("/items10/{message?}", (string? message) => $"2");
app.Map("/items10", () => "Index Page");

//app.MapGet("/user", ([AsParameters] Item item) => $"id: {item.id} description: {item.description}"); //можно использовать атрибуты


//app.Map("/items", () => "Store page");


//CORS below
//app.UseCors(builder => builder.WithOrigins("https://localhost:7281").WithHeaders("custom-header").WithMethods("DELETE").AllowCredentials());
app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
//app.UseCors(builder => builder.WithOrigins("http://localhost:5173").AllowAnyMethod().AllowAnyHeader());

/*app.Run(async (context) =>
{
    var a = context.Request.Cookies["a"];  // получаем отправленные куки
    await context.Response.WriteAsync($"Hello {a}!");
    Console.WriteLine(a);
});*/
app.UseDirectoryBrowser(new DirectoryBrowserOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Content")),

    RequestPath = new PathString("/pages")
});
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(
            Path.Combine(Directory.GetCurrentDirectory(), @"Content")),
    RequestPath = new PathString("/pages")
});

//builder.Configuration.AddJsonFile("conf.json");
//builder.Configuration.AddXmlFile("conf.xml");


//app.Map("/config", (IConfiguration appConfig) => $"Курс: {appConfig["course"]}. Лабораторная работа номер {appConfig["number:labnum"]} ({appConfig["number:realnum"]} по счету)");

/*builder.Configuration.AddInMemoryCollection(new Dictionary<string, string>
{
    {"course", "BackEnd"},
    {"number", "192"}
});*/

app.Map("/config", (IConfiguration appConfig) => $"Курс: {appConfig["course"]}. Лабораторная работа номер {appConfig["number"]}");


//app.Map("/config", (IConfiguration appConfig) => $"TMP: {appConfig["TMP"] ?? "not set"}");

/*ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddDebug());
ILogger logger = loggerFactory.CreateLogger<Program>();
app.Run(async (context) =>
{
    app.Logger.LogInformation($"Path: {context.Request.Path}  Time:{DateTime.Now.ToLongTimeString()}");
});*/

app.Map("/1", (HttpContext context) =>
{
    app.Logger.LogInformation($"Path: {context.Request.Path}  Time:{DateTime.Now.ToLongTimeString()}");
    return $"1";
});
app.Map("/2", (HttpContext context) =>
{
    app.Logger.LogCritical($"Path: {context.Request.Path}  Time:{DateTime.Now.ToLongTimeString()}");
    return $"2";
});
app.Map("/3", (HttpContext context) =>
{
    app.Logger.LogError($"Path: {context.Request.Path}  Time:{DateTime.Now.ToLongTimeString()}");
    return $"3";
});
app.Map("/4", (HttpContext context) =>
{
    app.Logger.LogWarning($"Path: {context.Request.Path}  Time:{DateTime.Now.ToLongTimeString()}");
    return $"4";
});

/*app.Map("/web2additem", async (HttpContext context) =>
{
    Web2item pine = new Web2item(1, "Хвоя", "549", "Натуральное мыло со вкусом хвои - вдохновленное свежестью лесных прогулок. Очищает кожу, оставляя нежный аромат свежести и спокойствия. Погрузитесь в атмосферу природы каждый раз, когда пользуетесь этим мылом.", "https://localhost:7287/pine.png");
    Web2item blue = new Web2item(2, "Голубика", "549", "Нежное мыло с ароматом голубики - воплощение летней свежести и яркости. Очищает кожу, оставляя приятное ощущение мягкости и аромата свежесорванной ягоды. Придайте своему уходу особый шарм с этим ароматическим мылом!", "https://localhost:7287/blueberry.png");
    Web2item cofe = new Web2item(3, "Кофе", "549", "Ароматное мыло с натуральным экстрактом кофе - бодрящее утро для вашей кожи. Очищает и тонизирует, придавая энергию и освежающий аромат свежесваренного кофе. Начните день с приятного кофейного настроения благодаря этому уникальному мылу!", "https://localhost:7287/coffee.png");
    Web2item stra = new Web2item(4, "Клубника", "549", "Натуральное мыло с ароматом спелой клубники - роскошное увлажнение и свежесть для вашей кожи. Очищает и питает, придавая нежный аромат ягодного лета. Превратите ритуал умывания в настоящее удовольствие с этим ароматическим мылом!", "https://localhost:7287/strawberry.png");
    List<Web2item> itemsList = new List<Web2item>();
    itemsList.Add( pine );
    itemsList.Add( blue );
    itemsList.Add( cofe );
    itemsList.Add( stra );
    var jsonResult = JsonSerializer.Serialize(itemsList);
    context.Response.ContentType = "application/json";
    await context.Response.WriteAsync(jsonResult);
    return;
});*/

app.Map("/web2additem", async (HttpContext context) =>
{
    UserContext context1 = new UserContext();
    List<Item> itemList = new List<Item>();
    //for (int i = 0; i < context.Items.Count(); i++)
    itemList = context1.Items.ToList();
    var jsonResult = JsonSerializer.Serialize(itemList);
    context.Response.ContentType = "application/json";
    context.Response.WriteAsync(jsonResult);
    return;
});

app.Map("/web2additemForCart", [Authorize] async (HttpContext context) =>
{
    string authorizationHeader = context.Request.Headers["Authorization"];
    string token = authorizationHeader.Replace("Bearer ", "");
    var handler = new JwtSecurityTokenHandler();
    var jwtToken = handler.ReadJwtToken(token);
    string userId = jwtToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
    UserContext context1 = new UserContext();


    List<ItemsInUser> itemInUserList = new List<ItemsInUser>();
    foreach (var item in context1.ItemsInUser)
    {
        if (context1.ItemsInUser.FirstOrDefault(u => u.UserId == int.Parse(userId)) != null) return;
    }

    var jsonResult = JsonSerializer.Serialize(itemInUserList);
    context.Response.ContentType = "application/json";
    context.Response.WriteAsync(jsonResult);
    return;
});

app.MapGet("/checkAuth", [Authorize] async (HttpContext context) =>
{
    return;
});

app.MapGet("/getInfoById/{idToGet}", [Authorize] async (int idToGet, HttpContext context) =>
{
    string authorizationHeader = context.Request.Headers["Authorization"];
    string token = authorizationHeader.Replace("Bearer ", "");
    var handler = new JwtSecurityTokenHandler();
    var jwtToken = handler.ReadJwtToken(token);
    string userId = jwtToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
    UserContext context1 = new UserContext();

    if (context1.ItemsInUser.FirstOrDefault(u => (u.UserId == int.Parse(userId) & u.itemId == idToGet)) == null)
    {
        ItemsInUser itemsInUser = new ItemsInUser();
        itemsInUser.UserId = int.Parse(userId);
        itemsInUser.isInCart = false;
        itemsInUser.isFavourite = false;
        itemsInUser.itemInCartNumber = 0;
        itemsInUser.itemId = idToGet;
        return itemsInUser;
    }
    else
    {
        ItemsInUser item = context1.ItemsInUser.FirstOrDefault(u => (u.UserId == int.Parse(userId) & u.itemId == idToGet));
        return item;
    }
});

app.MapGet("/getNumInCartById/{idToGet}", [Authorize] async (int idToGet, HttpContext context) =>
{
    string authorizationHeader = context.Request.Headers["Authorization"];
    string token = authorizationHeader.Replace("Bearer ", "");
    var handler = new JwtSecurityTokenHandler();
    var jwtToken = handler.ReadJwtToken(token);
    string userId = jwtToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
    UserContext context1 = new UserContext();
    ItemsInUser item = context1.ItemsInUser.FirstOrDefault(u => (u.UserId == int.Parse(userId) & u.itemId == idToGet));

    if (context1.ItemsInUser.FirstOrDefault(u => (u.UserId == int.Parse(userId) & u.itemId == idToGet)) == null)
    {
        Console.WriteLine("1");
        ItemsInUser itemsInUser = new ItemsInUser();
        itemsInUser.UserId = int.Parse(userId);
        itemsInUser.isInCart = true;
        itemsInUser.isFavourite = false;
        itemsInUser.itemInCartNumber = 1;
        itemsInUser.itemId = idToGet;
        context1.Add(itemsInUser);
        context1.SaveChanges();
    }
    else
    {
        Console.WriteLine("2");
        context1.ItemsInUser.FirstOrDefault(u => (u.UserId == int.Parse(userId) & u.itemId == idToGet)).itemInCartNumber++;
        context1.ItemsInUser.FirstOrDefault(u => (u.UserId == int.Parse(userId) & u.itemId == idToGet)).isInCart = true;
        context1.SaveChanges();
    }
    return;
});

app.UseCors("AllowAnyOrigin");

app.MapGet("/addNumInCartById/{idToAdd}", [Authorize] async (int idToAdd, HttpContext context) =>
{
    string authorizationHeader = context.Request.Headers["Authorization"];
    string token = authorizationHeader.Replace("Bearer ", "");
    var handler = new JwtSecurityTokenHandler();
    var jwtToken = handler.ReadJwtToken(token);
    string userId = jwtToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

    UserContext context1 = new UserContext();
    if (context1.ItemsInUser.FirstOrDefault(u => (u.UserId == int.Parse(userId) & u.itemId == idToAdd)) == null)
    {
        ItemsInUser itemsInUser = new ItemsInUser();
        itemsInUser.UserId = int.Parse(userId);
        itemsInUser.isInCart = true;
        itemsInUser.isFavourite = false;
        itemsInUser.itemInCartNumber = 1;
        itemsInUser.itemId = idToAdd;
        context1.Add(itemsInUser);
        context1.SaveChanges();
        return "Добавлен новый";
    }
    else
    {
        ItemsInUser itemsInUser = context1.ItemsInUser.FirstOrDefault(u => (u.UserId == int.Parse(userId) & u.itemId == idToAdd));
        context1.ItemsInUser.FirstOrDefault(u => (u.UserId == int.Parse(userId) & u.itemId == idToAdd)).itemInCartNumber++;
        context1.SaveChanges();
        return "Добавлен +1";
    }
});

app.UseCors("AllowAnyOrigin");

app.MapGet("/minusNumInCartById/{idToRemove}", [Authorize] async (int idToRemove, HttpContext context) =>
{
    string authorizationHeader = context.Request.Headers["Authorization"];
    string token = authorizationHeader.Replace("Bearer ", "");
    var handler = new JwtSecurityTokenHandler();
    var jwtToken = handler.ReadJwtToken(token);
    string userId = jwtToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

    UserContext context1 = new UserContext();
    if (context1.ItemsInUser.FirstOrDefault(u => (u.UserId == int.Parse(userId) & u.itemId == idToRemove)) != null)
    {
        ItemsInUser itemsInUser = context1.ItemsInUser.FirstOrDefault(u => (u.UserId == int.Parse(userId) & u.itemId == idToRemove));
        context1.ItemsInUser.FirstOrDefault(u => (u.UserId == int.Parse(userId) & u.itemId == idToRemove)).itemInCartNumber--;
        if (context1.ItemsInUser.FirstOrDefault(u => (u.UserId == int.Parse(userId) & u.itemId == idToRemove)).itemInCartNumber == 0)
        {
            context1.ItemsInUser.FirstOrDefault(u => (u.UserId == int.Parse(userId) & u.itemId == idToRemove)).isInCart = false;
        }
        context1.SaveChanges();
        return "Удален -1";
    }
    return"";
});



app.Map("/hrt", [Authorize] async (HttpContext context) =>
{
    Console.WriteLine("Кто-то зашел");
    return;
});

app.Map("/web3log", async (HttpContext context) =>
{

    return;
});


app.Run();


public class AuthOptions
{
    public const string ISSUER = "MyAuthServer";
    public const string AUDIENCE = "MyAuthClient";
    const string KEY = "hello_my-nameisdaniil12345sashenkov!5ballov";
    public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
}

class Person
{
    public string Email { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }
    public Person(string email, string password, Role role)
    {
        Email = email;
        Password = password;
        Role = role;
    }
}
class Role
{
    public string Name { get; set; }
    public Role(string name) => Name = name;
}

//record Item(int id, string description);