using LanguageExt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using WebLibApp.Models;
using WebLibApp.Security;
using WebLibApp.ViewModels;


namespace WebLibApp.Controllers
{
    //[Authorize]
    public class BookReservationController : Controller
    {

        private readonly IBookRepository _bookRepository;
        private readonly IBookManagementRepository _bookManagementRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        

        private readonly ILogger<BookReservationController> logger;
        public BookReservationController(

            IBookRepository bookRepository,
            IBookManagementRepository bookManagementRepository,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<BookReservationController> logger)
        {

            _bookRepository = bookRepository;
            _bookManagementRepository = bookManagementRepository;
            this.userManager = userManager;
            this.signInManager = signInManager;

            this.logger = logger;

        }
        [Route("bookReservationDetails/{BookReservationId}")]
        [AllowAnonymous]
        public ViewResult BookReservationDetails(int bookReservationId)
        {
            logger.LogTrace("Trace Log");
            logger.LogDebug("Debug Log");
            logger.LogInformation("Information Log");
            logger.LogWarning("Warning Log");
            logger.LogError("Error Log");
            logger.LogCritical("Critical Log");


            //int bookReservationId = Convert.ToInt32((BookReservationId));
            BookReservation BookReservation = _bookManagementRepository.GetBookReservationById(bookReservationId);

            // 404 Error Type 1 :
            // Resource with the specified ID doesn't exist
            if (BookReservation == null)
            {
                Response.StatusCode = 404;
                return View("BookReservationNotFound", bookReservationId);

            }

            BookReservationDetailsViewModel bookReservationDetailsViewModel = new BookReservationDetailsViewModel()
            {
                BookReservation = BookReservation,
                PageTitle = "BookReservation Details"
            };

            return View(bookReservationDetailsViewModel);

        }



        [Route("bookReservation/BookReservationCreate/{CopyId}")]
        [HttpGet]
        // GET: BookReservation/Create
        public ActionResult BookReservationCreate(int CopyId)
        {
              Copy copy = _bookRepository.GetCopy(CopyId);

                if (copy == null)
                {
                Response.StatusCode = 404;
                return View("BookNotFound", CopyId);
              
                }
            
            BookReservation BookReservation = new BookReservation { CopyId = CopyId, ReservationDate = DateTime.Now, ReservationEndDate = DateTime.Now };
            //ViewBag.AppUserId = new SelectList(HttpContext.User.Identity.Name);
            var users = userManager.Users.ToList();
            ViewBag.AppUserId = new SelectList(users, "Id", "UserName");
            return View(BookReservation);
        }
        

        // POST: BookReservation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        /*[ValidateAntiForgeryToken]*/
        public ActionResult BookReservationCreate([Bind("BookReservationId,CopyId,AppUserId,ReservationDate,ReservationEndDate")] BookReservation bookReservation)
        {
            if (ModelState.IsValid)
            {
                var users = userManager.Users.ToList();
                 ViewBag.AppUserId = new SelectList(users, "Id", "UserName", bookReservation.AppUserId);
                
                 
                _bookManagementRepository.AddBres(bookReservation);
                bookReservation.Copy = _bookRepository.GetCopy(bookReservation.CopyId);


                bookReservation.ReservationEndDate = DateTime.Now.AddDays(1);
              _bookManagementRepository.UpdateBres(bookReservation);

              return RedirectToAction("BookReservationDetails", new { BookReservationId = bookReservation.BookReservationId });
            }

            return View();
            // return View(BookReservation);
        }


        [Route("bookReservation/BookReservationEdit/{CopyId}")]
        [HttpGet]
        // GET: BorrowHistories/Edit/5
        public ActionResult BookReservationEdit(int bookReservationId) 
        {
        BookReservation bookReservation = _bookManagementRepository.GetBookReservationById(bookReservationId);
          
            if (bookReservation == null)
            {
                Response.StatusCode = 404;
                return View("BookReservationNotFound", bookReservationId);
               
            }
            return View(bookReservation);
        }

        // POST: BookReservations/Edit
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        /*[ValidateAntiForgeryToken]*/
         public ActionResult BookReservationEdit
        ([Bind("BookReservationId,CopyId,AppUserId,ReservationDate,ReservationEndDate")] BookReservation bookReservation)
        {
            BookReservation bookReservationItem = new BookReservation();
            if (ModelState.IsValid)
            {
             bookReservationItem = _bookManagementRepository
                                   .GetBookReservation(bookReservation.BookReservationId);
              
                if (bookReservationItem == null)
                {
                    Response.StatusCode = 404;
                    return View("BookReservationNotFound", bookReservation.BookReservationId);
                    
                }

                bookReservationItem.ReservationEndDate = DateTime.Now;
                _bookManagementRepository.UpdateBres(bookReservationItem);

                return RedirectToAction("BookReservationDetails", new { BookReservationId = bookReservation.BookReservationId });

                
            }
            return View(bookReservationItem);
        }

  
    }
}

    