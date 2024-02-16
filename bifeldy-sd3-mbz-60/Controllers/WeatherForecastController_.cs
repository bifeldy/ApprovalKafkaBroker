using Microsoft.AspNetCore.Mvc;

using bifeldy_sd3_mbz_60.Models;
using bifeldy_sd3_mbz_60.Repositories;

namespace bifeldy_sd3_mbz_60.Controllers {

    [ApiController]
    [Route("weather-forecast")]
    public sealed class WeatherForecastController : ControllerBase {

        private readonly IWeatherForecastRepository _wfRepo;

        public WeatherForecastController(IWeatherForecastRepository wfRepo) {
            _wfRepo = wfRepo;
        }

        [HttpGet]
        public async Task<ObjectResult> GetAll() {
            WeatherForecast[] wf = await _wfRepo.GetForecastAsync();
            return Ok(new {
                info = $"😅 200 - {GetType().Name} :: List All 🤣",
                results = wf
            });
        }

    }

}
