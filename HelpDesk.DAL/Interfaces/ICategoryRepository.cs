using HelpDesk.DAL.Models;

namespace HelpDesk.DAL.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync(); //Get all categories

        Task<Category?> GetByIdAsync(int id); //Get one category

        Task AddAsync(Category category); //Insert category

        Task UpdateAsync(Category category); //Update category

        Task DeleteAsync(int id); //Delete category

        Task<IEnumerable<Category>> SearchAsync(string searchText); //Search categories
    }
}