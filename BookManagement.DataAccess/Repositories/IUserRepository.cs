using BookManagement.BusinessObjects;

namespace BookManagement.DataAccess.Repositories;

public interface IUserRepository
{
	List<User> ListUsers();
	void AddUser(User user);
	void UpdateUser(User user);
	void DeleteUser(int id);
	User Login(string username);
}

