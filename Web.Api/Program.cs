using Refit;

using StackExchange.Redis;

using System.Text.Json;

using Web.Api.CalenderService;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddRedis("RedisCach");


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRefitClient<ICalenderService>().ConfigureHttpClient(option =>
{
    option.BaseAddress = new Uri("https://holidayapi.ir");
});

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/api/events/{year}/{month}/{day}", async (
    ICalenderService calenderservice,
    IConnectionMultiplexer redisConnection,
    ILogger<Program> Logger,
    int year, int month, int day) =>
    {
        var chachkey = $"events_{year}_{month}_{day}";
        var database = redisConnection.GetDatabase();
        if(await database.KeyExistsAsync(chachkey))
        {
            Logger.LogWarning("Chach hit for {chachkey}", chachkey);
            var chachValue=JsonSerializer.Deserialize<List<EventApiModel>>(await database.StringGetAsync(chachkey));
            return Results.Ok(chachValue);
        }
        var apievents = await calenderservice.GetEventAsync(year, month, day);
        var events = apievents.Events
        .Select(x => new EventApiModel(x.Description, x.AdditionalDescription, x.IsHoliday, x.IsReligious))
        .ToList();
        await database.StringSetAsync(chachkey, JsonSerializer.Serialize(events));

        return Results.Ok(events);
    });
app.Run();





public record EventApiModel(string Description, string AdditionalDescription, bool IsHoliday, bool IsReligious);