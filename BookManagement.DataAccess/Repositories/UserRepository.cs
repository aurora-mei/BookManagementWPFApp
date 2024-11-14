using BookManagement.BusinessObjects;

namespace BookManagement.DataAccess.Repositories;

public class UserRepository : IUserRepository
{
	public List<User> ListUsers()
	{
		var db = new BookManagementDbContext();
		return db.Users.ToList();
	}

	public void AddUser(User user)
	{
		var db = new BookManagementDbContext();
		db.Users.Add(user);
	}

	public void UpdateUser(User user)
	{
		var db = new BookManagementDbContext();
		var userToUpdate = db.Users.FirstOrDefault(u => u.UserID.Equals(user.UserID));
		if (userToUpdate != null)
		{
			//userToUpdate.UserID = user.UserID;
			userToUpdate.Username = user.Username;
			userToUpdate.Password = user.Password;
			userToUpdate.Dob = user.Dob;
			userToUpdate.Email = user.Email;
			userToUpdate.PhoneNumber = user.PhoneNumber;
			userToUpdate.Address = user.Address;
			userToUpdate.Role = user.Role;
			
			db.SaveChanges();
		}
		else
		{
			throw new Exception("User not found");
		}
	}

	public void DeleteUser(int id)
	{
		var db = new BookManagementDbContext();
		var user = db.Users.FirstOrDefault(u => u.UserID.Equals(id));
		if (user != null)
		{
			db.Users.Remove(user);
			db.SaveChanges();
		}
		else
		{
			throw new Exception("User not found");
		}
	}

	public User Login(string username)
	{
		var db = new BookManagementDbContext();
		return db.Users.FirstOrDefault(u => u.Username == username);
	}
}