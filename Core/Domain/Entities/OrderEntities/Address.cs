
namespace Domain.Entities.OrderEntities
{
    public class Address
    {
        public Address() { }
        public Address(string firstName, string lastName, string street, string city, string country, string phoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            Street = street;
            City = city;
            Country = country;
            PhoneNumber = phoneNumber;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
    }
}
