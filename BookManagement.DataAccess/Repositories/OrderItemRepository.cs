using System.Runtime.InteropServices.ComTypes;
using BookManagement.BusinessObjects;

namespace BookManagement.DataAccess.Repositories;

public class OrderItemRepository : IOrderItemRepository
{
	public List<OrderItem> ListOrderItems()
	{
		using  var db = new BookManagementDbContext();
		return db.OrderItems.ToList();
	}

	public void AddOrderItem(OrderItem orderItem)
	{
		Order o = new Order();
		using  var db = new BookManagementDbContext();
        var bookObj = db.Books.Find(orderItem.BookID);
        if(bookObj!=null)orderItem.Price = bookObj.Price * orderItem.Quantity;
        db.OrderItems.Add(orderItem);
		db.SaveChanges();
		UpdateTotalPrice(orderItem.OrderID);
	}

	public void UpdateOrderItem(OrderItem orderItem)
	{
		using  var db = new BookManagementDbContext();
		var existingOrderItem = db.OrderItems.FirstOrDefault(x => x.OrderID.Equals(orderItem.OrderID));
		var bookObj = db.Books.Find(orderItem.BookID);
		if (existingOrderItem != null && bookObj!=null)
		{
			//existingOrderItem.OrderID = orderItem.OrderID;
			//existingOrderItem.BookID = orderItem.BookID;
			existingOrderItem.Quantity = orderItem.Quantity;
			existingOrderItem.Price = bookObj.Price * orderItem.Quantity;
			db.SaveChanges();
			UpdateTotalPrice(orderItem.OrderID);
		}
		else
		{
			throw new Exception("OrderItem not found");
		}
	}

	public void DeleteOrderItem(OrderItem o)
	{
		using  var db = new BookManagementDbContext();
		var orderItem = db.OrderItems.FirstOrDefault(x => x.OrderID == o.OrderID && x.BookID == o.BookID);
		if (orderItem != null)
		{
			db.OrderItems.Remove(orderItem);
			db.SaveChanges();
			UpdateTotalPrice(orderItem.OrderID);
		}
		else
		{
			throw new Exception("OrderItem not found");
		}
	}

	public void UpdateTotalPrice(int orderId)
	{
		using  var db = new BookManagementDbContext();
		Order o = new Order();
		var order = db.Orders.FirstOrDefault(x => x.OrderID ==orderId);
		order.TotalPrice = o.CalTotalPrice();
		db.Orders.Update(order);
		db.SaveChanges();

	}
}