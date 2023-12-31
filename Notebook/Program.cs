using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Notebook.Models;
using Notebook.Mapper;
using AutoMapper;
using Notebook.BusinessLogic.Interfaces;
using Notebook.BusinessLogic.Services;
using Notebook;

var builder = WebApplication.CreateBuilder(args);

string connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));
builder.Services.AddControllersWithViews();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<INoteService, NoteService>();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

builder.Logging.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));
//builder.Logging.AddProvider(new FileLoggerProvider(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt")));

var mappingConfig = new MapperConfiguration(mc =>
{
	mc.AddProfile(new MapperProfile());
});
var mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization(); 

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
