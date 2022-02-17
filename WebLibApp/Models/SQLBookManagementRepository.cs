using LanguageExt;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using WebLibApp.Models;
using WebLibApp.ViewModels;

namespace WebLibApp.Models
{
    public class SQLBookManagementRepository : IBookManagementRepository
    {
        private readonly ILogger<SQLBookManagementRepository> logger;
        private readonly AppDbContext context;
        public SQLBookManagementRepository(AppDbContext context,
            ILogger<SQLBookManagementRepository> logger)
        {
            this.logger = logger;
            this.context = context;
        }
        public BorrowHistory AddBh(BorrowHistory borrowHistory)
        {
            context.BorrowHistories.Add(borrowHistory);
            context.SaveChanges();
            return borrowHistory;
        }

        public BookReservation AddBres(BookReservation bookReservation)
        {
            context.BookReservations.Add(bookReservation);
            context.SaveChanges();
            return bookReservation;
        }

        public BorrowHistory DeleteBh(int borrowHistoryId)
        {
            BorrowHistory borrowHistory = context.BorrowHistories.Find(borrowHistoryId);
            if (borrowHistory != null)
            {
                context.BorrowHistories.Remove(borrowHistory);
                context.SaveChanges();
            }
            return borrowHistory;
        }

        public BookReservation DeleteBRes(int BookReservationId)
        {
            BookReservation bookReservation = context.BookReservations.Find(BookReservationId);
            if (bookReservation != null)
            {
                context.BookReservations.Remove(bookReservation);
                context.SaveChanges();
            }
            return bookReservation;
        }

        public IEnumerable<BorrowHistory> GetAllborrowHistories()
        {
            return context.BorrowHistories.Include(b => b.Copy);
                
        }

            public IEnumerable<BookReservation> GetAllBookReservations()
        {
            return context.BookReservations.Include(b => b.Copy);
        }


        public BorrowHistory GetBorrowHistory(int CopyId)
        {
            logger.LogTrace("Trace Log");
            logger.LogDebug("Debug Log");
            logger.LogInformation("Information Log");
            logger.LogWarning("Warning Log");
            logger.LogError("Error Log");
            logger.LogCritical("Critical Log");

            return context.BorrowHistories
                               .Include(b => b.Copy)
                               .Where(b => b.CopyId == CopyId && b.ReturnDate != DateTime.Now)
                               .FirstOrDefault();

        }
  

        public BorrowHistory GetBorrowHistoryById(int BorrowHistoryId)
        {
            logger.LogTrace("Trace Log");
            logger.LogDebug("Debug Log");
            logger.LogInformation("Information Log");
            logger.LogWarning("Warning Log");
            logger.LogError("Error Log");
            logger.LogCritical("Critical Log");

            return context.BorrowHistories.Find(BorrowHistoryId);
                                  
            
        }


        public BookReservation GetBookReservation(int CopyId)
        {
            logger.LogTrace("Trace Log");
            logger.LogDebug("Debug Log");
            logger.LogInformation("Information Log");
            logger.LogWarning("Warning Log");
            logger.LogError("Error Log");
            logger.LogCritical("Critical Log");

            
            return context.BookReservations
                                       .Include(b => b.Copy)
                                       .Where(b => b.CopyId == CopyId && b.ReservationEndDate != DateTime.Now)
                                       .FirstOrDefault();
        }

        public BookReservation GetBookReservationById(int BookReservationId)
        {
            logger.LogTrace("Trace Log");
            logger.LogDebug("Debug Log");
            logger.LogInformation("Information Log");
            logger.LogWarning("Warning Log");
            logger.LogError("Error Log");
            logger.LogCritical("Critical Log");

            return context.BookReservations.Find(BookReservationId);


        }

        public BorrowHistory UpdateBh(BorrowHistory borrowHistoryChanges)
        {
            var borrowHistory = context.BorrowHistories.Attach(borrowHistoryChanges);
            borrowHistory.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return borrowHistoryChanges;
            
        }

        public BookReservation UpdateBres(BookReservation bookReservationChanges)
        {
            var bookReservation = context.BookReservations.Attach(bookReservationChanges);
            bookReservation.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return bookReservationChanges;
            ;
        }

        
    }
}
