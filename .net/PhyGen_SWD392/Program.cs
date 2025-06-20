using Microsoft.EntityFrameworkCore;
using PhyGen_SWD392.Models;
using PhyGen_SWD392.Repositories.Interface;
using PhyGen_SWD392.Repositories.Repo;
using PhyGen_SWD392.Services;
using PhyGen_SWD392.Data;
using PhyGen_SWD392.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddScoped<ITopicRepo, TopicRepo>();
builder.Services.AddScoped<TopicService>();
builder.Services.AddScoped<ISubjectRepo, SubjectRepo>();
builder.Services.AddScoped<SubjectService>();
builder.Services.AddScoped<IExamMatrixRepo, ExamMatrixRepo>();
builder.Services.AddScoped<ExamMatrixService>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddDbContext<PhyGenDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
app.UseSwaggerUI();
}
//app.UseSwagger();
//app.UseSwaggerUI(c =>
//{
//    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
//    c.RoutePrefix = string.Empty;  
//});

//app.UseHttpsRedirection();
app.MapControllers();

app.Run();


