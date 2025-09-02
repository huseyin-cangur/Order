using Microsoft.EntityFrameworkCore;
using OrderAPI;
using OrderAPI.AutoMapper;
using OrderAPI.Entity;
using OrderAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<OrderContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddAutoMapper(typeof(MapProfile));

builder.Services.AddScoped<IOrderService, OrderService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<OrderContext>();


    db.Database.Migrate();

    // Seed data
    if (!db.Products.Any())
    {
        db.Products.AddRange(
            new Product { Name = "Laptop" },
            new Product { Name = "Telefon" },
            new Product { Name = "Tablet" }
        );

        db.SaveChanges();
    }
    if (!db.Customers.Any())
    {
        db.Customers.AddRange(
          new Customer { Name = "Hüseyin", Email = "huseyin@gmail.com", Address = "İzmir" },
              new Customer { Name = "Ahmet", Email = "ahmet@gmail.com", Address = "İstanbul" }
      );
        db.SaveChanges();
    }
}

app.UseHttpsRedirection();



app.Run();

