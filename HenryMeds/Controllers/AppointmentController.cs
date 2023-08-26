using HenryMeds.DB;
using HenryMeds.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace HenryMeds.Controllers;

/// <summary>
///     Controller for managing appointments including creating, updating, and deleting appointments.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AppointmentController : ControllerBase
{
    private readonly ApiDbContext _context;

    public AppointmentController(ApiDbContext context)
    {
        _context = context;
    }

    /// <summary>
    ///     Retrieves all appointment slots that are reserved or on hold.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetExistingAppointments()
    {
        if (_context.Appointments == null)
        {
            return new List<AppointmentDto>();
        }

        var thirtyMinutesAgo = DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(30));

        return _context.Appointments.Where(a =>
                a.Status == AppointmentStatus.Confirmed ||
                (a.Status == AppointmentStatus.Requested &&
                 a.RequestExpires < thirtyMinutesAgo))
            .Select(a => new AppointmentDto
        {
            Id = a.Id,
            StartTime = a.Availabilities.First().Start,
            EndTime = a.Availabilities.First().Start,
            ProviderId = a.Provider.Id,
            ClientId = a.Client.Id,
            Status = a.Status,
        }).ToList();
    }

    /// <summary>
    /// Creates an appointment for a patient with a provider at a specific availablility for that provider.
    /// This appointment is pending until confirmation from the client is received.
    /// </summary>
    /// <response code="200">Appointment slot is available and pending confirmation from client.</response>
    /// <response code="400">Appointment slot is not available.</response>
    [HttpPost(Name = "Request Appointment")]
    public async Task<ActionResult> RequestAppointment(int availabilityId, int clientId, int providerId)
    {
        if (_context.Providers == null || _context.Clients == null || _context.Availabilities == null || _context.Appointments == null)
        {
            // Something has gone wrong with the db, we can't continue
            return StatusCode(500);
        }

        var provider = await _context.Providers.FindAsync(providerId);
        var client = await _context.Clients.FindAsync(clientId);
        var availability = await _context.Availabilities.FindAsync(availabilityId);
        if (provider == null || client == null || availability == null)
        {
            return BadRequest();
        }

        var appointment = new Appointment
        {
            Provider = provider,
            Client = client,
            Availabilities = new List<Availability> {availability},
            Status = AppointmentStatus.Requested,
            Created = DateTime.UtcNow,
            RequestExpires = DateTime.UtcNow.AddMinutes(30),
        };

        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();

        var appointmentResult = new AppointmentDto()
        {
            Id = appointment.Id,
            StartTime = appointment.Availabilities.First().Start,
            EndTime = appointment.Availabilities.First().End,
            ProviderId = appointment.Provider.Id,
            ClientId = appointment.Client.Id,
            Status = appointment.Status,
        };

        // return the provider as well as the name of the method to call to confirm the appointment
        return Ok(new {appointmentResult, confirmAppointment = nameof(ConfirmAppointment)});
    }

    /// <summary>
    /// Allows a client to confirm an appointment.
    /// </summary>
    /// <response code="200">Appointment confirmed.</response>
    /// <response code="409">Appointment already confirmed.</response>
    [HttpPut(Name = "Confirm Requested Appointment")]
    public async Task<ActionResult> ConfirmAppointment(int appointmentId)
    {
        var appointment = await _context.Appointments.FindAsync(appointmentId);
        if (appointment == null)
        {
            return NotFound();
        }

        if (appointment.Status == AppointmentStatus.Confirmed)
        {
            return Conflict();
        }

        // TODO: check if the status is pending and the time created is more than 30 minutes ago.
        // if so, return a 409 Conflict, they will need to re-request this appointment.
        // for now we can allow them to confirm the appointment if it not pending

        appointment.Status = AppointmentStatus.Confirmed;
        await _context.SaveChangesAsync();

        return Ok(appointment);
    }
}