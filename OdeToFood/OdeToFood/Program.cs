using Microsoft.EntityFrameworkCore;
using OdeToFood.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddDbContextPool<OdeToFoodDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("OdeToFood"));
});
//builder.Services.AddSingleton<IRestaurantData, InMemoryRestaurantData>(); // Registering a singleton service
builder.Services.AddScoped<IRestaurantData, SqlRestaurantData>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.UseNodeModules();

app.Run();
