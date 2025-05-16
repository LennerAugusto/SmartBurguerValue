using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.Interfaces;
using SmartBurguerValueAPI.Interfaces.IProducts;
using SmartBurguerValueAPI.IRepository.IProducts;
using SmartBurguerValueAPI.IRepository.IRepositoryBase;


//using SmartBurguerValueAPI.Filters;
using SmartBurguerValueAPI.Mappings;
using SmartBurguerValueAPI.Repository;
using SmartBurguerValueAPI.Repository.Base;
using SmartBurguerValueAPI.Repository.ProductsRepository;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddControllers(options =>
//{
//    options.Filters.Add(typeof(ApiExceptionFilter));
//})
//.AddJsonOptions(options =>
//{
   // options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
//});
builder.Services.AddControllers();

//Mapeamento Interfaces
builder.Services.AddScoped<IUnityOfWork, UnityOfWork>();
builder.Services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
builder.Services.AddScoped<IUnityTypesRepository, UnityTypesProductsRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IEnterpriseRepository, EnterpriseRepository>();
builder.Services.AddScoped<IIngredientRepository, IngredientRepository>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
options.UseMySql(mySqlConnection,
                    ServerVersion.AutoDetect(mySqlConnection)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
