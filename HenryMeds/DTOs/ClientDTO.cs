using Swashbuckle.AspNetCore.Annotations;

namespace HenryMeds.DTOs
{
    [SwaggerSchema(Description = "A client of the HenryMeds system")]
    public class ClientDto
    {
        [SwaggerSchema(Description = "The unique identifier of the client")]
        public int Id { get; set; }
    }
}