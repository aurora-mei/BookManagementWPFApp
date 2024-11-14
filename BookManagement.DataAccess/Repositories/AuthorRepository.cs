using BookManagement.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.DataAccess.Repositories
{
    internal class AuthorRepository : IAuthorRepository
    {
        public void AddAuthor(Author author)
        {
            var db = new BookManagementDbContext();
            db.Authors.Add(author);
        }

        public void DeleteAuthor(int id)
        {
            var db = new BookManagementDbContext();
            var author = db.Authors.Find(id);
            if (author != null)
            {
                db.Authors.Remove(author);
                db.SaveChanges();
            }
            else
            {
                throw new Exception("Author not found");
            }
        }

        public List<Author> GetListAuthors()
        {
            var db = new BookManagementDbContext();
            return db.Authors.ToList();
        }

        public void UpdateAuthor(Author author)
        {
            var db = new BookManagementDbContext();
            var authorToUpdate = db.Authors.FirstOrDefault(x => x.AuthorID.Equals(author.AuthorID));
            if (authorToUpdate != null)
            {
                authorToUpdate.AuthorName = author.AuthorName;
                authorToUpdate.AuthorDOB = author.AuthorDOB;
                authorToUpdate.AuthorEmail = author.AuthorEmail;
                authorToUpdate.AuthorImageURL = author.AuthorImageURL;
                db.SaveChanges();
            }
            else
            {
                throw new Exception("Author not found");
            }
        }
    }
}
