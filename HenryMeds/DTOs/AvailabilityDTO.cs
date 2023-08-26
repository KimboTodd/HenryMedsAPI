using System.ComponentModel.DataAnnotations;

namespace HenryMeds.DTOs;

public class AvailabilityDto
{
    public int? Id { get; set; }

    [Required]
    public DateTime StartTime { get; set; }

    [Required]
    public DateTime EndTime { get; set; }

    [Required]
    public int ProviderId { get; set; }
}