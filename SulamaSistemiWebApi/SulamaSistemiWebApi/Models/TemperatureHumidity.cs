using System.ComponentModel.DataAnnotations;

namespace SulamaSistemiWebApi.Models;

public class TemperatureHumidity
{
    [Key]
    public int Id { get; set; }
    public float Temperature { get; set; }
    public float Humidity { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
} 