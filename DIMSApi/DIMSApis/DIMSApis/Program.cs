using DIMSApis.Interfaces;
using DIMSApis.Models.Data;
using DIMSApis.Repositories;
using DIMSApis.Services;
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

//Add more<
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Swagger  Solution", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Scheme = "bearer",
        Description = "Please insert JWT token into field"
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


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"])),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero,
    };
});


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<DIMSContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("concac")));

builder.Services.AddScoped<IAuth, AuthRepository>();
builder.Services.AddScoped<IUserManage, UserManageRepository> ();
builder.Services.AddScoped<IBookingManage, BookingManageRepository> ();
builder.Services.AddScoped<IQrManage, QrManageRepository> ();


builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IOtherService, OtherService>();
builder.Services.AddScoped<IMail, MailService>();
builder.Services.AddScoped<IGenerateQr, GenerateQrImageStringService>();
//>Add more

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{

//}
    app.UseSwagger();
    app.UseSwaggerUI();
app.UseHttpsRedirection();
//Add more<
//PAHI NAM TRC HAN app.UseAuthorization();
app.UseAuthentication();
//>Add more
app.UseAuthorization();

app.MapControllers();

app.Run();
