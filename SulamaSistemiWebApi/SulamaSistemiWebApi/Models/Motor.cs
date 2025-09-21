using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SulamaSistemiWebApi.Models;

public class Motor
{
    [Key]
    public int Id { get; set; }
    public bool IsRunning { get; set; }
    public DateTime LastStatusChange { get; set; } = DateTime.UtcNow;
    public string? LastOperationBy { get; set; }
    
    // User ile ilişki
    public int? LastOperationByUserId { get; set; }
    [ForeignKey("LastOperationByUserId")]
    public User? LastOperationByUser { get; set; }
} 







