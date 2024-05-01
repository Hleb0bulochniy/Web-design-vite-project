using Azure;
using Azure.Core;
using back_1._2;
using back_1._2.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Prometheus;
using System;
using System.Diagnostics.Contracts;
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
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();


builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer("Data Source=MSI\\SQLEXPRESS;Initial Catalog=Backend12;Persist Security Info=True;User ID=daniil;Password=test;Encrypt=True;Trust Server Certificate=True"));
builder.Services.AddTransient<UserService>();
builder.Services.AddMemoryCache();
/*builder.Services.AddStackExchangeRedisCache(options => {
    options.Configuration = "localhost";
    options.InstanceName = "local";
});*/
//builder.Services.AddResponseCompression(options => options.EnableForHttps = true);
builder.Services.AddOutputCache();


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.Environment.EnvironmentName = "Production";

//app.UseResponseCompression();
app.UseOutputCache();

//app.UseHttpMetrics();
//app.MapMetrics();
app.UseMetricServer("/metrics");

var rewriteOptions = new RewriteOptions();
rewriteOptions.Add(RewriteAccessRules);
rewriteOptions.Add(RewriteAccessRules);

app.UseRewriter(rewriteOptions);




void RewriteAccessRules(RewriteContext context)
{
    var user = context.HttpContext.User;


    if (context.HttpContext.Request.Path.StartsWithSegments("/admin1") && !user.IsInRole("admin"))
    {
        context.HttpContext.Response.Redirect("/AccessDenied");
    }
    else if (context.HttpContext.Request.Path.StartsWithSegments("/user1") && !(user.IsInRole("user") | user.IsInRole("admin")))
    {
        context.HttpContext.Response.Redirect("/login1");
    }
    else if (context.HttpContext.Request.Path.StartsWithSegments("/myrights"))
    {
        if (user.IsInRole("admin"))
        {
            context.HttpContext.Response.Redirect("/myrightsforadmin");
            context.Result = RuleResult.EndResponse;
        }
        else if (user.IsInRole("user"))
        {
            context.HttpContext.Response.Redirect("/myrightsforuser");
            context.Result = RuleResult.EndResponse;
        }
        else
        {
            context.HttpContext.Response.Redirect("/login1");
            context.Result = RuleResult.EndResponse;
        }
    }
}




/*app.UseStatusCodePages(async statusCodeContext =>
{
    var response = statusCodeContext.HttpContext.Response;
    var path = statusCodeContext.HttpContext.Request.Path;

    response.ContentType = "text/plain; charset=UTF-8";
    if (response.StatusCode == 404)
    {
        await response.WriteAsync($"Resource {path} Not Found");
    }
    else if (response.StatusCode == 500)
    {
        await response.WriteAsync($"Something wrong, please contact us.");
    }
});*/

//app.UseStatusCodePagesWithRedirects("/error/{0}");
app.UseStatusCodePagesWithReExecute("/error/{0}");

app.UseSession();

app.UseHttpsRedirection();

app.MapControllers();



//маршруты из прошлых лабораторных
app.Map("/hello", SendHello);
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

app.Map("/data", [Authorize] () => new { message = "Every user can access this" });
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

app.MapGet("/user", ([AsParameters] Item item) => $"id: {item.id} description: {item.description}"); //можно использовать атрибуты


//app.Map("/items", () => "Store page");


