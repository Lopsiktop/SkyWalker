namespace SkyWalker.Models;

public class Station
{
    public int Id { get; set; }

    public string Name { get; set; }

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }

    public override string ToString()
    {
        return Name;
    }
}
