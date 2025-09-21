using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SulamaSistemiWebApi.Services;
using SulamaSistemiWebApi.Models;
using SulamaSistemiWebApi.DTOs;

namespace SulamaSistemiWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeviceController : ControllerBase
{
    private readonly DeviceService _service;

    public DeviceController(DeviceService service)
    {
        _service = service;
    }

    [HttpGet("temperature-humidity")]
    public async Task<ActionResult<TemperatureHumidity>> GetTemperatureHumidity()
    {
        return Ok(await _service.GetLatestTemperatureHumidityAsync());
    }

    [HttpGet("temperature-humidity/history")]
    public async Task<ActionResult<IEnumerable<TemperatureHumidity>>> GetTemperatureHumidityHistory(
        [FromQuery] DateTime? from = null,
        [FromQuery] DateTime? to = null,
        [FromQuery] int? limit = null)
    {
        return Ok(await _service.GetTemperatureHumidityHistoryAsync(from, to, limit));
    }

    [HttpGet("temperature-humidity/period/{period}")]
    public async Task<ActionResult<IEnumerable<TemperatureHumidity>>> GetTemperatureHumidityByPeriod(
        [FromRoute] string period)
    {
        return Ok(await _service.GetTemperatureHumidityByPeriodAsync(period));
    }

    [HttpGet("temperature-humidity/count/{count}")]
    public async Task<ActionResult<IEnumerable<TemperatureHumidity>>> GetTemperatureHumidityByCount(
        [FromRoute] int count)
    {
        if (count <= 0 || count > 1000)
            return BadRequest("Count must be between 1 and 1000");
            
        return Ok(await _service.GetTemperatureHumidityByCountAsync(count));
    }

    [HttpGet("motor")]
    public async Task<ActionResult<Motor>> GetMotorStatus()
    {
        return Ok(await _service.GetMotorStatusAsync());
    }

    [HttpGet("motor/history")]
    public async Task<ActionResult<IEnumerable<Motor>>> GetMotorHistory(
        [FromQuery] DateTime? from = null,
        [FromQuery] DateTime? to = null,
        [FromQuery] int? limit = null)
    {
        return Ok(await _service.GetMotorHistoryAsync(from, to, limit));
    }

    [HttpGet("motor/period/{period}")]
    public async Task<ActionResult<IEnumerable<Motor>>> GetMotorByPeriod(
        [FromRoute] string period)
    {
        return Ok(await _service.GetMotorByPeriodAsync(period));
    }

    [HttpGet("motor/count/{count}")]
    public async Task<ActionResult<IEnumerable<Motor>>> GetMotorByCount(
        [FromRoute] int count)
    {
        if (count <= 0 || count > 1000)
            return BadRequest("Count must be between 1 and 1000");
            
        return Ok(await _service.GetMotorByCountAsync(count));
    }

    [HttpPost("motor")]
    public async Task<IActionResult> SetMotorStatus([FromBody] bool state, [FromQuery] string operatedBy)
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        int? userId = null;
        
        if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int parsedUserId))
        {
            userId = parsedUserId;
        }
        
        await _service.SetMotorStatusAsync(state, operatedBy, userId);
        return NoContent();
    }

    [HttpGet("rain")]
    public async Task<ActionResult<RainSensor>> GetRainStatus()
    {
        return Ok(await _service.GetLatestRainStatusAsync());
    }

    [HttpGet("rain/history")]
    public async Task<ActionResult<IEnumerable<RainSensor>>> GetRainHistory(
        [FromQuery] DateTime? from = null,
        [FromQuery] DateTime? to = null,
        [FromQuery] int? limit = null)
    {
        return Ok(await _service.GetRainHistoryAsync(from, to, limit));
    }

    [HttpGet("rain/period/{period}")]
    public async Task<ActionResult<IEnumerable<RainSensor>>> GetRainByPeriod(
        [FromRoute] string period)
    {
        return Ok(await _service.GetRainByPeriodAsync(period));
    }

    [HttpGet("rain/count/{count}")]
    public async Task<ActionResult<IEnumerable<RainSensor>>> GetRainByCount(
        [FromRoute] int count)
    {
        if (count <= 0 || count > 1000)
            return BadRequest("Count must be between 1 and 1000");
            
        return Ok(await _service.GetRainByCountAsync(count));
    }

    [HttpPost("sensor-update")]
    public async Task<IActionResult> UpdateSensorValues(
        [FromBody] SensorUpdateRequest sensorData)
    {
        await _service.UpdateSensorValuesAsync(
            sensorData.temperature,
            sensorData.humidity,
            sensorData.rainLevel);
        return NoContent();
    }
}

