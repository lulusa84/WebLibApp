/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebLibApp.Models
{
    public class MockBookRepository : IBookRepository
    {
        public List<Book> _bookList;

        public MockBookRepository()
        {
            _bookList = new List<Book>()
            {
                new Book() { BookId = 1, Title = "The importance of Being Ernest", Author = "Oscar Wilde", Edt = 1800 },
                new Book() { BookId = 2, Title = "Emma", Author = "Jane Austen", Edt = 1800 },
                new Book() { BookId = 3, Title = "il visconte dimezzato", Author = "Italo Calvino", Edt = 1900 },
            };

        }

        public Book Add(Book book)
        {
            book.BookId = _bookList.Max(e => e.BookId) + 1;
            _bookList.Add(book);
            return book;

        }

        public Book Delete(int BookId)
        {
            Book book = _bookList.FirstOrDefault(e => e.BookId == BookId);
            if (book != null)
            {
                _bookList.Remove(book);
            }
            return book;
        }

        public IEnumerable<Book> GetAllBook()
        {
            return _bookList;
        }

        public Book GetBook(int BookId)
        {
            return _bookList.FirstOrDefault(e => e.BookId == BookId);
        }

        public Book Update(Book bookChanges)
        {
            Book book = _bookList.FirstOrDefault(e => e.BookId == bookChanges.BookId);
            if (book != null)
            {
                book.Title = bookChanges.Title;
                book.Author = bookChanges.Author;
                book.Edt = bookChanges.Edt;

            }
            return book;
        }
    }
}
*/