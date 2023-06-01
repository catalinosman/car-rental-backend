namespace CarRental.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int VehicleId { get; set; }
        public double Price { get; set; }
        public DateTime Date { get; set; }
    }
}
