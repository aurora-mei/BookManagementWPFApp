using BookManagement.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.DataAccess.Repositories
{
    public interface ICategoryRepository
    {
        List<Category> GetListCategories();
        void AddCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(int id);
        public Category? GetCategoryById(int id);
    }
}
