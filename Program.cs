using Microsoft.EntityFrameworkCore;
using SkillSwap.Contexts;
using SkillSwap.Services;
using SkillSwap.Interfaces;
using SkillSwap.Interfaces.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.Configure<ICookieOptions>(builder.Configuration.GetSection("Cookie"));
builder.Services.Configure<IJwtOptions>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowFrontend");

app.Run();
