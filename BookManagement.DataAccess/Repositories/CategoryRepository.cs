using BookManagement.BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.DataAccess.Repositories;

public class CategoryRepository : ICategoryRepository
{
	public List<Category> GetListCategories()
	{
		var db = new BookManagementDbContext();
		return db.Categories.Include(x=>x.Books).ThenInclude(x=>x.Author)
			.ToList();
	}
    public Category? GetCategoryById( int id)
    {
        var db = new BookManagementDbContext();
        return db.Categories.Include(x => x.Books).ThenInclude(x => x.Author)
            .FirstOrDefault(x=>x.CategoryID == id);
    }
    public void AddCategory(Category category)
	{
		var db = new BookManagementDbContext();
		db.Categories.Add(category);
	}

	public void UpdateCategory(Category category)
	{
		var db = new BookManagementDbContext();
		var categoryToUpdate = db.Categories.FirstOrDefault(c => c.CategoryID.Equals(category.CategoryID));
		if (categoryToUpdate != null)
		{
			categoryToUpdate.CategoryName = category.CategoryName;
			db.SaveChanges();
		}
		else
		{
			throw new Exception("Category not found");
		}
	}

	public void DeleteCategory(int id)
	{
		var db = new BookManagementDbContext();
		var categoryToDelete = db.Categories.FirstOrDefault(c => c.CategoryID.Equals(id));
		if (categoryToDelete != null)
		{
			db.Categories.Remove(categoryToDelete);
			db.SaveChanges();
		}
		else
		{
			throw new Exception("Category not found");
		}
	}
}