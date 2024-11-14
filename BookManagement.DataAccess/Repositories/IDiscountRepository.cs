using BookManagement.BusinessObjects;

namespace BookManagement.DataAccess.Repositories;

public interface IDiscountRepository
{
	List<Discount> GetListDiscounts();
	void AddDiscount(Discount discount);
	void UpdateDiscount(Discount discount);
	void DeleteDiscount(int id);
}