using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirportControl.Application.Model;

[Table("Route")]
public class AirportRoute
{
    [Key]
    public int RouteId { get; set; }
    
    public int DepartureAirportID { get; set; }
    
    public int ArrivalAirportID { get; set; }
}