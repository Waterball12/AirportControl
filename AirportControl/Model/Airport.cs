
using System.ComponentModel.DataAnnotations;

public class Airport
{
    [Key]
    public int AirportID { get; set; }
    
    public string IATACpde { get; set; }
    
    public int GeographyLevel1ID { get; set; }
    
    public string Type { get; set; }
}