using System.Text;
using codeSparkRequestAPI.Interfaces;
using codeSparkRequestAPI.Models;
using codeSparkRequestAPI.Services;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;
var key = Environment.GetEnvironmentVariable("CONFIG_KEY");
builder.WebHost.UseUrls("http://0.0.0.0:5001");
builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthHandler>("BasicAuthentication", null);
var decryptedJson = BlowFishDecryption.JsonDecryptor.DecryptFile($"secure.{env.EnvironmentName}.appsettings.json", key);
var decryptedStream = new MemoryStream(Encoding.UTF8.GetBytes(decryptedJson));
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
builder.Services.Configure<emailSettings>(
    builder.Configuration.GetSection("EmailSettings"));
builder.Configuration.AddJsonStream(decryptedStream);
builder.Services.AddControllers();
builder.Services.AddTransient<IsendEmailInteface, sendEmailService>();
var app = builder.Build();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();