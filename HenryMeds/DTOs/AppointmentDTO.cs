using Swashbuckle.AspNetCore.Annotations;

namespace HenryMeds.DTOs;

[SwaggerSchema(Description = "An appointment for a patient with a provider")]
public class AppointmentDto
{
    [SwaggerSchema(Description = "The unique identifier of the appointment")]
    public int Id { get; set; }

    [SwaggerSchema(Description = "The start time of the appointment")]
    public DateTime StartTime { get; set; }

    [SwaggerSchema(Description = "The end time of the appointment")]
    public DateTime EndTime { get; set; }

    [SwaggerSchema(Description = "The unique identifier of the provider")]
    public int ProviderId { get; set; }

    [SwaggerSchema(Description = "The unique identifier of the client")]
    public int ClientId { get; set; }

    [SwaggerSchema(Description = "The status of the appointment")]
    public AppointmentStatus Status { get; set; }
}