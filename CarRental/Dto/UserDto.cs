using System.ComponentModel.DataAnnotations;

namespace CarRental.Dto
{
    public class UserDto
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

        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [MaxLength(50, ErrorMessage = "Password can't be more than 50 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,}$", ErrorMessage = "Password must have at least 1 lowercase letter, 1 uppercase letter, 1 symbol and 1 number")]
        public string Password { get; set; } = null!;

    }
}
