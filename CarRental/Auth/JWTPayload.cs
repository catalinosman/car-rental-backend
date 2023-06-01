namespace CarRental.Auth
{
    public class JWTPayload
    {
        public string Id { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string UserRole { get; set; } = null!;
  
    }
}


