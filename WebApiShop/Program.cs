using Microsoft.EntityFrameworkCore;
using Repositories;
using Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IPasswordValidityService, PasswordValidityService>();
builder.Services.AddControllers();
builder.Services.AddDbContext<MyShopContext>(option => option.UseSqlServer
("Data Source=ELISHEVA;Initial Catalog=MyShop;Integrated Security=True;Encrypt=True;Trust Server Certificate=True"));
//("Data Source=SRV2\\PUPILS;Initial Catalog=MyShop;Integrated Security=True;Trust Server Certificate=True"));
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