//CORS below
//app.UseCors(builder => builder.WithOrigins("https://localhost:7281").WithHeaders("custom-header").WithMethods("DELETE").AllowCredentials());
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
    RequestPath = new PathString("/pages"),

});
app.UseStaticFiles(new StaticFileOptions()
{
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Add("Cache-Control", "public,max-age=600");
    }
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

app.Map("/lab8Set", (HttpContext context) =>
{
    context.Session.SetString("LabNum", "8");
    return ("Данные с сервера загружены");
});
app.Map("/lab8Get", (HttpContext context) =>
{
    return ($"Лабораторная работа номер {context.Session.GetString("LabNum")}");
});
app.Map("/lab8", (HttpContext context) =>
{
    if (context.Session.Keys.Contains("LabNum"))
        return ($"Лабораторная работа номер {context.Session.GetString("LabNum")}");
    else
    {
        context.Session.SetString("LabNum", "8");
        return ("Данные с сервера загружены");
    }
});
app.Map("/lab8Student", (HttpContext context) =>
{
    if (context.Session.Keys.Contains("student"))
    {
        Student? student = context.Session.Get<Student>("student");
        return ($"Выполняет {student?.Name} из группы {student?.Group}");
    }
    else
    {
        Student student = new Student("Даниил Сашенков", "221-379");
        context.Session.Set<Student>("student", student);
        return ("Данные с сервера загружены");
    }
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(app => app.Run(async context =>
    {
        //await context.Response.WriteAsync("Error");
    }));
}

app.Map("/divide/{a}/{b}", (int a, int b) => $"{a/b}");

app.Map("/error/{Code}", (int Code) => $"Error: {Code}");


app.Map("/admin1", (HttpContext context) =>
{
    return $"You are currently in an admin page";
});

app.Map("/user1", (HttpContext context) =>
{
    return $"You are currently in user page";
});

app.Map("/AccessDenied", (HttpContext context) =>
{
    return $"Access to the page was denied";
});

app.Map("/myrightsforadmin",(HttpContext context) =>
{
    return $"You can ban any user and you can see their MasterCard information and their IP addressess and all of their passwords and...";
});

app.Map("/myrightsforuser", () => "You can open /user1");



app.MapGet("/user/{id}", async (int id, UserService userService) =>
{
    await Task.Delay(5000);
    User? user = await userService.GetUser(id);
    if (user != null) return $"User {user.Name}  Id={user.Id}  Email={user.Email}";
    else return "User not found";
}).CacheOutput();

app.MapGet("/lorem", () => "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.");



app.Run();
IResult SendHello()
{
    return Results.Text("Hello ASP.NET Core");
}

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
class Student
{
    public string Name { get; set; }
    public string Group { get; set; }
    public Student(string Name, string Group)
    {
        this.Name = Name;
        this.Group = Group;
    }
}
class Role
{
    public string Name { get; set; }
    public Role(string name) => Name = name;
}

record Item(int id, string description);

public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) =>
        Database.EnsureCreated();
}

//внутренний кэш
public class UserService
{
    ApplicationContext db;
    IMemoryCache cache;
    public UserService(ApplicationContext context, IMemoryCache memoryCache)
    {
        db = context;
        cache = memoryCache;
    }
    public async Task<User?> GetUser(int id)
    {
        cache.TryGetValue(id, out User? user);
        if (user == null)
        {
            user = await db.Users.FindAsync(id);
            if (user != null)
            {
                Console.WriteLine($"{user.Name} извлечен из базы данных");
                cache.Set(user.Id, user, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
            }
        }
        else
        {
            Console.WriteLine($"{user.Name} извлечен из кэша");
        }
        return user;
    }
}

//распределенный кэш
/*public class UserService
{
    ApplicationContext db;
    IDistributedCache cache;
    public UserService(ApplicationContext context, IDistributedCache distributedCache)
    {
        db = context;
        cache = distributedCache;
    }
    public async Task<User?> GetUser(int id)
    {
        User? user = null;
        var userString = await cache.GetStringAsync(id.ToString());
        if (userString != null) user = JsonSerializer.Deserialize<User>(userString);

        if (user == null)
        {
            user = await db.Users.FindAsync(id);
            if (user != null)
            {
                Console.WriteLine($"{user.Name} извлечен из базы данных");
                userString = JsonSerializer.Serialize(user);
                await cache.SetStringAsync(user.Id.ToString(), userString, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
                });
            }
        }
        else
        {
            Console.WriteLine($"{user.Name} извлечен из кэша");
        }
        return user;
    }
}*/