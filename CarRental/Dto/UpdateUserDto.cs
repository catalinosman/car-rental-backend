using System.ComponentModel.DataAnnotations;

namespace CarRental.Dto
{
    public class UpdateUserDto
    {
        [Required]
        [MinLength(1)]
        [MaxLength(50)]
        [RegularExpression(@"^[A-Z][a-z]*$", ErrorMessage = "Invalid first name.")]
        public string FirstName { get; set; } = null!;
        [Required]
        [MinLength(1)]
        [MaxLength(50)]
        [RegularExpression(@"^[A-Z][a-z]*$", ErrorMessage = "Invalid last name.")]

        public string LastName { get; set; } = null!;
        [Required]
        [MinLength(1)]
        [MaxLength(50)]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } = null!;
    }
}
