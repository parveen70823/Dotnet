using Microsoft.EntityFrameworkCore;
using OdeToFood.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Logging.AddConsole();
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

/*
   In ASP.NET Core, middleware components handle HTTP requests and responses in a pipeline. Each middleware 
  can process, modify, or terminate requests before passing them to the next middleware.
 
 * var builder = WebApplication.CreateBuilder(args);
    var app = builder.Build();
    app.UseDeveloperExceptionPage(); // 1. Handles exceptions (only in development)
    app.UseHttpsRedirection(); // 2. Redirects HTTP to HTTPS
    app.UseStaticFiles(); // 3. Serves static files (CSS, JS, images)
    app.UseRouting(); // 4. Enables routing system
    app.UseAuthorization(); // 5. Enforces authorization policies
    app.MapControllers(); // 6. Maps controllers (API endpoints)
    app.Run();
*/

app.Use(SayHelloMiddleWare);//custom middleware


/*
 In ASP.NET Core, a RequestDelegate is a function that processes HTTP requests and can either:

Handle the request itself (e.g., returning a response).

Pass it to the next middleware in the pipeline.*/
RequestDelegate SayHelloMiddleWare(RequestDelegate next)
{
    return async ctx =>
    {
        if (ctx.Request.Path.StartsWithSegments("/hello"))
        {
            await ctx.Response.WriteAsync("Hello World");
        }
        else
        {
            await next(ctx);
        }
    };
}

app.UseHttpsRedirection();//middleware to redirect http request to https
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.UseNodeModules();

app.Run();
