using EarthQuake.Persistence.Models;
using EarthQuake.Persistence.Repository.Abstractions;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Timers;
using static System.Collections.Specialized.BitVector32;

namespace EarthQuakeDataAutomate;

public class DataAutomate
{
    private readonly ILogger<DataAutomate> _logger;
    private readonly System.Timers.Timer _timer;
    private readonly IConfiguration _configuration;
    private readonly IEarthquakeRepository _earthquakeRepository;
    private readonly IDataQueryRepository _dataQueryRepository;
    private readonly HttpClient _httpClient;

    public DataAutomate(IConfiguration configuration, HttpClient httpClient, ILogger<DataAutomate> logger, IEarthquakeRepository earthquakeRepository, IDataQueryRepository dataQueryRepository)
    {
        _earthquakeRepository = earthquakeRepository;
        _dataQueryRepository = dataQueryRepository;
        _configuration = configuration;
        _httpClient = httpClient;
        _logger = logger;
        _timer = new System.Timers.Timer(TimeSpan.FromMinutes(.3).TotalMilliseconds);
        _timer.Elapsed += OnTimedEvent;
        _timer.AutoReset = false;
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
        DateTime now = DateTime.Now;
        //RunQuakeCheckAndUpdate().GetAwaiter().GetResult();
        CreateWaveformData().GetAwaiter().GetResult();
    }

    private async Task RunQuakeCheckAndUpdate()
    {
        try
        {
            string startTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            string endTime = DateTime.Now.AddMinutes(1).ToString("yyyy-MM-ddTHH:mm:ss");

            string? urlTemplate = _configuration["DataAutomate:UpdateUrl"];

            if (string.IsNullOrEmpty(urlTemplate))
            {
                _logger.LogError("UpdateUrl is missing in configuration.");
                return;
            }

            string url = string.Format(urlTemplate, startTime, endTime);

            using var content = new StringContent(string.Empty);
            using HttpResponseMessage response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            string resultString = await response.Content.ReadAsStringAsync();

            //get the latitude and longitude and place and log those as well from the earthquake
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

    private async Task CreateWaveformData()
    {
        int? currentFeatureIdProcessing = null;

        try
        {
                // < 2.5        -- 0 - 6 miles
                // 2.5 - 3.9    -- 6 - 20 miles
                // 4.0 - 4.9    -- 30 - 80 miles
                // 5.0 - 5.9    -- 80 - 200miles
                // 6.0 - 6.9    -- 200 - 400 miles
                // 7.0 - 7.9    -- 400 - 800 miles
                // 8.0          -- 800 - 1000 miles

                //distance in miles
                Dictionary<double, int>? magnitudeDistance = new()
                {
                { 2.5, 6 },
                { 3.9, 20 },
                { 4.9, 80 },
                { 5.9, 200 },
                { 6.9, 400 },
                { 8, 800 }

            };

                List<int> eaarthquakeWaveformNeeded = await _dataQueryRepository.GetQuakesNeedingWaveformCheck();
                
                foreach (int featureId in eaarthquakeWaveformNeeded)
                {
                    currentFeatureIdProcessing = featureId;
                    Feature earthQuake = await _earthquakeRepository.GetEarthQuakeByFeatureId(featureId);

                    //a list of the max magnitudes assocaited with a distance
                    List<double> magnitudes = new List<double>(magnitudeDistance.Keys);

                    //get the first magnitude that is greater than the earthquake magnitude, this will give us the distance we need to search for stations
                    double keyForMiles = magnitudes.Where(n => n > earthQuake.properties.mag).OrderBy(n => n).FirstOrDefault();

                    //the furrthest we are looking for a station i terms of miles
                    int maxMilesToSearch = magnitudeDistance[keyForMiles];

                    string? urlTemplate = _configuration["DataAutomate:GetStationUrl"];

                    //stationcount needs to be sorted out
                    string url = string.Format(urlTemplate, 1, earthQuake.geometry.coordinates[1], earthQuake.geometry.coordinates[2], maxMilesToSearch);

                    using HttpResponseMessage response = await _httpClient.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    string resultContent = await response.Content.ReadAsStringAsync();

                    List<StationData>? stations = JsonSerializer.Deserialize<List<StationData>>(
                                        resultContent,
                                        new JsonSerializerOptions
                                        {
                                            PropertyNameCaseInsensitive = true
                                        }); //thes stations need to come from the get request

                    //filter out all the ones that their endtimes ended ebfore thsi earthquake started
                    stations = stations
                                .Where(station =>
                                    DateTime.TryParse(station.endtime, out DateTime endTime) &&
                                    endTime > DateTimeOffset
                                .FromUnixTimeMilliseconds((long)earthQuake.properties.time)
                                .UtcDateTime)
                                .ToList();

                    StationData? selectedStation = null;

                    foreach (StationData station in stations)
                    {
                        DateTime earthquakeTime = DateTimeOffset.FromUnixTimeMilliseconds((long)earthQuake.properties.time).UtcDateTime; //this is not the actual earthquake time, this is the time it happened at, epicenter, need to get time it reach the various stations
                        if (DateTime.Parse(station.starttime) <= earthquakeTime && DateTime.Parse(station.endtime) >= earthquakeTime)
                        {
                            //if no station added then add, if one there check THE distance and decide if you replace
                            if (selectedStation == null)
                            {
                                selectedStation = station;
                            }
                            else
                            {
                                //use the HaversineDistance -- if it is smaller than teh current selected station distance then we replace the selected station
                                double currentStationDistance = GetHaversineDistanceInMiles(earthQuake.geometry.coordinates[1], earthQuake.geometry.coordinates[2], selectedStation.latitude, selectedStation.longitude);
                                double newStationDistance = GetHaversineDistanceInMiles(earthQuake.geometry.coordinates[1], earthQuake.geometry.coordinates[2], station.latitude, station.longitude);   
                                if(currentStationDistance < newStationDistance)
                                {
                                    selectedStation = station;
                                }
                            }
                        }
                    }

                //remove this featureid from waveform needed list
                await _dataQueryRepository.RemoveQuakeFromWaveFormCheckList(featureId);
                currentFeatureIdProcessing = null;

                //if there is a selected station then place this selected station in a table associated with that quake
                if(selectedStation != null)
                {
                    await _dataQueryRepository.InsertStationWithFeatureWaveformData(featureId, selectedStation);
                    _logger.LogInformation("Station {0} selected for earthquake {1}", selectedStation.station, featureId);
                }
            }
        }
        catch(Exception ex)
        {
            if(currentFeatureIdProcessing != null)
            {
                //remove this featureid from waveform needed list
                await _dataQueryRepository.RemoveQuakeFromWaveFormCheckList((int)currentFeatureIdProcessing);
            }
            _logger.LogError(ex, "An unexpected error occurred while creating waveform data.");
        }
        finally
        {
            _timer.Start();
        }
        
    }

    public static double GetHaversineDistanceInMiles(double lat1, double lon1, double lat2, double lon2)
    {
        const double EarthRadiusMiles = 3958.8;

        // Convert degrees to radians
        double dLat = (lat2 - lat1) * (Math.PI / 180.0);
        double dLon = (lon2 - lon1) * (Math.PI / 180.0);

        double rLat1 = lat1 * (Math.PI / 180.0);
        double rLat2 = lat2 * (Math.PI / 180.0);

        // Haversine formula
        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Cos(rLat1) * Math.Cos(rLat2) *
                   Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));

        return EarthRadiusMiles * c;
    }


}
