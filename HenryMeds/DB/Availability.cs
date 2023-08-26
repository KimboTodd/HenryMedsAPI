using System.ComponentModel.DataAnnotations;

namespace HenryMeds.DB;

/// <summary>
///     AvailabilityController.cs entity that represents the availability of a provider
///     in small time slot increments of 15 minutes.
/// </summary>
public class Availability
{
    public Availability()
    {

    }
    /// <summary>
    ///     Unique identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     The start time for the availability.
    /// </summary>
    [Required]
    public DateTime Start { get; set; }

    /// <summary>
    ///     The end time for the availability.
    ///     This is calculated by adding 15 minutes to the start time.
    /// </summary>
    public DateTime End => Start.AddMinutes(15);

    /// <summary>
    /// The provider that the availability is for.
    /// </summary>
    public Provider? Provider { get; set; }
}