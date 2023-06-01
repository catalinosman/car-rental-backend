using CarRental.Models;

namespace CarRental.Services
{
    public interface ICategoryService
    {
        Task<ICollection<Category>>GetCategories();
        Task<Category>GetCategory(int id);
        Task<bool> CategoryExists(int id);
        Task<bool> CreateCategory(Category category);
        Task<bool> UpdateCategory(Category category);
        Task<bool> DeleteCategory(Category category);
    }
}

// interface = blueprint