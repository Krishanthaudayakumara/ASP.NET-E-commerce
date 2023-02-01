using System.ComponentModel.DataAnnotations;

namespace OZQ_cart.DTOs
{
    public class CustomerToInsertDto
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(255, ErrorMessage = "Name cannot be longer than 255 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }

        [StringLength(255, ErrorMessage = "Address cannot be longer than 255 characters.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "User Id is required.")]
        public string UserId { get; set; }
    }

}
