using BookManagement.BusinessObjects;

namespace BookManagement.DataAccess.Repositories;

public class DiscountRepository : IDiscountRepository
{
	public List<Discount> GetListDiscounts()
	{
		using  var db = new BookManagementDbContext();
		return db.Discounts.ToList();
	}

	public void AddDiscount(Discount discount)
	{
		using  var db = new BookManagementDbContext();
		db.Discounts.Add(discount);
		db.SaveChanges();
	}

	public void UpdateDiscount(Discount discount)
	{
		using  var db = new BookManagementDbContext();
		var discountToUpdate = db.Discounts.FirstOrDefault(d => d.DiscountID.Equals(discount.DiscountID));
		if (discountToUpdate != null)
		{
			//discountToUpdate.DiscountID = discount.DiscountID;
			discountToUpdate.discountName = discount.discountName;
			discountToUpdate.discountValue = discount.discountValue;
			db.SaveChanges();
		}
		else
		{
			throw new Exception("Discount not found");
		}
	}

	public void DeleteDiscount(int id)
	{
		using  var db = new BookManagementDbContext();
		var discountToDelete = db.Discounts.FirstOrDefault(d => d.DiscountID.Equals(id));
		if (discountToDelete != null)
		{
			db.Discounts.Remove(discountToDelete);
			db.SaveChanges();
		}
		else
		{
			throw new Exception("Discount not found");
		}
	}
}