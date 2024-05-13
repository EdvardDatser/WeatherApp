using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WeatherApp
{
    public partial class MainPage : ContentPage
    {
        private readonly string _baseUrl = "https://api.open-meteo.com/v1/forecast";
        private static readonly HttpClient _client = new HttpClient();
        Label lbl = new Label();

        public MainPage()
        {
            InitializeComponent();
            // Call the method asynchronously to avoid blocking the UI thread
            LoadTemperature().ConfigureAwait(false);
        }

        private async Task LoadTemperature()
        {
            try
            {
                var latitude = 59.3853;
                var longitude = 28.1717;
                var requestUri = $"?latitude={latitude}&longitude={longitude}&current=temperature_2m&timezone=auto";

                var result = await _client.GetAsync(_baseUrl + requestUri).ConfigureAwait(false);
                result.EnsureSuccessStatusCode(); // Ensure the request was successful

                var json = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                var weatherData = JsonConvert.DeserializeObject<OpenMeteoResponse>(json);

                var temperature_2m = weatherData?.Current?.temperature_2m;

                var relative_humidity_2m = weatherData?.Current?.Humiditiy;

                WeatherLabel.Text = $"Temperature: {temperature_2m.Value} °C, Humidity: {relative_humidity_2m.Value}%";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                lbl.Text = "Error occurred while retrieving weather data.";
            }
            finally
            {
                Content = lbl;
            }
        }
    }

    // Define classes to represent the JSON response from Open-Meteo API
    public class OpenMeteoResponse
    {
        public CurrentData Current { get; set; }
    }

    public class CurrentData
    {
        public double? temperature_2m { get; set; }

        public double? Humiditiy { get; set; } //Vlaznost'
    }
}