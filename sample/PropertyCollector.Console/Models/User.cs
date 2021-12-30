namespace PropertyCollector.Console.Models;

public class User : EntityBase
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public bool Enabled { get; set; }
    public Gender Gender { get; set; }
    public short Weight { get; set; }
    public short Height { get; set; }
    public DateTime RegisterDate { get; set; }
    public List<Address> Addresses { get; set; } = new();
    public Address Address { get; set; } = new();//temp
    public List<User> Friends { get; set; } = new();
}