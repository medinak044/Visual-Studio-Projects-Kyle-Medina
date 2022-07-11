global using StackExchange.Redis;
global using Core.Entities.Identity;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(MappingProfiles));
builder.Services.AddControllers();
builder.Services.AddDbContext<StoreContext>(x =>
    x.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<AppIdentityDbContext>(x =>
{
    x.UseNpgsql(builder.Configuration.GetConnectionString("IdentityConnection"));
});

builder.Services.AddSingleton<IConnectionMultiplexer>(c =>
{
    var configuration = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"), true);
    return ConnectionMultiplexer.Connect(configuration);
});
builder.Services.AddApplicationServices();
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddSwaggerDocumentation();
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
    });
});

// Configure the http request pipline
var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseSwaggerDocumentation();

app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Content")
    ),
    RequestPath = "/content"
});

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapFallbackToController("Index", "Fallback");

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var loggerFactory = services.GetRequiredService<ILoggerFactory>();
try
{
    var context = services.GetRequiredService<StoreContext>();
    await context.Database.MigrateAsync();
    await StoreContextSeed.SeedAsync(context, loggerFactory);

    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var identityContext = services.GetRequiredService<AppIdentityDbContext>();
    await identityContext.Database.MigrateAsync();
    await AppIdentityDbContextSeed.SeedUsersAsync(userManager);
}
catch (Exception ex)
{
    var logger = loggerFactory.CreateLogger<Program>();
    logger.LogError(ex, "An error occurred during migration");
}

await app.RunAsync();

#region .NET 5 legacy code
//namespace API
//{
//public class Program
//{
//    public static async Task Main(string[] args)
//    {
//        var host = CreateHostBuilder(args).Build();
//        using var scope = host.Services.CreateScope();
//        var services = scope.ServiceProvider;
//        var loggerFactory = services.GetRequiredService<ILoggerFactory>();
//        try 
//        {
//            var context = services.GetRequiredService<StoreContext>();
//            await context.Database.MigrateAsync();
//            await StoreContextSeed.SeedAsync(context, loggerFactory);

//            var userManager = services.GetRequiredService<UserManager<AppUser>>();
//            var identityContext = services.GetRequiredService<AppIdentityDbContext>();
//            await identityContext.Database.MigrateAsync();
//            await AppIdentityDbContextSeed.SeedUsersAsync(userManager);
//        }
//        catch (Exception ex)
//        {
//            var logger = loggerFactory.CreateLogger<Program>();
//            logger.LogError(ex, "An error occurred during migration");
//        }

//        host.Run();
//    }

//    public static IHostBuilder CreateHostBuilder(string[] args) =>
//        Host.CreateDefaultBuilder(args)
//            .ConfigureWebHostDefaults(webBuilder =>
//            {
//                webBuilder.UseStartup<Startup>();
//            });
//}
//}
#endregion