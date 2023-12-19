namespace AirportControl.Application.Dto;

public class CreateRouteRequest
{
    public int DepartureAirportID { get; set; }
    
    public int ArrivalAirportID { get; set; }
}