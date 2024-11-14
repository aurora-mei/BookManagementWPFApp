using BookManagement.BusinessObjects;

namespace BookManagement.DataAccess.Repositories;

public class CategoryRepository : ICatgoryRepository
{
	public List<Category> GetListCategories()
	{
		var db = new BookManagementDbContext();
		return db.Categories.ToList();
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