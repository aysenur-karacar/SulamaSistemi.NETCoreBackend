using Microsoft.EntityFrameworkCore;
using SulamaSistemiWebApi.Models;

namespace SulamaSistemiWebApi.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<TemperatureHumidity> TemperatureHumidities { get; set; }
    public DbSet<Motor> Motors { get; set; }
    public DbSet<RainSensor> RainSensors { get; set; }
    public DbSet<User> Users { get; set; }
}