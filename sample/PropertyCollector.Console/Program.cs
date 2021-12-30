using System.Text.Json;
using Bogus;
using PropertyCollector;
using PropertyCollector.Console.Models;

var otherFaker = new Faker<Other>()
    .RuleFor(x => x.Id, f => f.Random.Int())
    .RuleFor(x => x.Name, f => f.Random.AlphaNumeric(10));

var addressFaker = new Faker<Address>()
    .RuleFor(a => a.Id, f => f.Random.Int())
    .RuleFor(a => a.BuildingNumber, f => f.Address.BuildingNumber())
    .RuleFor(a => a.City, f => f.Address.City())
    .RuleFor(a => a.Country, f => f.Address.Country())
    .RuleFor(a => a.State, f => f.Address.State())
    .RuleFor(a => a.Street, f => f.Address.StreetName())
    .RuleFor(a => a.ZipCode, f => f.Address.ZipCode())
    .RuleFor(a => a.Other, otherFaker);

var userFaker = new Faker<User>()
    .RuleFor(u => u.Id, f => f.Random.Int())
    .RuleFor(u => u.Name, f => f.Name.FirstName())
    .RuleFor(u => u.Surname, f => f.Name.LastName())
    .RuleFor(u => u.Enabled, f => f.Random.Bool())
    .RuleFor(u => u.Gender, f => f.PickRandom<Gender>())
    .RuleFor(u => u.Weight, f => f.Random.Short(0, 100))
    .RuleFor(u => u.Height, f => f.Random.Short(0, 200))
    .RuleFor(u => u.RegisterDate, f => f.Date.Past())
    .RuleFor(u => u.Address, addressFaker)
    .RuleFor(u => u.Addresses, () => addressFaker.Generate(3).ToList());

var user = userFaker.Generate();
user.Friends = userFaker.Generate(3);

var propertyCollector = new PropertyCollector<EntityBase>();
var collected = propertyCollector.Run(user);

//propertyCollector.PrintProperties(user);

//var options = new JsonSerializerOptions { WriteIndented = true };
//var jsonString = JsonSerializer.Serialize(user, options);

//Console.WriteLine(jsonString);

foreach (var property in collected)
{
    Console.WriteLine(property.Path);
}

Console.Read();