using Microsoft.EntityFrameworkCore;
using SulamaSistemiWebApi.Data;
using SulamaSistemiWebApi.Models;
using SulamaSistemiWebApi.Interfaces;

namespace SulamaSistemiWebApi.Services;

public class DeviceService
{
    private readonly ApplicationDbContext _context;

    public DeviceService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TemperatureHumidity> GetLatestTemperatureHumidityAsync()
    {
        return await _context.TemperatureHumidities
            .OrderByDescending(th => th.CreatedAt)
            .FirstOrDefaultAsync() ?? new TemperatureHumidity();
    }

    public async Task<IEnumerable<TemperatureHumidity>> GetTemperatureHumidityHistoryAsync(
        DateTime? from = null,
        DateTime? to = null,
        int? limit = null)
    {
        var query = _context.TemperatureHumidities.AsQueryable();

        if (from.HasValue)
            query = query.Where(th => th.CreatedAt >= from.Value);

        if (to.HasValue)
            query = query.Where(th => th.CreatedAt <= to.Value);

        query = query.OrderByDescending(th => th.CreatedAt);

        if (limit.HasValue)
            query = query.Take(limit.Value);
        else
            query = query.Take(100);

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<TemperatureHumidity>> GetTemperatureHumidityByPeriodAsync(string period)
    {
        var query = _context.TemperatureHumidities.AsQueryable();
        var now = DateTime.UtcNow;

        switch (period.ToLower())
        {
            case "1h":
            case "1hour":
                query = query.Where(th => th.CreatedAt >= now.AddHours(-1));
                break;
            case "6h":
            case "6hours":
                query = query.Where(th => th.CreatedAt >= now.AddHours(-6));
                break;
            case "12h":
            case "12hours":
                query = query.Where(th => th.CreatedAt >= now.AddHours(-12));
                break;
            case "1d":
            case "1day":
                query = query.Where(th => th.CreatedAt >= now.AddDays(-1));
                break;
            case "3d":
            case "3days":
                query = query.Where(th => th.CreatedAt >= now.AddDays(-3));
                break;
            case "1w":
            case "1week":
                query = query.Where(th => th.CreatedAt >= now.AddDays(-7));
                break;
            case "1m":
            case "1month":
                query = query.Where(th => th.CreatedAt >= now.AddMonths(-1));
                break;
            default:
                query = query.Where(th => th.CreatedAt >= now.AddDays(-1)); // Varsayılan 1 gün
                break;
        }

        return await query
            .OrderByDescending(th => th.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<TemperatureHumidity>> GetTemperatureHumidityByCountAsync(int count)
    {
        return await _context.TemperatureHumidities
            .OrderByDescending(th => th.CreatedAt)
            .Take(count)
            .ToListAsync();
    }

    public async Task<Motor> GetMotorStatusAsync()
    {
        return await _context.Motors
            .Include(m => m.LastOperationByUser)
            .OrderByDescending(m => m.LastStatusChange)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Motor>> GetMotorHistoryAsync(
        DateTime? from = null,
        DateTime? to = null,
        int? limit = null)
    {
        var query = _context.Motors
            .Include(m => m.LastOperationByUser)
            .AsQueryable();

        if (from.HasValue)
            query = query.Where(m => m.LastStatusChange >= from.Value);

        if (to.HasValue)
            query = query.Where(m => m.LastStatusChange <= to.Value);

        query = query.OrderByDescending(m => m.LastStatusChange);

        if (limit.HasValue)
            query = query.Take(limit.Value);
        else
            query = query.Take(100);

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Motor>> GetMotorByPeriodAsync(string period)
    {
        var query = _context.Motors
            .Include(m => m.LastOperationByUser)
            .AsQueryable();
        var now = DateTime.UtcNow;

        switch (period.ToLower())
        {
            case "1h":
            case "1hour":
                query = query.Where(m => m.LastStatusChange >= now.AddHours(-1));
                break;
            case "6h":
            case "6hours":
                query = query.Where(m => m.LastStatusChange >= now.AddHours(-6));
                break;
            case "12h":
            case "12hours":
                query = query.Where(m => m.LastStatusChange >= now.AddHours(-12));
                break;
            case "1d":
            case "1day":
                query = query.Where(m => m.LastStatusChange >= now.AddDays(-1));
                break;
            case "3d":
            case "3days":
                query = query.Where(m => m.LastStatusChange >= now.AddDays(-3));
                break;
            case "1w":
            case "1week":
                query = query.Where(m => m.LastStatusChange >= now.AddDays(-7));
                break;
            case "1m":
            case "1month":
                query = query.Where(m => m.LastStatusChange >= now.AddMonths(-1));
                break;
            default:
                query = query.Where(m => m.LastStatusChange >= now.AddDays(-1)); // Varsayılan 1 gün
                break;
        }

        return await query
            .OrderByDescending(m => m.LastStatusChange)
            .ToListAsync();
    }

    public async Task<IEnumerable<Motor>> GetMotorByCountAsync(int count)
    {
        return await _context.Motors
            .Include(m => m.LastOperationByUser)
            .OrderByDescending(m => m.LastStatusChange)
            .Take(count)
            .ToListAsync();
    }

    public async Task SetMotorStatusAsync(bool state, string operatedBy, int? userId = null)
    {
        var motor = new Motor
        {
            IsRunning = state,
            LastOperationBy = operatedBy,
            LastOperationByUserId = userId
        };

        _context.Motors.Add(motor);
        await _context.SaveChangesAsync();
    }

    public async Task<RainSensor> GetLatestRainStatusAsync()
    {
        return await _context.RainSensors
            .OrderByDescending(r => r.CreatedAt)
            .FirstOrDefaultAsync() ?? new RainSensor();
    }

    public async Task<IEnumerable<RainSensor>> GetRainHistoryAsync(
        DateTime? from = null,
        DateTime? to = null,
        int? limit = null)
    {
        var query = _context.RainSensors.AsQueryable();

        if (from.HasValue)
            query = query.Where(r => r.CreatedAt >= from.Value);

        if (to.HasValue)
            query = query.Where(r => r.CreatedAt <= to.Value);

        query = query.OrderByDescending(r => r.CreatedAt);

        if (limit.HasValue)
            query = query.Take(limit.Value);
        else
            query = query.Take(100);

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<RainSensor>> GetRainByPeriodAsync(string period)
    {
        var query = _context.RainSensors.AsQueryable();
        var now = DateTime.UtcNow;

        switch (period.ToLower())
        {
            case "1h":
            case "1hour":
                query = query.Where(r => r.CreatedAt >= now.AddHours(-1));
                break;
            case "6h":
            case "6hours":
                query = query.Where(r => r.CreatedAt >= now.AddHours(-6));
                break;
            case "12h":
            case "12hours":
                query = query.Where(r => r.CreatedAt >= now.AddHours(-12));
                break;
            case "1d":
            case "1day":
                query = query.Where(r => r.CreatedAt >= now.AddDays(-1));
                break;
            case "3d":
            case "3days":
                query = query.Where(r => r.CreatedAt >= now.AddDays(-3));
                break;
            case "1w":
            case "1week":
                query = query.Where(r => r.CreatedAt >= now.AddDays(-7));
                break;
            case "1m":
            case "1month":
                query = query.Where(r => r.CreatedAt >= now.AddMonths(-1));
                break;
            default:
                query = query.Where(r => r.CreatedAt >= now.AddDays(-1)); // Varsayılan 1 gün
                break;
        }

        return await query
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<RainSensor>> GetRainByCountAsync(int count)
    {
        return await _context.RainSensors
            .OrderByDescending(r => r.CreatedAt)
            .Take(count)
            .ToListAsync();
    }

    public async Task UpdateSensorValuesAsync(float temperature, float humidity, int rainLevel)
    {
        // Sıcaklık ve nem kaydı
        var tempHumidity = new TemperatureHumidity
        {
            Temperature = temperature,
            Humidity = humidity
        };
        _context.TemperatureHumidities.Add(tempHumidity);

        // Yağmur sensörü kaydı
        var rainSensor = new RainSensor
        {
            RainLevel = rainLevel
        };
        _context.RainSensors.Add(rainSensor);

        await _context.SaveChangesAsync();
    }
} 