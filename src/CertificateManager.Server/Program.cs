using CertificateManager.Server.Data;
using CertificateManager.Server.Models; // For ApplicationUser
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Configure SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=certificatemanager.db"));

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// Add ASP.NET Core Identity & Identity Endpoints
builder.Services.AddAuthorization(); // Ensure authorization services are registered
builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add CORS policy for Blazor client (existing)
// NOTE: The "PermissivePlaceholderCors" policy below is added for development convenience and MUST be reviewed and secured for production.
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermissivePlaceholderCors", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add CORS policy for Blazor client
var allowedOrigins = builder.Configuration["AllowedOrigins"]?.Split(';', StringSplitOptions.RemoveEmptyEntries) 
    ?? new[] { "https://localhost:5001", "http://localhost:5000" };

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.MapOpenApi();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Use HTTPS redirection in production only
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// NOTE: Security headers below are placeholders and/or permissive for development.
// MUST be reviewed and configured securely for production.
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Append("X-Frame-Options", "DENY"); // Consider SAMEORIGIN if framing is needed from the same origin
    context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin"); // A common reasonable default
    // Extremely permissive CSP for development. REVIEW AND HARDEN FOR PRODUCTION.
    // Example: "default-src 'self'; script-src 'self' 'wasm-unsafe-eval'; style-src 'self' 'unsafe-inline' https://cdn.jsdelivr.net; font-src 'self' https://cdn.jsdelivr.net; img-src 'self' data:; object-src 'none'; base-uri 'self'; form-action 'self'; frame-ancestors 'none';"
    context.Response.Headers.Append("Content-Security-Policy", 
        "default-src * 'unsafe-inline' 'unsafe-eval' data: blob:; " + // blob: might be needed for Blazor WASM debugging
        "script-src * 'unsafe-inline' 'unsafe-eval'; " +
        "style-src * 'unsafe-inline'; " +
        "img-src * data: blob:; " +
        "font-src * data:; " +
        "object-src 'none'; " +
        "frame-ancestors 'none';"); // Deny framing by default
    // Permissions-Policy: Deny common sensitive features by default. Adjust as needed.
    context.Response.Headers.Append("Permissions-Policy", "geolocation=(), microphone=(), camera=(), payment=(), usb=(), accelerometer=(), gyroscope=(), magnetometer=()");
    await next();
});


app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

// app.UseCors("AllowBlazorClient"); // Temporarily replaced by PermissivePlaceholderCors for review
app.UseCors("PermissivePlaceholderCors"); // NOTE: Using highly permissive CORS policy. MUST be secured for production.

app.UseAuthentication(); // Must be before UseAuthorization
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

// Map Identity API endpoints
app.MapIdentityApi<ApplicationUser>();

// Initialize the database
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await db.Database.MigrateAsync(); // Apply pending migrations
}

app.Run();
