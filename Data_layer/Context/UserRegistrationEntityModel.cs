using System.ComponentModel.DataAnnotations;

namespace Data_layer.Context.Data
{
    public class UserRegistrationEntityModel
    {
        [Key]
        public Guid UserId { get; set; }

        public string? BedrijfsNaam { get; set; }

        public string? KvkNummer { get; set; }

       
        public string? BTW { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public bool IsApproved { get; set; }

        public Address Address { get; set; }

        public virtual ICollection<CartEnityModel> Carts { get; set; }

        public virtual ICollection<WishlistEntityModel> Wishlists { get; set; }

        public ICollection<Order> Orders { get; set; }

    }

    public class Address
    {
        [Key]
        public Guid AddressId { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string ZipCode { get; set; }

        [Required]
        public string Residence { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }
    }


}

