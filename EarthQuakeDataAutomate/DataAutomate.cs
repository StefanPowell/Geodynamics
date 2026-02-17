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
    private bool _hasRunToday;
    private DateTime _lastRunDate = DateTime.MinValue;

    public DataAutomate(IConfiguration configuration, HttpClient httpClient, ILogger<DataAutomate> logger)
    {
        _configuration = configuration;
        _httpClient = httpClient;
        _logger = logger;

        _timer = new System.Timers.Timer(TimeSpan.FromHours(1).TotalMilliseconds);
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

        // Reset flag if it's a new day
        if (_lastRunDate.Date != now.Date)
        {
            _hasRunToday = false;
        }

        // If it's 23:00 (11 PM) and hasn't run yet
        if (now.Hour == 23 && !_hasRunToday)
        {
            RunDataUpdate().GetAwaiter().GetResult();

            _hasRunToday = true;
            _lastRunDate = now.Date;
        }
    }

    private async Task RunDataUpdate()
    {
        try
        {
            string? url = _configuration["DataAutomate:UpdateUrl"];
            using StringContent? content = new StringContent(string.Empty);
            HttpResponseMessage? response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            _logger.LogInformation("Updating data at: " + DateTime.Now);
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex, ex.Message);
        }
    }

}
