using System.Text.Json.Serialization;

namespace CarRental.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double Price { get; set; }
        public string ImageURL { get; set; } = null!;
        public ICollection<Review> Reviews { get; set; } = null!;
        public int ReviewId { get; set; }
        public ICollection<VehicleCategory> VehicleCategories { get; set; } = null!;

    }
}
