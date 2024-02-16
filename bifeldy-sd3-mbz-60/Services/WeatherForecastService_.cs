namespace bifeldy_sd3_mbz_60.Services {

    public interface IWeatherForecastService {
        string[] Summaries { get; }
    }

    public sealed class CWeatherForecastService : IWeatherForecastService {

        public string[] Summaries { get; } = new[] {
            "Freezing",
            "Bracing",
            "Chilly",
            "Cool",
            "Mild",
            "Warm",
            "Balmy",
            "Hot",
            "Sweltering",
            "Scorching"
        };

        public CWeatherForecastService() { }

    }

}
