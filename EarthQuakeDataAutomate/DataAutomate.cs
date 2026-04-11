using System;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Timers;
using Microsoft.Extensions.Configuration;

namespace EarthQuakeDataAutomate;

public class DataAutomate
{
    private readonly ILogger<DataAutomate> _logger;
    private readonly System.Timers.Timer _timer;
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public DataAutomate(IConfiguration configuration, HttpClient httpClient, ILogger<DataAutomate> logger)
    {
        _configuration = configuration;
        _httpClient = httpClient;
        _logger = logger;
        _timer = new System.Timers.Timer(TimeSpan.FromMinutes(1).TotalMilliseconds);
        _timer.Elapsed += OnTimedEvent;
        _timer.AutoReset = true;
    }


    public void Start()
    {
        _timer.Start();
    }

    public void Stop()
    {
        _timer.Stop();
    }

    private void OnTimedEvent(object? sender, ElapsedEventArgs e)
    {
        var now = DateTime.Now;
        _logger.LogInformation("Timer Triggered");
        RunDataUpdate().GetAwaiter().GetResult();

    }

    private async Task RunDataUpdate()
    {
        try
        {
            // 1. Format dates for the URL
            string startTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            string endTime = DateTime.Now.AddMinutes(1).ToString("yyyy-MM-ddTHH:mm:ss");

            // 2. Safely get the template from configuration
            string? urlTemplate = _configuration["DataAutomate:UpdateUrl"];

            if (string.IsNullOrEmpty(urlTemplate))
            {
                _logger.LogError("UpdateUrl is missing in configuration.");
                return;
            }

            // 3. Construct the full URL
            string url = string.Format(urlTemplate, startTime, endTime);

            // 4. Send the POST request
            using var content = new StringContent(string.Empty);
            using HttpResponseMessage response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            string resultString = await response.Content.ReadAsStringAsync();

            _logger.LogInformation("{0} Earthquake found in : {1} - {2}",resultString ,startTime, endTime);

        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "API request failed.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred.");
        }

    }

}
