namespace SkyWalker.Models;

public class Station
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Address { get; set; }

    public override string ToString()
    {
        return Name;
    }
}
