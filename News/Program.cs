using News.OpenApi;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

string NewsConnectionString = builder.Configuration.GetConnectionString("NewsSystem");
string securityConnectionString = builder.Configuration.GetConnectionString("Security");
string secretKey = builder.Configuration["jwt:SecretKey"];

News.BootStrap.BootStrap.WierUpNewsSystem(builder.Services,NewsConnectionString,securityConnectionString,secretKey);




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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}



app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
