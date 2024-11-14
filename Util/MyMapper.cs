using BookManagement.BusinessObjects;
using BookManagement.BusinessObjects.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util
{
    public class MyMapper:IMyMapper
    {
        public void Map<T1, T2>(T1 src, T2 des)
        {
            var srcProps = typeof(T1).GetProperties();
            var desProps = typeof(T2).GetProperties();
            foreach (var srcProp in srcProps)
            {
                // Find a matching property in the destination by name and type
                var desProp = desProps
                    .FirstOrDefault(dp => dp.Name == srcProp.Name && dp.PropertyType == srcProp.PropertyType);

                if (desProp != null && desProp.CanWrite)
                {
                    desProp.SetValue(des, srcProp.GetValue(src));
                }
            }

            if (typeof(T1) == typeof(Book) && typeof(T2) == typeof(BookVM))
            {
                var book = src as Book;
                var bookVm = des as BookVM;

                if (book != null && bookVm != null)
                {
                    bookVm.AuthorName = book.Author.AuthorName;
                    bookVm.CategoryName = book.Category.CategoryName;
                    bookVm.AuthorImageURL = book.Author.AuthorImageURL;
                    if (book.Discount != null)
                    {
                        bookVm.DiscountValue = book.Discount.discountValue;
                    }
                  
                    bookVm.BookAvatar = book.BookImages.Split(';')[0];
                }
            }
            if (typeof(T1) == typeof(OrderItem) && typeof(T2) == typeof(OrderItemVM))
            {
                var orderItem = src as OrderItem;
                var orderItemVm = des as OrderItemVM;

                if (orderItem != null && orderItemVm != null)
                {
                    orderItemVm.Title = orderItem.Book.Title;
                    orderItemVm.BookImages = orderItem.Book.BookImages.Split(';')[0];
                }
            }
        }
        }
}
