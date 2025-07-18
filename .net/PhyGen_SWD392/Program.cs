using Microsoft.EntityFrameworkCore;
using PhyGen_SWD392.Models;
using PhyGen_SWD392.Repositories.Interface;
using PhyGen_SWD392.Repositories.Repo;
using PhyGen_SWD392.Services;
using PhyGen_SWD392.Data;
using PhyGen_SWD392.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using PhyGen_SWD392.Helpers;
using PhyGen_SWD392.Models.Entities;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
//var frontendUrls = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "PhyGen API", Version = "v1" });

    var jwtScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Description = "Paste your JWT token directly here.",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "JWT"
        }
    };

    c.AddSecurityDefinition("JWT", jwtScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
{
    { jwtScheme, Array.Empty<string>() }
    }); 
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173", "https://phygen-ai.vercel.app")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddScoped<IAuthRepo, AuthRepo>();
builder.Services.AddScoped<AuthService>();

builder.Services.AddScoped<IAccountAdminRepo, AccountAdminRepo>();
builder.Services.AddScoped<AccountAdminService>();

builder.Services.AddScoped<IAccountUser, AccountUserRepo>();
builder.Services.AddScoped<AccountUserService>();

builder.Services.AddScoped<ITopicRepo, TopicRepo>();
builder.Services.AddScoped<TopicService>();
builder.Services.AddScoped<ISubjectRepo, SubjectRepo>();
builder.Services.AddScoped<SubjectService>();
builder.Services.AddScoped<IExamMatrixRepo, ExamMatrixRepo>();
builder.Services.AddScoped<ExamMatrixService>();
builder.Services.AddScoped<IMatrixSectionRepo, MatrixSectionRepo>();
builder.Services.AddScoped<MatrixSectionService>();
builder.Services.AddScoped<IMatrixDetailRepo, MatrixDetailRepo>();
builder.Services.AddScoped<MatrixDetailService>();
builder.Services.AddScoped<IQuestionRepo, QuestionRepo>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<DashboardService>();

builder.Services.AddScoped<AIGenerationService>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped<IEmailVerificationRepo, EmailVerificationRepo>();
builder.Services.AddScoped<EmailVerificationService>();

builder.Services.AddScoped<IForgetPasswordRepo, ForgetPasswordRepo>();
builder.Services.AddScoped<ForgetPasswordService>();

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ImageService>();
builder.Services.AddScoped<GoogleAuthService>();

builder.Services.Configure<CloudinarySettings>(
    builder.Configuration.GetSection("CloudinarySettings"));

builder.Services.AddSingleton<VnPayService>();

builder.Services.AddSingleton(cloudinary =>
{
    var config = builder.Configuration.GetSection("CloudinarySettings").Get<CloudinarySettings>();
    var account = new CloudinaryDotNet.Account(config.CloudName, config.ApiKey, config.ApiSecret);
    return new CloudinaryDotNet.Cloudinary(account);
});

builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis"))
);

builder.Services.AddScoped<RedisLogService>();
FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile(builder.Configuration["Firebase:CredentialPath"])
});
builder.Services.AddScoped<FirebaseNotificationService>();

builder.Services.AddDbContext<PhyGenDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));

var jwtSettings = builder.Configuration
    .GetSection("JwtSettings")
    .Get<JwtSettings>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
        };
    });


var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseCors("AllowFrontend");

app.Use(async (context, next) =>
{
    var token = context.Request.Headers["Authorization"].FirstOrDefault();
    if (!string.IsNullOrEmpty(token) && !token.StartsWith("Bearer "))
    {
        context.Request.Headers["Authorization"] = $"Bearer {token}";
    }
    await next();
});


app.UseAuthentication();
app.UseAuthorization();

//To deploy
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Urls.Add($"http://*:{port}");
Console.WriteLine($"Listening on port {port}");

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = "swagger";  // Để swagger UI chạy ở /swagger thay vì /
});
//

app.MapControllers();

app.Run();


