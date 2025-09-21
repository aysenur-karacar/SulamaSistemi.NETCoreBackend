namespace SulamaSistemiWebApi.DTOs;

public class SensorUpdateRequest
{
    public float temperature { get; set; }
    public float humidity { get; set; }
    public int rainLevel { get; set; }
} 