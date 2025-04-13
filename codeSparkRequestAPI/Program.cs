using codeSparkRequestAPI.Interfaces;
using codeSparkRequestAPI.Models;
using codeSparkRequestAPI.Services;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://0.0.0.0:5001");
builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthHandler>("BasicAuthentication", null);
builder.Services.Configure<emailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddControllers();
builder.Services.AddTransient<IsendEmailInteface, sendEmailService>();
var app = builder.Build();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();