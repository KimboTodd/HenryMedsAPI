
namespace HenryMeds.DB;

/// <summary>
///     An appointment is one or more availabilities
///     that have been booked by a client with a specific provider.
/// </summary>
public class Appointment
{
    /// <summary>
    ///     Unique identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     A collection of one or more availabilities that have been booked.
    ///     Note: We'll probably stick with just one for now.
    /// </summary>
    public required ICollection<Availability> Availabilities { get; set; }

    /// <summary>
    ///     The client that booked the appointment.
    /// </summary>
    public required Client Client { get; set; }

    /// <summary>
    ///     The provider that the appointment is with.
    /// </summary>
    public required Provider Provider { get; set; }

    /// <summary>
    ///     A confirmation status.
    /// </summary>
    public AppointmentStatus Status { get; set; }

    /// <summary>
    /// Created date.
    /// </summary>
    public DateTime Created { get; set; }

    /// <summary>
    /// Date the appointment request expires.
    /// This will be used to filter out appointments that have been requested but not confirmed
    /// and that are available to be booked by other clients.
    /// </summary>
    public DateTime RequestExpires { get; set; }
}