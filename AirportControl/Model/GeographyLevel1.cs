using System.ComponentModel.DataAnnotations;

namespace AirportControl.Application.Model;

public class GeographyLevel1
{
    [Key]
    public int GeographyLevel1ID { get; set; }
    
    public string Name { get; set; }
}