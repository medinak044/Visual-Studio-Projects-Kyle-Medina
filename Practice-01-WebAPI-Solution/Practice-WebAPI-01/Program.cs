using Microsoft.EntityFrameworkCore;
using Practice_WebAPI_01;
using Practice_WebAPI_01.Data;
using Practice_WebAPI_01.Interfaces;
using Practice_WebAPI_01.Repository;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);
var myAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddTransient<Seed>(); // dotnet run seeddata
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); // ref: https://youtu.be/K4WuxwkXrIY?list=PL82C6-O4XrHdiS10BLh23x71ve9mQCln0&t=1660
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Establish EntityFramework connection between data context file and SqlServer database
builder.Services.AddDbContext<DataContext>(options => 
{
    //options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddCors(
    options =>
    {
        options.AddPolicy(
            name: myAllowSpecificOrigins,
            policy =>
            {
                policy.WithOrigins("http://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
    }
);

var app = builder.Build();

#region Seed data
if (args.Length == 1 && args[0].ToLower() == "seeddata")
    SeedData(app);

void SeedData(IHost app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (var scope = scopedFactory.CreateScope())
    {
        var service = scope.ServiceProvider.GetService<Seed>();
        service.SeedDataContext();
    }
}
#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(myAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
