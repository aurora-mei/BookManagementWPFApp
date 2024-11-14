using BookManagement.BusinessObjects;

namespace BookManagement.DataAccess.Repositories;

public interface IOrderItemRepository
{
	List<OrderItem> ListOrderItems();
	void AddOrderItem(OrderItem orderItem);
	void UpdateOrderItem(OrderItem orderItem);
	void DeleteOrderItem(int id);
}