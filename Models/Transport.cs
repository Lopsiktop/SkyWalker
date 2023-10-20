namespace SkyWalker.Models;

public class Transport
{
    public int Id { get; set; }

    public User Owner { get; set; }

    public Station Station { get; set; }

    public string Identifier { get; set; }

    public string Model { get; set; }

    public string Color { get; set; }

    public decimal? HourPrice { get; set; }

    public decimal? DayPrice { get; set; }
}
