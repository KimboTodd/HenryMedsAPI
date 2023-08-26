using HenryMeds.DB;
using HenryMeds.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace HenryMeds.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientController : ControllerBase
{
    private readonly ApiDbContext _context;

    public ClientController(ApiDbContext context)
    {
        _context = context;
    }

    /// <summary>
    ///     Returns all clients.
    /// </summary>
    [HttpGet]
    public IEnumerable<ClientDto> Get()
    {
        if (_context.Clients == null)
        {
            return new List<ClientDto>();
        }
        return _context.Clients.Select(c => new ClientDto()
        {
            Id = c.Id,
        }).ToList();
    }

    /// <summary>
    /// For testing, allows creating new random clients.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ClientDto>> Create()
    {
        var client = new Client();
        _context.Clients.Add(client);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = client.Id }, client);
    }
}