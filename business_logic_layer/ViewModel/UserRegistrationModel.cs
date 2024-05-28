using System;
using System.ComponentModel.DataAnnotations;

namespace business_logic_layer.ViewModel
{
    public class UserRegistrationModel
    {
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

        public Addres Address { get; set; }

        [Required]
        public bool IsApproved { get; set; }

    }

    public class Addres
    {
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



