//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using Microsoft.OpenApi.Models;
//using MySqlConnector;
//using SmartBurguerValueAPI.Context;
//using SmartBurguerValueAPI.Interfaces;
//using SmartBurguerValueAPI.Interfaces.IProducts;
//using SmartBurguerValueAPI.IRepository.IProducts;
//using SmartBurguerValueAPI.IRepository.IRepositoryBase;
////using SmartBurguerValueAPI.Filters;
//using SmartBurguerValueAPI.Mappings;
//using SmartBurguerValueAPI.Models;
//using SmartBurguerValueAPI.Repository;
//using SmartBurguerValueAPI.Repository.Base;
//using SmartBurguerValueAPI.Repository.ProductsRepository;
//using SmartBurguerValueAPI.Services;
//using System.Text;
//using System.Text.Json.Serialization;


//var builder = WebApplication.CreateBuilder(args);

////builder.Services.AddControllers(options =>
////{
////    options.Filters.Add(typeof(ApiExceptionFilter));
////})
////.AddJsonOptions(options =>
////{
//// options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
////});


//builder.Services.AddControllers();

////Autentica��o 
//builder.Services.AddAuthorization();

//builder.Services.AddIdentity<ApplicationUser, IdentityRole>().
//        AddEntityFrameworkStores<AppDbContext>()
//        .AddDefaultTokenProviders();
//var secretKey = builder.Configuration["JWT:SecretKey"]
//                   ?? throw new ArgumentException("Invalid secret key!!");

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//}).AddJwtBearer(options =>
//{
//    options.SaveToken = true;
//    options.RequireHttpsMetadata = false;
//    options.TokenValidationParameters = new TokenValidationParameters()
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        ClockSkew = TimeSpan.Zero,
//        ValidAudience = builder.Configuration["JWT:ValidAudience"],
//        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
//        IssuerSigningKey = new SymmetricSecurityKey(
//                           Encoding.UTF8.GetBytes(secretKey))
//    };
//});

//builder.Services.AddAuthorization(options =>{
//    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
//    options.AddPolicy("EnterpriseOnly", policy => policy.RequireRole("Enterprise"));
//    options.AddPolicy("EnterpriseEmployeeOnly", policy => policy.RequireRole("EnterpriseEmployee"));
//});

////Mapeamento Interfaces
//builder.Services.AddScoped<IUnityOfWork, UnityOfWork>();
//builder.Services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
//builder.Services.AddScoped<IUnityTypesRepository, UnityTypesProductsRepository>();
//builder.Services.AddScoped<IProductRepository, ProductRepository>();
//builder.Services.AddScoped<IEnterpriseRepository, EnterpriseRepository>();
//builder.Services.AddScoped<IIngredientRepository, IngredientRepository>();
//builder.Services.AddScoped<ISalesGoalRepository, SalesGoalRepository>();
//builder.Services.AddScoped<IDailyEntryRepository, DailyEntryRepository>();
//builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
//builder.Services.AddScoped<IEmployeeWorkScheduleRepository, EmployeeWorkScheduleRepository>();
//builder.Services.AddScoped<IPurchaseRepository, PurchaseRepository>();
//builder.Services.AddScoped<IFinancialSnapshotsRepository, FinancialSnapshotsRepository>();
//builder.Services.AddScoped<IProductCostAnalysisRepository, ProductCostAnalysisRepository>();

////Mapeamento Servi�os
//builder.Services.AddScoped<ITokenService, TokenService>();

//builder.Services.AddAutoMapper(typeof(MappingProfile));

//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo { Title = "apicatalogo", Version = "v1" });

//    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
//    {
//        Name = "Authorization",
//        Type = SecuritySchemeType.ApiKey,
//        Scheme = "Bearer",
//        BearerFormat = "JWT",
//        In = ParameterLocation.Header,
//        Description = "Bearer JWT ",
//    });
//    c.AddSecurityRequirement(new OpenApiSecurityRequirement
//                {
//                    {
//                          new OpenApiSecurityScheme
//                          {
//                              Reference = new OpenApiReference
//                              {
//                                  Type = ReferenceType.SecurityScheme,
//                                  Id = "Bearer"
//                              }
//                          },
//                         new string[] {}
//                    }
//                });
//});
//string mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");

//builder.Services.AddDbContext<AppDbContext>(options =>
//options.UseMySql(mySqlConnection,
//                    ServerVersion.AutoDetect(mySqlConnection)));

//var app = builder.Build();


//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.Interfaces;
using SmartBurguerValueAPI.Interfaces.IProducts;
using SmartBurguerValueAPI.IRepository.IProducts;
using SmartBurguerValueAPI.IRepository.IRepositoryBase;
using SmartBurguerValueAPI.Mappings;
using SmartBurguerValueAPI.Models;
using SmartBurguerValueAPI.Repository;
using SmartBurguerValueAPI.Repository.Base;
using SmartBurguerValueAPI.Repository.ProductsRepository;
using SmartBurguerValueAPI.Services;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// =====================
// Configura��o do banco de dados
// =====================
var mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(mySqlConnection, ServerVersion.AutoDetect(mySqlConnection)));

// =====================
// Configura��o de identidade e autentica��o JWT
// =====================
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

var secretKey = builder.Configuration["JWT:SecretKey"]
    ?? throw new ArgumentException("Invalid secret key!!");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

// =====================
// Pol�ticas de autoriza��o
// =====================
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("EnterpriseOnly", policy => policy.RequireRole("Enterprise"));
    options.AddPolicy("EnterpriseEmployeeOnly", policy => policy.RequireRole("EnterpriseEmployee"));
});

// =====================
// Servi�os e Reposit�rios
// =====================
builder.Services.AddScoped<IUnityOfWork, UnityOfWork>();
builder.Services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
builder.Services.AddScoped<IUnityTypesRepository, UnityTypesProductsRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IEnterpriseRepository, EnterpriseRepository>();
builder.Services.AddScoped<IIngredientRepository, IngredientRepository>();
builder.Services.AddScoped<ISalesGoalRepository, SalesGoalRepository>();
builder.Services.AddScoped<IDailyEntryRepository, DailyEntryRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeWorkScheduleRepository, EmployeeWorkScheduleRepository>();
builder.Services.AddScoped<IPurchaseRepository, PurchaseRepository>();
builder.Services.AddScoped<IPurchaseItemRepository, PurchaseItemRepository>();
builder.Services.AddScoped<IFinancialSnapshotsRepository, FinancialSnapshotsRepository>();
builder.Services.AddScoped<IProductCostAnalysisRepository, ProductCostAnalysisRepository>();
builder.Services.AddScoped<IAnalysisByPeriodRepository, AnalysisByPeriodRepository>();  
// =====================
// Servi�os auxiliares
// =====================
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddAutoMapper(typeof(MappingProfile));

// =====================
// CORS para acesso do Blazor WebAssembly
// =====================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorFrontend", policy =>
    {
        policy
            .WithOrigins("https://localhost:7056") // Porta do Blazor
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// =====================
// Controllers e JSON
// =====================
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// =====================
// Swagger
// =====================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SmartBurguerValueAPI", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT no formato: Bearer {token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

var app = builder.Build();

// =====================
// Middleware
// =====================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowBlazorFrontend");

app.UseAuthentication(); // Importante: antes do UseAuthorization
app.UseAuthorization();

app.MapControllers();

app.Run();
