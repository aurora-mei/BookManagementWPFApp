using BookManagement.BusinessObjects;

namespace BookManagement.DataAccess.Repositories;

public interface ICatgoryRepository
{
	List<Category> GetListCategories();
	void AddCategory(Category category);
	void UpdateCategory(Category category);
	void DeleteCategory(int id);
}