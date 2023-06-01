using CarRental.Dto;
using CarRental.Models;

namespace CarRental.Services
{
    public interface IVehicleService
    {
            Task<ICollection<Vehicle>> GetVehicles();
            Task<ICollection<Vehicle>> GetVehiclesByCategory(int id);
            Task<Vehicle> GetVehicle(int id);
            Task<bool> VehicleExists(int id);
            Task<bool> CreateVehicle(Vehicle vehicle, int id);
            Task<bool> UpdateVehicle(VehicleDto updatedVehicle, int vehicleId);
            Task<bool> DeleteVehicle(Vehicle vehicle);
        
    }
}
