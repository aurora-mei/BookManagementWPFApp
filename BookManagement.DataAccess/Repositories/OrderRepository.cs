using System.Linq.Expressions;
using BookManagement.BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.DataAccess.Repositories;

public class OrderRepository : IOrderRepository
{
    public async Task<Order>? GetOrderAsync(Expression<Func<Order, bool>>? predicate = null)
    {
        await using var db = new BookManagementDbContext();
        return await db.Orders
            .Where(predicate == null ? null : predicate)
            .Include(o => o.OrderItems)
            .ThenInclude(o => o.Book) // Include the Book
            .ThenInclude(b => b.Author) // Include the Author from Book
            .Include(o => o.OrderItems)
            .ThenInclude(o => o.Book) // Include the Book again
            .ThenInclude(b => b.Category) // Include the Category from Book
            .Include(o => o.OrderItems)
            .ThenInclude(o => o.Book) // Include the Book
            .ThenInclude(b => b.Discount)
            .Include(o => o.User)
            .FirstOrDefaultAsync();
    }

    public List<Order> ListOrders()
    {
        var db = new BookManagementDbContext();
        return db.Orders.ToList();
    }

    public void AddOrder(Order order)
    {
        var db = new BookManagementDbContext();
        db.Orders.Add(order);
        db.SaveChanges();
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
            existingOrder.TotalPrice = order.TotalPrice;
            existingOrder.ShippingMethod = order.ShippingMethod;
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
}