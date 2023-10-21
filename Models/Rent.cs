using System;

namespace SkyWalker.Models;

public class Rent
{
    public int Id { get; set; }

    public User Renter { get; set; } = null!;
    public int RenterId { get; set; }

    public Transport Transport { get; set; } = null!;
    public int TransportId { get; set; }

    public RentType RentType { get; set; }

    public Status Status { get; set; }

    public decimal PriceOfUnit { get; set; }

    public decimal FinalPrice { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime EndedAt { get; set; }
}
