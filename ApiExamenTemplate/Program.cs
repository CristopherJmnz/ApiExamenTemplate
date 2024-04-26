using ApiExamenTemplate.Data;
using ApiExamenTemplate.Helpers;
using ApiExamenTemplate.Repositories;
using Azure.Security.KeyVault.Secrets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.OpenApi.Models;

//var builder = WebApplication.CreateBuilder(args);

////Configuracion de keyvaults
//builder.Services.AddAzureClients(factory =>
//{
//    factory.AddSecretClient
//    (builder.Configuration.GetSection("KeyVault"));
//});
//SecretClient secretClient = builder.Services.BuildServiceProvider().GetService<SecretClient>();
//KeyVaultSecret secretConnectionString =
//    await secretClient.GetSecretAsync("SqlAzure");

////config DB connection
//string connectionString = secretConnectionString.Value;
//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddTransient<DoctoresRepository>();
////string connectionString = builder.Configuration.GetConnectionString("AzureSql");
//builder.Services.AddDbContext<DoctoresContext>(options=>options.UseSqlServer(connectionString));

////Inyeccion del helper
//HelperActionServicesOAuth helper = new HelperActionServicesOAuth(builder.Configuration);
//builder.Services.AddSingleton<HelperActionServicesOAuth>(helper);


////Configuracion del JWT
//builder.Services.AddAuthentication
//    (helper.GetAuthenticateSchema())
//    .AddJwtBearer(helper.GetJwtBearerOptions());


////CONFIG SWAGGER
//builder.Services.AddSwaggerGen(options =>
//{
//    options.SwaggerDoc("v1", new OpenApiInfo
//    {
//        Version = "v1",
//        Title = "Api Cristopher Jmnz",
//        Description = "Api",
//        Contact = new OpenApiContact
//        {
//            Name = "Cristopher Jimenez",
//            Email = "crisjimenez19@gmail.com"
//        }
//    });
//});
//var app = builder.Build();

//app.UseSwagger();
//app.UseSwaggerUI(options =>
//{
//    options.SwaggerEndpoint(
//        url: "/swagger/v1/swagger.json",
//        name: "Api v1"
//        );
//    options.RoutePrefix = "";
//});
//app.UseDeveloperExceptionPage();
//app.UseHttpsRedirection();

//app.UseAuthentication();
//app.UseAuthorization();

//app.MapControllers();

//app.Run();



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();




// Add services to the container.

builder.Services.AddAzureClients(factory =>

{

    factory.AddSecretClient

    (builder.Configuration.GetSection("KeyVault"));

});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Api Cristopher Jmnz",
        Description = "Api",
        Contact = new OpenApiContact
        {
            Name = "Cristopher Jimenez",
            Email = "crisjimenez19@gmail.com"
        }
    });
});

//DEBEMOS PODER RECUPERAR UN OBJETO INYECTADO EN CLASES  

//QUE NO TIENEN CONSTRUCTOR

SecretClient secretClient =

builder.Services.BuildServiceProvider().GetService<SecretClient>();

KeyVaultSecret secret =

    await secretClient.GetSecretAsync("SqlAzure");

string connectionString = secret.Value;


//builder.Services.AddSwaggerGen();
builder.Services.AddTransient<DoctoresRepository>();
//string connectionString = builder.Configuration.GetConnectionString("SqlAzure");
builder.Services.AddDbContext<DoctoresContext>(options => options.UseSqlServer(connectionString));

HelperActionServicesOAuth helper = new HelperActionServicesOAuth(builder.Configuration);
builder.Services.AddSingleton<HelperActionServicesOAuth>(helper);
builder.Services.AddAuthentication(helper.GetAuthenticateSchema()).AddJwtBearer(helper.GetJwtBearerOptions());



var app = builder.Build();
//app.UseOpenApi();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "api prueba v1");
    options.RoutePrefix = "";
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
