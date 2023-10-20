using System;

namespace SkyWalker.Models;

public class Rent
{
    public int Id { get; set; }

    public User Renter { get; set; } = null!;

    public Transport Transport { get; set; } = null!;

    public RentType RentType { get; set; }

    public Status Status { get; set; }

    public decimal PriceOfUnit { get; set; }

    public decimal FinalPrice { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime EndedAt { get; set; }
}
