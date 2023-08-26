namespace HenryMeds.DB;

public class Provider
{
    public int Id { get; set; }

    public required ICollection<Availability> Availabilities { get; set; } = new List<Availability>();
}