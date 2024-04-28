using EntityFrameworkCore.UseRowNumberForPaging;
using ExcelOnline.Api.Services;
using ExcelOnline.Api.Services.Implementations;
using ExcelOnline.Api.Unities;
using ExcelOnline.Data.DbContexts;
using ExcelOnline.Data.Repositories;
using ExcelOnline.Data.Repositories.Implementations;
using FileContextCore;
using FileContextCore.FileManager;
using FileContextCore.Serializer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Configuration;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Description = "Token: Bearer Token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
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
            Array.Empty<string>()
        }
    });
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ExcelOnline.Api", Version = "v1" });
});
builder.Services.AddCors();

string secretKey = "YourKey-2374-OFFKDI940NG7:56753253-tyuw-5769-0921-kfirox29zoxv";
string validIssuer = "saleinfo";
string validAudience = "saleinfo";

builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(token =>
{
    token.RequireHttpsMetadata = false;
    token.SaveToken = true;
    token.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ValidateIssuer = true,
        ValidIssuer = validIssuer,
        ValidateAudience = true,
        ValidAudience = validAudience,
        RequireExpirationTime = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddHttpContextAccessor();

var section = builder.Configuration.GetSection("ConnectionStrings").GetChildren().FirstOrDefault();
if (section != null) 
{
    if (section.Key.StartsWith("MySql"))
    {
        builder.Services.AddDbContext<AppDbContext>(options =>
                       options.UseMySQL(section.Value ?? string.Empty));
    }
    else if (section.Key.StartsWith("SqlServer")) 
    {
        builder.Services.AddDbContext<AppDbContext>(options =>
                       options.UseSqlServer(section.Value ?? string.Empty, b => b.UseRowNumberForPaging()));
    }
}
else
{
    builder.Services.AddDbContext<AppDbContext>(options => 
    {
        options.UseInMemoryDatabase("SaleInfo");
    });
}

builder.Services.AddAutoMapper(typeof(TransfersMappingProfile));

builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<ISaleStatusService, SaleStatusService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IExcelService, ExcelService>();

builder.Services.AddMemoryCache();
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseCors(builder =>
{
    builder
        .SetIsOriginAllowed(_ => true)
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
