using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using PhoneNumbers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPIWhatsApp;

var builder = WebApplication.CreateBuilder(args);

//Authorization
builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddAuthorization();

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast",
    [Authorize(Roles ="Admin")]() =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateTime.Now.AddDays(index),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
});

app.MapGet("/whatsapp", () =>
{
    return "Hello!";
});

app.MapPost("/whatsapp", (Root root) =>
{
    var forecast = new whatsapp(root);

    var phoneUtil = PhoneNumberUtil.GetInstance();
    PhoneNumber numberProto = phoneUtil.Parse(root.msisdn, "ZZ");
    //int countryCode = numberProto.CountryCode();

    Console.WriteLine("Formatted phone number : ");
    Console.WriteLine("formattedPhone");

    return forecast;
});

app.MapPost("/minimalapi/security/getToken",
[AllowAnonymous] (User user) =>
{

    if (user.UserName == "user1" && user.Password == "password1")
    {
        var issuer = builder.Configuration["Jwt:Issuer"];
        var audience = builder.Configuration["Jwt:Audience"];
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);
        DateTime expires = DateTime.UtcNow.AddHours(1);
        List<Claim> claims = new List<Claim>() {
            new Claim (ClaimTypes.Role,"Admin")
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            signingCredentials: credentials,
            expires: expires,
            claims: claims
            );

        var tokenHandler = new JwtSecurityTokenHandler();
        var stringToken = tokenHandler.WriteToken(token);

        return Results.Ok(stringToken);
    }
    else
    {
        return Results.Unauthorized();
    }
});

app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

internal record whatsapp(Root root)
{

}

