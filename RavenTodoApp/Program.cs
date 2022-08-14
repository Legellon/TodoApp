using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using RavenTodoApp.Persistence;
using RavenTodoApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddControllers();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

builder.Services
    .AddAuthorization(o =>
    {
        o.AddPolicy("AccessPolicy", policy => 
            policy.Requirements.Add(new SameOwnerRequirement()));
    })
    .AddSingleton<IAuthorizationHandler, ItemsAuthHandler>();
    
builder.Services
    .Configure<PersistenceSettings>(builder.Configuration.GetSection("Database"))
    .AddSingleton<IRavenDbContext, RavenDbContext>()
    .AddSingleton(typeof(IRepository<>), typeof(RavenDbRepository<>));

builder.Services
    .AddSingleton<IItemRepository, ItemRepository>()
    .AddSingleton<IUserRepository, UserRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseRouting();

app
    .UseAuthentication()
    .UseAuthorization()
    .UseCookiePolicy(new CookiePolicyOptions
    {
        MinimumSameSitePolicy = SameSiteMode.Strict
    });

app.UseHttpsRedirection();
// app.UseStaticFiles();

app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();