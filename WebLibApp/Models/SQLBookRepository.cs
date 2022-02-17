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
    public class SQLBookRepository : IBookRepository
    {
        private readonly ILogger<SQLBookRepository> logger;
        private readonly AppDbContext context;
        public SQLBookRepository(AppDbContext context,
            ILogger<SQLBookRepository> logger)
        {
            this.logger = logger;
            this.context = context;
        }

        public Book Add(Book book)
        {
            context.Books.Add(book);
            context.SaveChanges();
            return book;
        }


        public Book Delete(int BookId)
        {
            Book book = context.Books.Find(BookId);
            if (book != null)
            {
                context.Books.Remove(book);
                context.SaveChanges();
            }
            return book;
        }


        public IEnumerable<Book> GetAllBook()
        {
            return context.Books;
        }


        public Book GetBook(int BookId)
        {
            logger.LogTrace("Trace Log");
            logger.LogDebug("Debug Log");
            logger.LogInformation("Information Log");
            logger.LogWarning("Warning Log");
            logger.LogError("Error Log");
            logger.LogCritical("Critical Log");

            return context.Books.Find(BookId);
        }

        public IEnumerable<Copy> GetBookCopies(int BookId)
        {
            IEnumerable<Copy> copies = GetAllCopy();

            foreach (Copy copy in copies)
            {
                if (copy != null && copy.BookId == BookId)
                {
                    GetBook(BookId).Copies.Add(copy);
                    context.SaveChanges();
                }
               
            }

            return GetBook(BookId).Copies;
                
        }

        public IEnumerable<Copy> GetBookCopiesByCopyType(int BookId, int CopyType)
        {
            IEnumerable<Copy> copies = GetBookCopies(BookId);
            foreach (Copy copy in copies)
            {
                if (copy != null && copy.BookId == BookId &&
                    copy.CopyType.Equals(CopyType))
                {
                    //book.Copies.Add(copy);
                    GetBook(BookId).Copies.Add(copy);
                    context.SaveChanges();
                }

            }
          
            return GetBook(BookId).Copies;
        }


        public Book Update(Book bookChanges)
        {
            var book = context.Books.Attach(bookChanges);
            book.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return bookChanges;

        }


        public Copy AddCopy(Copy copy)
        {
            context.Copies.Add(copy);
            context.SaveChanges();
            return copy;
        }


        public Copy DeleteCopy(int CopyId)
        {
            Copy copy = context.Copies.Find(CopyId);
            if (copy != null)
            {
                context.Copies.Remove(copy);
                context.SaveChanges();
            }
            return copy;
        }

        public IEnumerable<Copy> GetAllCopy()
        {
            return context.Copies.
                 Include(h => h.BorrowHistories).
                 Include(h => h.BookReservations);
                 

        }
        public IEnumerable<Copy> GetAllCopyByBId(int BookId)
        {
            return context.Copies.
                 Include(h => h.BorrowHistories).
                 Include(h => h.BookReservations).
                 Where(b => b.BookId == BookId);
                              

        }
        public Copy GetCopy(int CopyId)
        {
            logger.LogTrace("Trace Log");
            logger.LogDebug("Debug Log");
            logger.LogInformation("Information Log");
            logger.LogWarning("Warning Log");
            logger.LogError("Error Log");
            logger.LogCritical("Critical Log");

            return context.Copies.Find(CopyId);
        }

        public Copy GetCopyByBresId(int BookReservationId)
        {
            logger.LogTrace("Trace Log");
            logger.LogDebug("Debug Log");
            logger.LogInformation("Information Log");
            logger.LogWarning("Warning Log");
            logger.LogError("Error Log");
            logger.LogCritical("Critical Log");

            return context.Copies.Find(BookReservationId);
        }

        public Copy GetCopyByBhId(int BorrowHistoryId)
        {
            logger.LogTrace("Trace Log");
            logger.LogDebug("Debug Log");
            logger.LogInformation("Information Log");
            logger.LogWarning("Warning Log");
            logger.LogError("Error Log");
            logger.LogCritical("Critical Log");

            return context.Copies.Find(BorrowHistoryId);
        }
        public Copy GetCopyByBookId(int BookId)
        {
            logger.LogTrace("Trace Log");
            logger.LogDebug("Debug Log");
            logger.LogInformation("Information Log");
            logger.LogWarning("Warning Log");
            logger.LogError("Error Log");
            logger.LogCritical("Critical Log");

            return context.Copies.Find(BookId);
        }

        public Copy UpdateCopy(Copy copyChanges)
        {
            var copy = context.Copies.Attach(copyChanges);
            copy.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return copyChanges;
        }


    }
}
