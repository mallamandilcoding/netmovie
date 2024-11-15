using netflix_clone.data;  // Import the Data namespace
using Microsoft.EntityFrameworkCore;
// using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add Swagger services
// builder.Services.AddSwaggerGen(c =>
// {
//     c.SwaggerDoc("v1", new OpenApiInfo
//     {
//         Title = "My MVC Application API",
//         Version = "v1",
//         Description = "This is a simple example of Swagger integration in ASP.NET Core MVC.",
//     });
//     var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
//     var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
//     c.IncludeXmlComments(xmlPath);
// });

// Register the DbContext with the PostgreSQL connection string
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    
    
    // app.UseSwagger(); // Generates the Swagger specification
    // app.UseSwaggerUI(options =>
    // {
    //     // Point to the swagger.json located in wwwroot/swagger
    //     options.SwaggerEndpoint("/swagger/swagger.json", "My API v1");
    //     options.RoutePrefix = string.Empty; // Optional: set Swagger UI as the root page
    //     options.RoutePrefix = "swagger"; // Optional: set Swagger UI as the root page
    // });
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();