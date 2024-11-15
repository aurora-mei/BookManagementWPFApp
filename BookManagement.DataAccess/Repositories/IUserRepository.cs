using System.Linq.Expressions;
using BookManagement.BusinessObjects;

namespace BookManagement.DataAccess.Repositories;

public interface IUserRepository
{
	List<User> ListUsers();
	User GetUser(Expression<Func<User, bool>> predicate);
	void AddUser(User user);
	void UpdateUser(User user);
	void DeleteUser(int id);
	 public string Login(string email, string password);
	public User GetCustomerByEmail(string email);
}

