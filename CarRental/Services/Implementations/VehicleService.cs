using CarRental.Data;
using CarRental.Dto;
using CarRental.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Services.Implementations
{
    public class VehicleService : IVehicleService
    {
        private readonly DataContext _context;

        public VehicleService(DataContext context)
        {
            _context = context;
        }


        public async Task<bool> CreateVehicle(Vehicle vehicle, int categoryId)
        {
            var category = await _context.Categories.Where(a => a.Id == categoryId).FirstOrDefaultAsync();

            var vehicleCategory = new VehicleCategory()
            {
                Category = category,
                Vehicle = vehicle,
            };

            await _context.AddAsync(vehicleCategory);

            await _context.AddAsync(vehicle);

            return await Save();
        }

        public async Task<bool> DeleteVehicle(Vehicle vehicle)
        {
            _context.Remove(vehicle);
            return await Save();
        }

        public async Task<Vehicle> GetVehicle(int id)
        {
            return await _context.Vehicles.Where(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<ICollection<Vehicle>> GetVehicles()
        {
            return await _context.Vehicles.OrderBy(c => c.Id).ToListAsync();
        }

        public async Task<ICollection<Vehicle>> GetVehiclesByCategory(int id)
        {
            return await _context.VehicleCategories.Where(c => c.CategoryId == id).Select(v => v.Vehicle).ToListAsync();
        }

        public async Task<bool> UpdateVehicle(VehicleDto updatedVehicle, int vehicleId)
        {
            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.Id == vehicleId);

            if (vehicle == null)
            {
                return false;
            }

            vehicle.Name = updatedVehicle.Name;
            vehicle.Description = updatedVehicle.Description;
            vehicle.Price = updatedVehicle.Price;
            vehicle.ImageURL = updatedVehicle.ImageURL;


            _context.Update(vehicle);
            await Save();
            return true;
        }

        public async Task<bool> VehicleExists(int id)
        {
            return await _context.Vehicles.AnyAsync(c => c.Id == id);
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
