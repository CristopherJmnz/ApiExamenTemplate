using ApiExamenTemplate.Data;
using ApiExamenTemplate.Helpers;
using ApiExamenTemplate.Repositories;
using Azure.Security.KeyVault.Secrets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

//Configuracion de keyvaults
builder.Services.AddAzureClients(factory =>
{
    factory.AddSecretClient
    (builder.Configuration.GetSection("KeyVault"));
});
SecretClient secretClient = builder.Services.BuildServiceProvider().GetService<SecretClient>();
KeyVaultSecret secretConnectionString =
    await secretClient.GetSecretAsync("SqlAzure");

//config DB connection
string connectionString = secretConnectionString.Value;
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<DoctoresRepository>();
//string connectionString = builder.Configuration.GetConnectionString("AzureSql");
builder.Services.AddDbContext<DoctoresContext>(options => options.UseSqlServer(connectionString));

//Inyeccion del helper
HelperActionServicesOAuth helper = new HelperActionServicesOAuth(builder.Configuration);
builder.Services.AddSingleton<HelperActionServicesOAuth>(helper);


//Configuracion del JWT
builder.Services.AddAuthentication
    (helper.GetAuthenticateSchema())
    .AddJwtBearer(helper.GetJwtBearerOptions());


//CONFIG SWAGGER
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
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint(
        url: "/swagger/v1/swagger.json",
        name: "Api v1"
        );
    options.RoutePrefix = "";
});
app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

