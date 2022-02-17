using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebLibApp.Models
{
    public interface IBookManagementRepository
    { 
        BorrowHistory GetBorrowHistory(int CopyId);
        BorrowHistory GetBorrowHistoryById(int BorrowHistoryId);
        IEnumerable<BorrowHistory> GetAllborrowHistories();
        BorrowHistory AddBh(BorrowHistory borrowHistory);
        BorrowHistory UpdateBh(BorrowHistory borrowHistoryChanges);
        BorrowHistory DeleteBh(int borrowHistoryId);

        BookReservation GetBookReservation(int CopyId);
        BookReservation GetBookReservationById(int BookReservationId);
        IEnumerable<BookReservation> GetAllBookReservations();
        BookReservation AddBres(BookReservation bookReservation);
        BookReservation UpdateBres(BookReservation bookReservationChanges);
        BookReservation DeleteBRes(int BookReservationtId);
       
    }
}
