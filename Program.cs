using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using ShopKaro.Models.Users;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// Register the DbContext and configure it to  use SQLite
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite3ConnectionString")));


// JWT configuration
var key = Encoding.ASCII.GetBytes("f3f5cbac3d4d1ade80eef71db4324270c83f229c73614fe4f2d1d51ca157fb6a5c385e8078b2beab24fda4d83dc4772e09f2d350125115edb99ee43846590b3d17818f7d27d4b43d5419820877f425b5295e21999e6f76e20c719bda3a0e479de5692b237ff805ccbead61be96bdc2d414f5ec34e131947f14b56f06267fa74d0cdc5a709f65178fb660f6d0285cda0cb25da143f3b634ac267736d85499a5bbd923b6452fa7930460f98834a2cacfd805cfa34006a4a27dba9ad50d5d5c5045b0670a6462e5f8405c4c067a74c0ad60edab1516ed5a10bdfc12d514af417be14f4566308674fa4abc961cbce8abd5c987539b3fbeb8b67ecb2c29414c2ad30b20ca0ad924d7eca3d68380909f5c4b04714b989529f0594cfad15b277d22d3e7"); // Replace with a secure key


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, // Ensure the token has a valid issuer
        ValidateAudience = true, // Ensure the token has a valid audience
        ValidateLifetime = true, // Ensure the token has not expired
        ValidateIssuerSigningKey = true, // Ensure the signing key is valid

        ValidIssuer = "ShopKaro", // Replace with your app's issuer
        ValidAudience = "ShopKaro", // Replace with your app's audience
        IssuerSigningKey = new SymmetricSecurityKey(key) // Signing key
    };

    // Optional: Configure event handlers
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine("Authentication failed: " + context.Exception.Message);
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine("Token validated.");
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization(); // Add role-based authorization

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    // app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Enable JWT authentication middleware
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=SignUp}/{id?}");

app.Run();
