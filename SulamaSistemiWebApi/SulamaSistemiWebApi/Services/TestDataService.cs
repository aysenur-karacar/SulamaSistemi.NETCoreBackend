using SulamaSistemiWebApi.Data;
using SulamaSistemiWebApi.Models;

namespace SulamaSistemiWebApi.Services;

public class TestDataService
{
    private readonly ApplicationDbContext _context;

    public TestDataService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<string> CreateTestDataAsync()
    {
        try
        {
            // Yeni sıcaklık ve nem verisi ekle
            var random = new Random();
            var temperatureHumidity = new TemperatureHumidity
            {
                Temperature = 20 + random.Next(-5, 10) + (float)random.NextDouble(),
                Humidity = 40 + random.Next(-10, 20) + (float)random.NextDouble(),
                CreatedAt = DateTime.UtcNow
            };

            _context.TemperatureHumidities.Add(temperatureHumidity);

            // Yeni yağmur sensörü verisi ekle
            var rainSensor = new RainSensor
            {
                RainLevel = random.Next(0, 100),
                CreatedAt = DateTime.UtcNow
            };

            _context.RainSensors.Add(rainSensor);

            await _context.SaveChangesAsync();

            return $"Test verileri başarıyla oluşturuldu. Sıcaklık: {temperatureHumidity.Temperature:F1}°C, Nem: {temperatureHumidity.Humidity:F1}%, Yağmur Seviyesi: {rainSensor.RainLevel}%";
        }
        catch (Exception ex)
        {
            return $"Test verileri oluşturulurken hata: {ex.Message}";
        }
    }

    public async Task<string> CreateMultipleTestDataAsync(int count = 10)
    {
        try
        {
            var random = new Random();
            var temperatureHumidities = new List<TemperatureHumidity>();
            var rainSensors = new List<RainSensor>();

            for (int i = 0; i < count; i++)
            {
                // Sıcaklık ve nem verisi
                temperatureHumidities.Add(new TemperatureHumidity
                {
                    Temperature = 20 + random.Next(-5, 10) + (float)random.NextDouble(),
                    Humidity = 40 + random.Next(-10, 20) + (float)random.NextDouble(),
                    CreatedAt = DateTime.UtcNow.AddMinutes(-i * 5) // Her 5 dakikada bir
                });

                // Yağmur sensörü verisi
                rainSensors.Add(new RainSensor
                {
                    RainLevel = random.Next(0, 100),
                    CreatedAt = DateTime.UtcNow.AddMinutes(-i * 5)
                });
            }

            _context.TemperatureHumidities.AddRange(temperatureHumidities);
            _context.RainSensors.AddRange(rainSensors);

            await _context.SaveChangesAsync();

            return $"{count} adet test verisi başarıyla oluşturuldu.";
        }
        catch (Exception ex)
        {
            return $"Test verileri oluşturulurken hata: {ex.Message}";
        }
    }

    public async Task<string> ClearTestDataAsync()
    {
        try
        {
            var temperatureHumidities = _context.TemperatureHumidities.ToList();
            var rainSensors = _context.RainSensors.ToList();

            _context.TemperatureHumidities.RemoveRange(temperatureHumidities);
            _context.RainSensors.RemoveRange(rainSensors);

            await _context.SaveChangesAsync();

            return $"Test verileri temizlendi. {temperatureHumidities.Count} sıcaklık/nem ve {rainSensors.Count} yağmur sensörü verisi silindi.";
        }
        catch (Exception ex)
        {
            return $"Test verileri temizlenirken hata: {ex.Message}";
        }
    }
} 