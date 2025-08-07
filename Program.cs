using TaskManager.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI;
using Serilog;
using Microsoft.AspNetCore.Components;



//var builder = WebApplication.CreateBuilder(args);
var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
	ContentRootPath = AppContext.BaseDirectory,
	Args = args
});

//configure serilog ca logger global
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.Seq("http://localhost:5341") //daca folosesti Seq
    .CreateLogger();

builder.Host.UseSerilog(); // integrate cu Host-ul


// Add services to the container. EF Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Code Refactor, will delete this: //Identity
//builder.Services.AddIdentity<IdentityUser, IdentityRole>(opts =>
//{
//    opts.Password.RequiredLength = 6;
//    opts.Password.RequireNonAlphanumeric = false;
//    opts.User.RequireUniqueEmail = true;
//})
//.AddEntityFrameworkStores<AppDbContext>()
//.AddDefaultTokenProviders();

//Add JWTBearer
var jwt = builder.Configuration.GetSection("Jwt");
//code refactor //builder.Services.AddAuthentication(options =>
//{
//    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
builder.Services.AddAuthentication()
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwt["Issuer"],
        ValidAudience = jwt["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwt["Key"]))
    };
});

//Config Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdmin", policy =>
        policy.RequireRole("Admin"));

    options.AddPolicy("RequireManagerOrAdmin", policy =>
        policy.RequireRole("Manager", "Admin"));

    //Exemplu policy bazata pe claim
    options.AddPolicy("Over18", policy =>
        policy.RequireClaim("DateOfBirth")); // sau orice alt claim custom
});



//Adaugam Identity cu cookie-uri si EntityFramework store

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.User.RequireUniqueEmail = true;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<AppDbContext>();

//MVC + API
builder.Services.AddControllersWithViews();
builder.Services.AddControllers(); // Necesar pentru API

//Activam Pages pentru Identy UI
builder.Services.AddRazorPages();

//Swagger 
builder.Services.AddEndpointsApiExplorer();



// … după builder.Services.AddSwaggerGen():
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TaskManager API", Version = "v1" });

    // 1. Defineşte schema de securitate JWT Bearer
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",              // Header name
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Introduceți Bearer token JWT astfel: **Bearer &lt;token&gt;**"
    });

    // 2. Adaugă requirement la toate operaţiunile
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
      {
        new OpenApiSecurityScheme {
          Reference = new OpenApiReference {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
          }
        },
        Array.Empty<string>()
      }
    });
});

//Adauga serviciile Blazor
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<HttpClient>(sp =>
{
	var navigationManager = sp.GetRequiredService<NavigationManager>();
	return new HttpClient
	{
		BaseAddress = new Uri(navigationManager.BaseUri)
	};
});

builder.Services.AddHttpClient();



var app = builder.Build();

// Seed initial admin account
using (var scope = app.Services.CreateScope())
{
    await SeedData.EnsureAdminCreatedAsync(scope.ServiceProvider);
}


// Configure the HTTP request pipeline. //Middleware
//if (!app.Environment.IsDevelopment())
//{
app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskManager API v1");
    });

    //app.UseExceptionHandler("/Home/Error");
//}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub("/blazor");
app.MapFallbackToPage("/blazor/Host");

app.MapRazorPages();// mapam treseele Identity UI (/Identity/Account/Login, etc)
app.MapControllers(); //Maps your API controller too
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

public partial class Program { };