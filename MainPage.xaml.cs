using Microsoft.Maui.Controls;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace WeatherApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnGetWeatherClicked(object sender, EventArgs e)
        {
            string location = LocationEntry.Text; // Get location from entry field

            string url = $"https://api.weatherapi.com/v1/current.json?key=aa7758b35f384a5eb62102337241405&q={location}&aqi=no";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();

                        JObject data = JObject.Parse(jsonResponse);

                        string locationName = data["location"]["name"].ToString();
                        string temperature = data["current"]["temp_c"].ToString();
                        string condition = data["current"]["condition"]["text"].ToString();
                        string wind_dir = data["current"]["wind_dir"].ToString();

                        WeatherLabel.Text = $"Weather in {locationName} ({location}):\n" +
                                            $"Temperature: {temperature}°C\n" +
                                            $"Condition: {condition}\n" +
                                            $"Wind direction: {wind_dir}\n";
                    }
                    else
                    {
                        WeatherLabel.Text = $"Failed to retrieve weather data for {location}. Status code: {response.StatusCode}";
                    }
                }
                catch (Exception ex)
                {
                    WeatherLabel.Text = $"An error occurred: {ex.Message}";
                }
            }
        }
    }
}