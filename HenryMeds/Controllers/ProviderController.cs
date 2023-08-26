using HenryMeds.DB;
using Microsoft.AspNetCore.Mvc;

namespace HenryMeds.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProviderController : Controller
{
    private readonly ApiDbContext _context;

    public ProviderController(ApiDbContext context)
    {
        _context = context;
    }

    /// <summary>
    ///     Returns all providers.
    /// </summary>
    [HttpGet]
    public IEnumerable<Provider> Get()
    {
        var providers = _context.Providers.ToList();
        return providers;
    }

    /// <summary>
    /// Create a random test provider
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Provider>> CreateProvider()
    {
        var provider = new Provider {Availabilities = new List<Availability>()};
        _context.Providers.Add(provider);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = provider.Id }, provider);
    }
}