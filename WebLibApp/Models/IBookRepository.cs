using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLibApp.ViewModels;

namespace WebLibApp.Models
{
    public interface IBookRepository
    {
        Book GetBook(int BookId);
        IEnumerable<Book> GetAllBook();
        Book Add(Book book);
        Book Update(Book bookChanges);
        Book Delete(int BookId);
        IEnumerable<Copy> GetBookCopies(int BookId);
        IEnumerable<Copy> GetBookCopiesByCopyType(int BookId, int CopyType);

        Copy GetCopy(int CopyId);
        Copy GetCopyByBhId(int BorrowHistoryId);
        Copy GetCopyByBresId(int BoookReservationId);
        Copy GetCopyByBookId(int BookId);
        IEnumerable<Copy> GetAllCopy();
        IEnumerable<Copy> GetAllCopyByBId(int BookId);
        Copy AddCopy(Copy copy);
        Copy UpdateCopy(Copy copyChanges);
        Copy DeleteCopy(int CopyId);
    }
}
