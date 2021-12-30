namespace PropertyCollector.Console.Models;

public class Address : EntityBase
{
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? ZipCode { get; set; }
    public string? BuildingNumber { get; set; }
    public Other? Other { get; set; }
}