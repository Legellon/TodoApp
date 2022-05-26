using RavenTodoApp.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

builder.Services.AddSingleton(typeof(IRepository<>), typeof(RavenDbRepository<>));
builder.Services.AddSingleton<IItemRepository, ItemRepository>();
builder.Services.AddSingleton<IRavenDbContext, RavenDbContext>();

builder.Services.Configure<PersistenceSettings>(builder.Configuration.GetSection("Database"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();