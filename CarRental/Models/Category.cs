namespace CarRental.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<VehicleCategory> VehicleCategories { get; set; } = null!;
    }
}
