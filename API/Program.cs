using API.Extensions;
using API.Helpers;
using API.MiddleWare;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApplicationServices();
builder.Services.AddDbContext<CurrencyConverterDbContext>(x => x.UseSqlServer(connectionString));
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MappingProfiles));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionMiddleWare>();

app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = builder.Services.BuildServiceProvider().CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactry = services.GetRequiredService<ILoggerFactory>();
    try
    {
        var context = services.GetRequiredService<CurrencyConverterDbContext>();
        await context.Database.MigrateAsync();
        await CurrencyConverterDbContextSeed.SeedAsync(context, loggerFactry);


    }
    catch (Exception ex)
    {
        var logger = loggerFactry.CreateLogger<Program>();
        logger.LogError(ex, "An error occurred during migration");
    }
}

//app.Run();



await app.RunAsync();
