using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebLibApp.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().HasData(
        new Book
        {
           BookId = 1,
            Title = "The importance of being Ernest",
            Author = "Oscar Wilde",
            Edt = 1800
        },
        new Book
        {
            BookId = 2,
            Title = "Orgoglio e Pregiudizio", /*"Emma"*/
            Author = "Jane Austen",
            Edt = 1800
        },
        new Book
        {
            BookId = 4,
            Title = "I promessi sposi",
            Author = "Alessandro Manzoni",
            Edt = 1800
        },
        new Book
        {
            BookId = 3,
            Title = "Marcovaldo",
            Author = "Italo calvino",
            Edt = 1900

        }


       );

        }
    }
}
