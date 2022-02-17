using Bogus.Extensions;
using LanguageExt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using WebLibApp.Models;
using WebLibApp.ViewModels;


namespace WebLibApp.Controllers
{
    //[Authorize]
    public class BookManagementController : Controller
    {

        private readonly IBookRepository _bookRepository;
        private readonly IBookManagementRepository _bookManagementRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

       
        private readonly ILogger<BookManagementController> logger;
        public BookManagementController(

            IBookRepository bookRepository,
            IBookManagementRepository bookManagementRepository,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<BookManagementController> logger)
        {

            _bookRepository = bookRepository;
            _bookManagementRepository = bookManagementRepository;
            this.userManager = userManager;
            this.signInManager = signInManager;

            this.logger = logger;

        }


        [Route("borrowHistoryDetails/{BorrowHistoryId}")]
        [AllowAnonymous]
        public ViewResult BorrowHistoryDetails(string BorrowHistoryId)
        {
            logger.LogTrace("Trace Log");
            logger.LogDebug("Debug Log");
            logger.LogInformation("Information Log");
            logger.LogWarning("Warning Log");
            logger.LogError("Error Log");
            logger.LogCritical("Critical Log");


            int borrowHistoryId = Convert.ToInt32((BorrowHistoryId));
            BorrowHistory borrowHistory = _bookManagementRepository.GetBorrowHistoryById(borrowHistoryId);

            // 404 Error Type 1 :
            // Resource with the specified ID doesn't exist
            if (borrowHistory == null)
            {
                Response.StatusCode = 404;
                return View("borrowHistoryNotFound", BorrowHistoryId);

            }

            BorrowHistoryDetailsViewModel borrowHistoryDetailsViewModel = new BorrowHistoryDetailsViewModel()
            {
                BorrowHistory = borrowHistory,
                PageTitle = "borrowHistory Details"
            };

            return View(borrowHistoryDetailsViewModel);

        }


        
        [Route("bookManagement/BorrowHistoryCreate/{CopyId}")]
        [HttpGet]
        // GET: BorrowHistories/Create
        public ActionResult BorrowHistoryCreate(int CopyId)
        {
              Copy copy = _bookRepository.GetCopy(CopyId);

                if (copy == null)
                {
                Response.StatusCode = 404;
                return View("BookNotFound", CopyId);
              
                }
            
            BorrowHistory borrowHistory = new BorrowHistory { CopyId = CopyId, BorrowDate = DateTime.Now, ReturnDate = DateTime.Now };
            //ViewBag.AppUserId = new SelectList(HttpContext.User.Identity.Name);
            var users = userManager.Users.ToList();
            ViewBag.AppUserId = new SelectList(users, "Id", "UserName");
            return View(borrowHistory);
        }


        // POST: BorrowHistories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        /*[ValidateAntiForgeryToken]*/
        public ActionResult BorrowHistoryCreate([Bind("BorrowHistoryId,CopyId,AppUserId,BorrowDate,ReturnDate,Duration,Delay,DelayFee")] BorrowHistory borrowHistory)
        {

            if (ModelState.IsValid)
            {

                var users = userManager.Users.ToList();
                ViewBag.AppUserId = new SelectList(users, "Id", "UserName", borrowHistory.AppUserId);
                
                
                _bookManagementRepository.AddBh(borrowHistory);
                borrowHistory.Copy = _bookRepository.GetCopy(borrowHistory.CopyId);

                borrowHistory.ReturnDate = DateTime.Now.AddDays(1);
               _bookManagementRepository.UpdateBh(borrowHistory);

                return RedirectToAction("borrowHistoryDetails", new { BorrowHistoryId = borrowHistory.BorrowHistoryId });
           }
            return View();
        }


        [Route("bookManagement/BorrowHistoryEdit/{CopyId}")]
        [HttpGet]
        // GET: BorrowHistories/Edit
        public ActionResult BorrowHistoryEdit(int CopyId) 
        {
        BorrowHistory borrowHistory = _bookManagementRepository.GetBorrowHistory(CopyId);
          
            if (borrowHistory == null)
            {
                Response.StatusCode = 404;
                return View("BorrowHistoryNotFound", CopyId);
               
            }
            return View(borrowHistory);
        }

        // POST: BorrowHistories/Edit
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        /*[ValidateAntiForgeryToken]*/
        public ActionResult BorrowHistoryEdit
        ([Bind("BorrowHistoryId,CopyId,AppUserId,BorrowDate,ReturnDate,Duration,Delay,DelayFee")] BorrowHistory borrowHistory)
        {
            BorrowHistory borrowHistoryItem = new BorrowHistory();
            if (ModelState.IsValid)
            {
             borrowHistoryItem = _bookManagementRepository
                                 .GetBorrowHistoryById(borrowHistory.BorrowHistoryId);
              
                if (borrowHistoryItem == null)
                {
                    Response.StatusCode = 404;
                    return View("BorrowHistoryNotFound", borrowHistory.BorrowHistoryId);
                    
                }

                 borrowHistoryItem.ReturnDate = DateTime.Now;
                _bookManagementRepository.UpdateBh(borrowHistoryItem);

                return RedirectToAction("BorrowHistoryDetails", new { BorrowHistoryId = borrowHistory.BorrowHistoryId });

                
            }
            return View(borrowHistoryItem);
        }

       

        [Route("bookManagement/Copies/{bookid}")]
        [HttpGet]
        public ActionResult Copies(int bookId)
        {
            IList<CopyViewModel> ModelList = new List<CopyViewModel>();
            IEnumerable<Copy> listCopies = _bookRepository.GetAllCopyByBId(bookId).ToList();
          
            foreach (Copy copy in listCopies)
            {
                CopyViewModel model = new CopyViewModel();
               

                model.Copy = copy;
                if (model.Copy.BorrowHistories.Count()!= 0 && model.Copy.CopyType.ToString().Equals("R"))
                {
                    model.IsAvailable = !model.Copy.BorrowHistories
                   .Any(h => (h.ReturnDate > DateTime.Now));
                    
                }
                else if (model.Copy.BorrowHistories.Count() == 0 || model.Copy.CopyType.ToString().Equals("C"))
                {
                    model.IsAvailable = false;
                }
                else
                {
                    model.IsAvailable = true;
                }

                model.IsReservationAllowed = model.Copy.BookReservations
                .Any(Copy => _bookManagementRepository.GetBookReservation(Copy.CopyId) == null)
                && !model.IsAvailable;

                ModelList.Add(model);
            }
            return View(ModelList);
        }


      
    }
}

    