using netflix_clone.data;  // Import the Data namespace
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.StaticFiles;
using netflix_clone.Attributes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add session services
builder.Services.AddDistributedMemoryCache();  // Use memory cache for session data
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);  // Set session timeout
    options.Cookie.HttpOnly = true;                  // Make the session cookie HTTP-only
    options.Cookie.IsEssential = true;               // Mark the cookie as essential
});


builder.Services.AddAuthentication("CookieAuthentication")
    .AddCookie("CookieAuthentication", options =>
    {
        options.LoginPath = "/User/Login"; // Redirect to login page
        options.AccessDeniedPath = "/User/Login"; // Redirect if access is denied
    });

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:5034")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Register the DbContext with the PostgreSQL connection string
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// builder.Services.AddControllersWithViews(options =>
// {
//     options.Filters.Add<AuthorizeUserAttribute>();
// });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Enable CORS for the application
app.UseCors("AllowSpecificOrigin");

// Enable session middleware
app.UseSession();

app.UseHttpsRedirection();

// Configure static file serving with custom MIME types
var contentTypeProvider = new FileExtensionContentTypeProvider();
contentTypeProvider.Mappings[".m3u8"] = "application/vnd.apple.mpegurl"; // Correct MIME type for .m3u8
contentTypeProvider.Mappings[".ts"] = "video/mp2t";                     // Correct MIME type for .ts

// Serve static files (general and videos)
app.UseStaticFiles(); // General static files in wwwroot

// Serve static files from the /videos/hls folder with MIME type handling
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "videos", "hls")
    ),
    RequestPath = "/videos/hls",
    ContentTypeProvider = contentTypeProvider,
    ServeUnknownFileTypes = true, // Allow unknown file types
    OnPrepareResponse = ctx =>
    {
        if (ctx.File.Name.EndsWith(".m3u8"))
        {
            ctx.Context.Response.ContentType = "application/vnd.apple.mpegurl";
        }
        else if (ctx.File.Name.EndsWith(".ts"))
        {
            ctx.Context.Response.ContentType = "video/mp2t";
        }
    }
});

app.UseRouting();

app.UseAuthentication(); // Enable authentication

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Login}/{id?}");

app.Run();
