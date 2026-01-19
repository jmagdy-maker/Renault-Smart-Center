using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RenaultSmartCenter.Application;
using RenaultSmartCenter.BlazorUI.Services;
using RenaultSmartCenter.Infrastructure;
using RenaultSmartCenter.Infrastructure.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// -------------------------------
// Add services to the container
// -------------------------------
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// -------------------------------
// Database & Identity
// -------------------------------
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

// -------------------------------
// Authentication (JWT Bearer)
// -------------------------------
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        var jwtSettings = builder.Configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"];

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
        };
    });

// -------------------------------
// Authorization
// -------------------------------
builder.Services.AddAuthorizationCore(); // ??? ?? Blazor Server

// -------------------------------
// Protected Browser Storage
// -------------------------------
builder.Services.AddScoped<ProtectedLocalStorage>();

// -------------------------------
// Custom Services
// -------------------------------
// Custom AuthenticationStateProvider
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
    provider.GetRequiredService<CustomAuthenticationStateProvider>());

// LocalStorage wrapper
builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();

// ApiClient
builder.Services.AddScoped<ApiClient>();

// -------------------------------
// HTTP Client
// -------------------------------
builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7001");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

// -------------------------------
// Build App
// -------------------------------
var app = builder.Build();

// -------------------------------
// Configure the HTTP request pipeline
// -------------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
