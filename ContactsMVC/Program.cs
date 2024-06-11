using ContactsMVC;
using ContactsMVC.Services;
using ContactsMVC.Services.IServices;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAutoMapper(typeof(MappingConfig)); //auto maper

builder.Services.AddHttpClient<IContactService, ContactService>(); // http contact

builder.Services.AddScoped<IContactService, ContactService>(); // Service


builder.Services.AddHttpClient<IAuthService, AuthService>(); // http user

builder.Services.AddScoped<IAuthService, AuthService>(); // service

builder.Services.AddDistributedMemoryCache();//cache dla servera

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => //cookie config
						{
							options.Cookie.HttpOnly = true;
							options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
							options.LoginPath = "/Auth/Login";
							options.AccessDeniedPath = "/Auth/AccessDenied";
							options.SlidingExpiration = true;
						});


builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); // accesor do sesji _layout
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromSeconds(10);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});

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

app.UseAuthorization();//dodanie autoryzacji

app.UseSession();//dodanie sesji

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
