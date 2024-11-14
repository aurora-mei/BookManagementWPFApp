using BookManagement.BusinessObjects;

namespace BookManagement.DataAccess.Repositories;

public class OrderItemRepository : IOrderItemRepository
{
	public List<OrderItem> ListOrderItems()
	{
		var db = new BookManagementDbContext();
		return db.OrderItems.ToList();
	}

	public void AddOrderItem(OrderItem orderItem)
	{
		var db = new BookManagementDbContext();
		db.OrderItems.Add(orderItem);
	}

	public void UpdateOrderItem(OrderItem orderItem)
	{
		var db = new BookManagementDbContext();
		var existingOrderItem = db.OrderItems.FirstOrDefault(x => x.OrderID.Equals(orderItem.OrderID));
		if (existingOrderItem != null)
		{
			//existingOrderItem.OrderID = orderItem.OrderID;
			//existingOrderItem.BookID = orderItem.BookID;
			existingOrderItem.Quantity = orderItem.Quantity;
			existingOrderItem.Price = orderItem.Price;
			
			db.SaveChanges();
		}
		else
		{
			throw new Exception("OrderItem not found");
		}
	}

	public void DeleteOrderItem(int id)
	{
		var db = new BookManagementDbContext();
		var orderItem = db.OrderItems.FirstOrDefault(x => x.OrderID.Equals(id));
		if (orderItem != null)
		{
			db.OrderItems.Remove(orderItem);
			db.SaveChanges();
		}
		else
		{
			throw new Exception("OrderItem not found");
		}
	}
}