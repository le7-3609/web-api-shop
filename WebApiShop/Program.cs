using Microsoft.EntityFrameworkCore;
using Repositories;
using Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IPasswordValidityService, PasswordValidityService>();

builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<IProductService, ProductService>();

builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<ICategoryService, CategoryService>();

builder.Services.AddTransient<IOrderRepository, OrderRepository>();
builder.Services.AddTransient<IOrderService, OrderService>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<MyShopContext>(option => option.UseSqlServer
//("Data Source=ELISHEVA;Initial Catalog=MyShop;Integrated Security=True;Encrypt=True;Trust Server Certificate=True"));
("Data Source=SRV2\\PUPILS;Initial Catalog=MyShop;Integrated Security=True;Trust Server Certificate=True"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "MyAPI-V1");
    });
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
