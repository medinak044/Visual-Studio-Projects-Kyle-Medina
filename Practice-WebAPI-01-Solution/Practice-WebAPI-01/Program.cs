using Microsoft.EntityFrameworkCore;
using Practice_WebAPI_01;
using Practice_WebAPI_01.Data;
using Practice_WebAPI_01.Interfaces;
using Practice_WebAPI_01.Repository;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//builder.Services.AddTransient<Seed>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); // ref: https://youtu.be/K4WuxwkXrIY?list=PL82C6-O4XrHdiS10BLh23x71ve9mQCln0&t=1660
builder.Services.AddScoped<IHeroRepository, HeroRepository>();
builder.Services.AddScoped<IWeaponTypeRepository, WeaponTypeRepository>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Establish EntityFramework connection between data context file and SqlServer database
builder.Services.AddDbContext<DataContext>(options => 
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
