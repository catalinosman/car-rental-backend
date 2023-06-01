using CarRental.Data;
using CarRental.Dto;
using CarRental.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CarRental.Services.Implementations
{
    public class ReviewService : IReviewService
    {
        private readonly DataContext _context;

        public ReviewService(DataContext context)
        {
            _context = context;
        }
       

        public async Task<ReviewDto> AddNewReview(int userId, int vehicleId, string title, string description)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            var newReview = new Review
            {
                VehicleId = vehicleId,
                UserId = userId,
                Title = title,
                Description = description
            };

            await _context.Reviews.AddAsync(newReview);
            await Save();

            // Get the vehicle from the database based on its ID
            var review = await _context.Reviews.FindAsync(newReview.Id);

            // Project the Review entity to the ReviewDto
            var reviewDto = new ReviewDto
            {
                Id = review.Id,
                Title = review.Title ?? "Unknown product",
                Description = review.Description ?? "No description"
            };


            return reviewDto;
        }

        public async Task<bool> DeleteReview(Review review)
        {
            _context.Remove(review);
            return await Save();
        }

        public async Task<Review> GetReview(int id)
        {
            return await _context.Reviews.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<ICollection<Review>> GetReviews()
        {
            return await _context.Reviews.OrderBy(r => r.Id).ToListAsync();
        }

        public async Task<ICollection<Review>> GetReviewsByVehicleId(int id)
        {
            // return await _context.Vehicles.Where(v => v.Id == id).Select(r => r.Reviews).ToListAsync();
            var vehicle = await _context.Vehicles.Include(v => v.Reviews).FirstOrDefaultAsync(v => v.Id == id);
            if (vehicle == null)
            {
                return null;
            }

            return vehicle.Reviews;
        }

        public async Task<bool> ReviewExists(int id)
        {
            return await _context.Reviews.AnyAsync(r => r.Id == id);
        }

        private async Task<bool> Save()
        {
            try
            {
                var saved = await _context.SaveChangesAsync();
                return saved > 0 ? true : false;
            }
            catch (Exception ex)
            {
                var message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                throw new Exception($"Error while saving the data: {message}");
            }
        }
    }
}
