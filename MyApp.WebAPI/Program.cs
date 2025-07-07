using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyApp.Application.Interface;
using MyApp.Application.Services;
using MyApp.Domain.Interfaces;
using MyApp.Infrastructure.Data;
using MyApp.Infrastructure.Repositories;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
// افزودن سرویس‌ها به کانتینر
builder.Services.AddControllers();

// تنظیم EF Core با SQL Server (یا هر دیتابیس دیگه‌ای که استفاده می‌کنی)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ثبت سرویس‌ها و ریپازیتوری‌ها برای Dependency Injection
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();

// تنظیم احراز هویت JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"] ?? "your-very-secure-secret-key"))
    };
});

// تنظیم CORS برای اجازه دادن به MVC
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMvc", builder =>
    {
        builder.WithOrigins("https://localhost:5002", "https://localhost:5003") // آدرس MVC
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// افزودن Swagger برای مستندسازی API
builder.Services.AddEndpointsApiExplorer();
// تنظیم Swagger با پشتیبانی از Bearer Token
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Library Management API", Version = "v1" });

    // تعریف طرح امنیتی Bearer
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "لطفاً توکن JWT را وارد کنید (بدون پیشوند Bearer)",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    // اعمال طرح امنیتی به همه endpointها
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});
var app = builder.Build();

// پیکربندی Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowMvc");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();