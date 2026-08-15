using News.OpenApi;
using News.Security;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

string NewsConnectionString = builder.Configuration.GetConnectionString("NewsSystem");
string securityConnectionString = builder.Configuration.GetConnectionString("Security");
string secretKey = builder.Configuration["jwt:SecretKey"];
string issuer = builder.Configuration["jwt:Issuer"];
string audience = builder.Configuration["jwt:Audience"];

News.BootStrap.BootStrap.WierUpNewsSystem(builder.Services,NewsConnectionString,securityConnectionString,secretKey,issuer,audience);




// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    options.AddScalarTransformers();
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});
builder.Services.AddSingleton<BearerSecuritySchemeTransformer>();


var app = builder.Build();
await IdentityDataSeeder.SeedAsync(app.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}



app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
