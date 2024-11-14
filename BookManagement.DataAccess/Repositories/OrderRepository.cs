using BookManagement.BusinessObjects;
using Microsoft.EntityFrameworkCore;
using Util;

namespace BookManagement.DataAccess.Repositories;

public class OrderRepository : IOrderRepository
{
	public List<Order> ListOrders()
	{
		var db = new BookManagementDbContext();
		return db.Orders.Include(x=>x.OrderItems).ThenInclude(x=>x.Book).Include(x=>x.User).ToList();
	}

	public void AddOrder(Order order)
	{
		var db = new BookManagementDbContext();
		order.Status = OrderStatusConstant.Pending;
		order.OrderDate = DateTime.Now;
		order.TotalPrice = order.CalTotalPrice();
		db.Orders.Add(order);
	}

	public void UpdateOrder(Order order)
	{
		var db = new BookManagementDbContext();
		var existingOrder = db.Orders.FirstOrDefault(x => x.OrderID.Equals(order.OrderID));
		if (existingOrder != null)
		{
			//existingOrder.OrderID = order.OrderID;
			existingOrder.UserID = order.UserID;
			existingOrder.OrderDate = order.OrderDate;
			existingOrder.Status = order.Status;
			existingOrder.ShippingMethod = order.ShippingMethod;
			existingOrder.TotalPrice = order.TotalPrice;
			db.SaveChanges();
		}
		else
		{
			throw new Exception("Order not found");
		}
	}

	public void DeleteOrder(int id)
	{
		var db = new BookManagementDbContext();
		var order = db.Orders.FirstOrDefault(x => x.OrderID.Equals(id));
		if (order != null)
		{
			db.Orders.Remove(order);
			db.SaveChanges();
		}
		else
		{
			throw new Exception("Order not found");
		}
	}
    public Order? GetOrderById(int id)
    {
        var db = new BookManagementDbContext();
        return db.Orders.Include(x => x.OrderItems).ThenInclude(x => x.Book).Include(x => x.User)
			.FirstOrDefault(x => x.OrderID == id);
    }

}