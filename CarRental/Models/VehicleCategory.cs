namespace CarRental.Models
{
    public class VehicleCategory
    {
        public int VehicleId { get; set; }
        public int CategoryId { get; set; }
        public Vehicle Vehicle { get; set; } = null!;
        public Category Category { get; set; } = null!;  
    }
}
