using Microsoft.EntityFrameworkCore;
using spilchat_api.Models;

var builder = WebApplication.CreateBuilder(args);

// EF Core baðlantýsý
builder.Services.AddDbContext<SpilchatDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app =  builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();
