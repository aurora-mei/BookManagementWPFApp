using BookManagement.BusinessObjects;

namespace BookManagement.DataAccess.Repositories;

public interface IOrderRepository
{
	List<Order> ListOrders();
	void AddOrder(Order order);
	void UpdateOrder(Order order);
	void DeleteOrder(int id);
}