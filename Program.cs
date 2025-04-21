using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using server_truyen_v.Services;
using server_truyen_v.Data;
using server_truyen_v.Models;
using server_truyen_v;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<Cloudinarys>();
builder.Services.AddScoped<ReadPdf>();
builder.Services.AddScoped<ServiceStory>();
builder.Services.AddScoped<ReadTxt>();
builder.Services.AddScoped<DetailService>();
builder.Services.AddScoped<ServiceComent>();
builder.Services.AddScoped<StoryStatisticsService>();
builder.Services.AddScoped<JwtTokenService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policyBuilder =>
        {
            policyBuilder.AllowAnyOrigin();
            policyBuilder.AllowAnyHeader();
            policyBuilder.AllowAnyMethod();
        });
});


var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"] ?? "vu_dinh_huynh_vu_dinh_huynh_vu_dinh_huynh");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.AddAuthorization();
// Add services to the container.

builder.Services.AddControllers();
// Thêm DbContext và cấu hình kết nối
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
// Cấu hình Swagger với Bearer Token
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });

    // Thêm Bearer Token vào Swagger UI
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Nhập token theo định dạng: Bearer {token}"
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
            new string[] {}
        }
    });

    // Thêm bộ lọc cho file upload và token
    c.OperationFilter<AddFileUploadOperation>();
});


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
    c.RoutePrefix = string.Empty; // Mặc định mở Swagger UI ở đường dẫn gốc "/"
});





app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication(); // Xác thực
app.UseAuthorization();
app.MapControllers();

app.Run();
