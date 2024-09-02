using LeagueLeaders.API.Middleware;
using LeagueLeaders.Application;
using LeagueLeaders.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "LeagueLeaders API",
        Version = "v1",
        Description = "This API provides access to information about leagues, teams, schedules, and leaderboards. Use this API to retrieve and manage data related to Champions League.",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Roman Vorobel",
            Email = "rvorobel@leobit.com",
            Url = new Uri("https://example.com/contact"),
        },
    });
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "api.xml"));
});

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});

builder.Services.AddDbContext<LeagueLeadersDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IScheduleSerivce, ScheduleSerivce>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<ILeaderboardService, LeaderboardService>();

builder.Services.AddScoped<ExceptionHandlingMiddleware>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();   

app.Run();