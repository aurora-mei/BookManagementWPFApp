using System.Linq.Expressions;
using BookManagement.BusinessObjects;
using Util;

namespace BookManagement.DataAccess.Repositories;

public class UserRepository : IUserRepository
{
	public List<User> ListUsers()
	{
		using var db = new BookManagementDbContext();
		return db.Users.ToList();
	}

	public User GetUser(Expression<Func<User, bool>> predicate)
	{
		using var db = new BookManagementDbContext();
		return db.Users.FirstOrDefault(predicate);
	}

	public void AddUser(User user)
	{
		 using var db = new BookManagementDbContext();
		user.UserStatus = UserStatusConstant.Active;
		user.Role = RoleConstant.User;
		db.Users.Add(user);
		db.SaveChanges();
	}

	public void UpdateUser(User user)
	{
		using var db = new BookManagementDbContext();
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
		using var db = new BookManagementDbContext();
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

	public string Login(string email, string password)
	{
		if (AuthenticateAdmin(email, password))
		{
			return "Admin";
		}

		var resAuthenticateCustomer = AuthenticateCustomer(email, password);
		if (resAuthenticateCustomer == 0)
		{
			return "Invalid";
		}
		else if (resAuthenticateCustomer == -1)
		{
			return "Banned";
		}
		return resAuthenticateCustomer.ToString();

	}
	public bool AuthenticateAdmin(string email, string password)
	{
        var user = GetCustomerByEmail(email);

        if (user != null)
        {
           return user.Role.Equals(RoleConstant.Admin) && user.Password.Equals(password);
        }
		return false;
    }

	public int AuthenticateCustomer(string email, string password)
	{
		var user = GetCustomerByEmail(email);
		if (user != null)
		{
			if (user.UserStatus == UserStatusConstant.Inactive) return -1;
			if (user.Password.Equals(password)) return user.UserID;
		}
		return 0;
	}

	public User GetCustomerByEmail(string email)
	{
		using var db = new BookManagementDbContext();
		return db.Users.FirstOrDefault(u => u.Email.Equals(email));
	}
}

