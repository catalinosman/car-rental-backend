using AutoMapper;
using CarRental.Dto;
using CarRental.Models;
using CarRental.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{
    public class ReviewController : ApiBaseController
    {
        private readonly IReviewService _reviewService;
        private readonly IMapper _mapper;
        private readonly IVehicleService _vehicleService;
        private readonly IUserService _userService;

        public ReviewController(IReviewService reviewService, IMapper mapper, IVehicleService vehicleService, IUserService userService)
        {
            _reviewService = reviewService;
            _mapper = mapper;
            _vehicleService = vehicleService;
            _userService = userService;
        }


        [HttpGet]
        public async Task<IActionResult> GetReviewsByVehicle(int vehicleId)
        {
            var vehicle = await _vehicleService.GetVehicle(vehicleId);
            if (vehicle == null)
            {
                return NotFound("Vehicle not found");
            }

            var reviews = await _reviewService.GetReviewsByVehicleId(vehicleId);
            if (reviews == null)
            {
                return NotFound("No reviews found");
            }

            return Ok(reviews);
        }

        [HttpPost("{userId}/{vehicleId}"), Authorize]
        public async Task<IActionResult> AddNewReview(ReviewDto reviewCreate, int userId, int vehicleId)
        {
            var vehicle = await _vehicleService.GetVehicle(vehicleId);
              if (vehicle == null)
              {
                  return NotFound("Vehicle not found");
              }

              var user = await _userService.GetUser(userId);
              if (user == null)
              {
                  return NotFound("User not found");
              } 

            var reviews = await _reviewService.GetReviews();
            var review = reviews.
                Where(r => r.Title.Trim().ToUpper() == reviewCreate.Title.TrimEnd().ToUpper()).
                FirstOrDefault();

            if (review != null)
            {
                ModelState.AddModelError("", "Review already exists");
                return StatusCode(422, ModelState);
            }

            if (string.IsNullOrWhiteSpace(reviewCreate.Title))
            {
                ModelState.AddModelError("", "Title cannot be null or empty");
                return StatusCode(422, ModelState);
            }

            if (string.IsNullOrWhiteSpace(reviewCreate.Description))
            {
                ModelState.AddModelError("", "Description cannot be null or empty");
                return StatusCode(422, ModelState);
            }


            var created = await _reviewService.AddNewReview(userId, vehicleId, reviewCreate.Title, reviewCreate.Description);
            return Ok(created);

        }

        [HttpDelete("{reviewId}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteReview(int reviewId)
        {
            if(!await _reviewService.ReviewExists(reviewId))
            {
                return NotFound();
            }

            var reviewDelete = await _reviewService.GetReview(reviewId);

            if(!await _reviewService.DeleteReview(reviewDelete))
            {
                ModelState.AddModelError("", "Something went wrong");
            }
            return Ok("Success");
        }
    }
}
