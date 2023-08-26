using HenryMeds.DB;
using HenryMeds.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace HenryMeds.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AvailabilityController : ControllerBase
{
    private readonly ApiDbContext _context;

    public AvailabilityController(ApiDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all available appointment slots that are not already reserved or on hold.
    /// /// TODO: add filtering by provider and date, and pagination or at least a limit.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AvailabilityDto>>> GetOpenAvailabilitiesSlots()
    {
        if (_context.Appointments == null || _context.Availabilities == null)
        {
            return new List<AvailabilityDto>();
        }

        var thirtyMinutesAgo = DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(30));
        var availabilities = _context.Availabilities
            .Where(a => !_context.Appointments
                .Any(appt => appt.Availabilities.Any(availability => availability.Id ==a.Id) &&
                             (appt.Status == AppointmentStatus.Confirmed ||
                              (appt.Status == AppointmentStatus.Requested && appt.RequestExpires < thirtyMinutesAgo))))
            .Select(a => new AvailabilityDto()
            {
                Id = a.Id,
                StartTime = a.Start,
                EndTime = a.End,
                ProviderId = a.Provider.Id,
            })
            .ToList();

        return availabilities;
    }

    /// <summary>
    /// Ger an availability by id.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<AvailabilityDto>> GetAvailability(int id)
    {
        var availability = await _context.Availabilities.FindAsync(id);

        if (availability == null)
        {
            return NotFound();
        }

        // return the availability dtp
        return new AvailabilityDto
        {
            Id = availability.Id,
            StartTime = availability.Start,
            EndTime = availability.End,
            ProviderId = availability.Provider.Id,
        };
    }

    /// <summary>
    ///     Create a new availability for a provider
    /// </summary>
    /// <remarks>
    ///     POST /Availability
    ///     {
    ///     "providerId": 1,
    ///     "startTime": "2021-10-01T09:00:00",
    ///     "endTime": "2021-10-01T17:00:00"
    ///     }
    /// </remarks>
    /// <returns></returns>
    /// <response code="200">Availability created</response>
    /// <response code="400">Invalid request. Check provider Id and start date is before end date</response>
    [HttpPost]
    public async Task<ActionResult> PostAvailability([FromBody] AvailabilityDto availabilityDto)
    {
        // create new availability
        var provider = await _context.Providers.FindAsync(availabilityDto.ProviderId);
        if (provider == null)
        {
            return NotFound();
        }

        if (availabilityDto.StartTime > availabilityDto.EndTime)
        {
            return BadRequest();
        }

        if (availabilityDto.StartTime.Minute % 15 != 0)
        {
            return BadRequest();
        }

        var currentSlotStart = availabilityDto.StartTime;
        var timeSlots = new List<Availability>();
        while (currentSlotStart < availabilityDto.EndTime)
        {
            timeSlots.Add(new Availability
            {
                Start = currentSlotStart,
                Provider = provider,
            });
            currentSlotStart = currentSlotStart.AddMinutes(15);
        }

        _context.Availabilities?.AddRange(timeSlots);
        await _context.SaveChangesAsync();

        return Ok();
    }
}