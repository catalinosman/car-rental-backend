using System.Text.Json.Serialization;

namespace CarRental.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public int VehicleId { get; set; }
        public User User { get; set; } = null!;
        [JsonIgnore]
        public Vehicle Vehicle { get; set; } = null!;
    }
}
