using Controllers.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Controllers.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ICacheService _cacheService;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(
            ILogger<WeatherForecastController> logger,
            ICacheService cacheService)
        {
            _logger = logger;
            _cacheService = cacheService;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get(CancellationToken cancellationToken)
        {
            const string cacheKey = "ProductService:WeatherForecast";
            var cachedForecasts = await _cacheService.GetStringAsync(cacheKey, cancellationToken);

            if (!string.IsNullOrEmpty(cachedForecasts))
            {
                var fromCache = JsonSerializer.Deserialize<IEnumerable<WeatherForecast>>(cachedForecasts);
                if (fromCache is not null)
                {
                    return fromCache;
                }
            }

            var forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();

            await _cacheService.SetStringAsync(cacheKey, JsonSerializer.Serialize(forecasts), cancellationToken: cancellationToken);

            return forecasts;
        }
    }
}
