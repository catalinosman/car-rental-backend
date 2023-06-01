namespace CarRental.Dto
{
    public class VehicleDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double Price { get; set; }
        public string ImageURL { get; set; } = null!;
    }
}
