using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookManagement.BusinessObjects;

namespace BookManagement.DataAccess.Repositories
{
    public interface IBookRepository
    {
        List<Book> GetListBooks();
        void AddBook(Book book);
        void UpdateBook(Book book);
        void DeleteBook(int id);
    }
}
