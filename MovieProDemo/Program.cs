using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieProDemo.Data;
using MovieProDemo.Models.Settings;
using MovieProDemo.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var connectionString = ConnectionService.GetConnectionString(builder.Configuration);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString)
    );
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<SeedService>(); // Seed database

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

await SeedDatabase(app);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();


async Task SeedDatabase(WebApplication app)
{
    var dataService = app.Services
        .CreateScope()
        .ServiceProvider
        .GetRequiredService<SeedService>();

    await dataService.ManageDataAsync();
}