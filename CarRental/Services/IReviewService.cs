using CarRental.Dto;
using CarRental.Models;

namespace CarRental.Services
{
    public interface IReviewService
    {
        Task<ICollection<Review>> GetReviews();
        Task<Review> GetReview(int id);
        Task<ICollection<Review>> GetReviewsByVehicleId(int id);
        Task<bool> ReviewExists(int id);
        Task<ReviewDto> AddNewReview(int userId, int vehicleId,string title, string description);
        Task<bool> DeleteReview(Review review);
    }
}
