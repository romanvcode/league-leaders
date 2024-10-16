using LeagueLeaders.API.Middleware;
using LeagueLeaders.Application.Leaderboard;
using LeagueLeaders.Application.Schedule;
using LeagueLeaders.Application.Teams;
using LeagueLeaders.Infrastructure.Clients.SportradarApi;
using LeagueLeaders.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using AspNetCore.Swagger.Themes;
using Polly;
using Polly.Extensions.Http;
using System.Net;
using LeagueLeaders.Application.ApiDataSync;
using LeagueLeaders.API.Workers;

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
builder.Services.AddScoped<IApiDataSyncService, ApiDataSyncService>();
builder.Services.AddScoped<ExceptionHandlingMiddleware>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost");
    });
});

static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
}

builder.Services.AddHttpClient<ISportradarApiClient, SportradarApiClient>(client =>
{
    client.BaseAddress = new Uri("https://api.sportradar.com/soccer/trial/v4/eu/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
})
    .SetHandlerLifetime(TimeSpan.FromMinutes(5))
    .AddPolicyHandler(GetRetryPolicy());

builder.Services.AddHostedService<ApiDataSyncBackgroundWorker>();

builder.Services.Configure<SportradarSettings>(builder.Configuration.GetSection("Sportradar"));
builder.Services.Configure<SyncSettings>(builder.Configuration.GetSection("Sync"));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(ModernStyle.Dark);
}

app.UseHttpsRedirection();
app.UseCors();
app.MapControllers();

app.MapGet("/", () => "Hello from League Leaders API!");

app.Run();

public partial class Program { }