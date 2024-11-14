using System.Linq.Expressions;
using BookManagement.BusinessObjects;

namespace BookManagement.DataAccess.Repositories;

public interface IOrderRepository
{
	Task<Order>? GetOrderAsync(Expression<Func<Order, bool>>? predicate = null);
	List<Order> ListOrders();
	void AddOrder(Order order);
	void UpdateOrder(Order order);
	void DeleteOrder(int id);
	public Order? GetOrderById(int id);
}