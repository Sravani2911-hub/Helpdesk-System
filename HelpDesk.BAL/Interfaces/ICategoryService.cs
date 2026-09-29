using HelpDesk.DAL.Models;

namespace HelpDesk.BAL.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();

        Task<Category?> GetCategoryByIdAsync(int id);

        Task AddCategoryAsync(Category category);

        Task UpdateCategoryAsync(Category category);

        Task DeleteCategoryAsync(int id);

        Task<IEnumerable<Category>> SearchCategoriesAsync(string searchText);
    }
}