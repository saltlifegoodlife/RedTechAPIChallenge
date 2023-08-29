using MySql.Data.MySqlClient;
using RedTechAPIChallenge.Models.Repositories;
using System.Data;

var builder = WebApplication.CreateBuilder(args);




builder.Services.AddTransient<IDbConnection>((s) =>
{
    IDbConnection conn = new MySqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"));
    conn.Open();
    return conn;
});

builder.Services.AddTransient<IOrderRepository, OrderRepository>();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();




app.MapControllerRoute(
    name: "query",
    pattern: "{controller}/{action}");




app.UseCors("AllowAnyOrigin");
app.Run();
