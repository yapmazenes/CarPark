using AspNetCore.Identity.MongoDbCore.Models;
using CarPark.Business.Abstract;
using CarPark.Business.Concrete;
using CarPark.Core.Repository.Abstract;
using CarPark.DataAccess.Abstract;
using CarPark.DataAccess.Concrete;
using CarPark.DataAccess.Context;
using CarPark.DataAccess.Repository;
using CarPark.DataAccess.Settings;
using CarPark.Entities.Concrete;
using CarPark.User.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;
using System.Globalization;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddMvc().
    AddDataAnnotationsLocalization(options => options.DataAnnotationLocalizerProvider = (type, factory) =>
    {
        var assemblyName = new AssemblyName(typeof(SharedModelResource).GetTypeInfo().Assembly.FullName);
        return factory.Create(nameof(SharedModelResource), assemblyName.Name);
    }).
    AddViewLocalization();

builder.Services.Configure<MongoConnectionSetting>(builder.Configuration.GetSection("MongoConnectionSetting"));
//Todo: Create a new Extension method as like BuildServiceProviders and use here.
//Todo: Create ServiceRegistration extension class Data Access and Business layer.

builder.Services.AddAuthentication(option =>
{
    option.DefaultScheme = IdentityConstants.ApplicationScheme;
    option.DefaultSignInScheme = IdentityConstants.ExternalScheme;
}).AddIdentityCookies(option => { });

builder.Services.AddIdentityCore<Personel>(option => { })
    .AddRoles<MongoIdentityRole>()
    .AddMongoDbStores<Personel, MongoIdentityRole, Guid>(builder.Configuration.GetSection("MongoConnectionSetting:ConnectionString").Value,
                                                         builder.Configuration.GetSection("MongoConnectionSetting:Database").Value)
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(option =>
{
    option.Cookie.HttpOnly = true;
    option.ExpireTimeSpan = TimeSpan.FromSeconds(5);
    option.LoginPath = "/Account/Login";
    option.SlidingExpiration = true;
});

builder.Services.AddScoped(typeof(IRepository<>), typeof(MongoRepositoryBase<>));
builder.Services.AddScoped<IPersonelDataAccess, PersonelDataAccess>();
builder.Services.AddScoped<IPersonelService, PersonelManager>();

builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new List<CultureInfo>
    {
        new CultureInfo("en-US"),
        new CultureInfo("tr-TR"),
        new CultureInfo("ar-SA"),
    };

    options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("tr-TR");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;

    options.RequestCultureProviders = new List<IRequestCultureProvider>
    {
        new QueryStringRequestCultureProvider(),
        new CookieRequestCultureProvider(),
        new AcceptLanguageHeaderRequestCultureProvider()
    };
});

builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console()
        .WriteTo.File("log.txt")
        .WriteTo.Seq("http://localhost:5341")
        .Enrich.WithProperty("ApplicationName", "CarPark.User")
        .Enrich.WithMachineName());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

var cultureOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(cultureOptions.Value);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
