using System.ComponentModel.DataAnnotations;

namespace SulamaSistemiWebApi.Models;

public class RainSensor
{
    [Key]
    public int Id { get; set; }
    public int RainLevel { get; set; } 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
} 