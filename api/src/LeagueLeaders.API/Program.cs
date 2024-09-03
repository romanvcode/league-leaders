using LeagueLeaders.API.Middleware;
using LeagueLeaders.Application.Leaderboard;
using LeagueLeaders.Application.Schedule;
using LeagueLeaders.Application.Teams;
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
        Contact = new OpenApiContact
        {
            Name = "Roman Vorobel",
            Email = "rvorobel@leobit.com",
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

builder.Services.AddScoped<IScheduleService, ScheduleService>();
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

public partial class Program { }