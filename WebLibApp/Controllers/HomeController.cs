using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using WebLibApp.Models;
using WebLibApp.Security;
using WebLibApp.ViewModels;

namespace WebLibApp.Controllers
{
    //Attribute Routing attributes 
    //conbined with attribute for single actions
    //[Route("[Home]")]
    //[Route("[controller]/[action]")]
    //[Authorize]
    public class HomeController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ILogger<HomeController> logger;
        
        private readonly IDataProtector protector;
       
        public HomeController(IBookRepository bookRepository,
            IWebHostEnvironment webHostEnvironment,
            ILogger<HomeController> logger,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings)

        {

            _bookRepository = bookRepository;
            this.webHostEnvironment = webHostEnvironment;
            this.logger = logger;
            protector = dataProtectionProvider
                 .CreateProtector(dataProtectionPurposeStrings.BookIdRouteValue);
        }

        //Attribute Routing attributes
        //[Route("~/Home")]
        // 1 [Route("Home")]
        // 2 [Route("Home/Index")]
        // 3 [Route("[action]")]
        // 4 [Route ("~/")]
        [AllowAnonymous]
        public ViewResult Index()
        {
            var model = _bookRepository.GetAllBook()
                        .Select(e =>
                        {
                            e.EncryptedId = protector.Protect(e.BookId.ToString());
                            return e;
                        });

            return View(model);

        }

        /* 
          * 1 [Route("Home/Details/{BookId?}")]
          * 2 [Route("Details/{BookId?}")]
          * 3 [Route("[action]/{BookId?}")]
          * 4 [Route("{BookId?}")]           */

        [AllowAnonymous]
        // public ViewResult Details(int? bookId)
        public ViewResult Details(string id)
        {
            logger.LogTrace("Trace Log");
            logger.LogDebug("Debug Log");
            logger.LogInformation("Information Log");
            logger.LogWarning("Warning Log");
            logger.LogError("Error Log");
            logger.LogCritical("Critical Log");

            int bookId = Convert.ToInt32(protector.Unprotect(id));

            Book book = _bookRepository.GetBook(bookId);
            /*
             * Book book = _bookRepository.GetBook(bookId.Value);
             * Book book = _bookRepository.GetBook(bookId ?? 1);
             * HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel() */

            // 404 Error Type 1 :
            // Resource with the specified ID doesn't exist
            if (book == null)
            {
                Response.StatusCode = 404;
                return View("BookNotFound", bookId);
                //return View("BookNotFound", bookId.Value);
            }

            HomeDetailsViewModel bookDetailsViewModel = new HomeDetailsViewModel()
            {
                Book = book,
                PageTitle = "Book Details"
            };

            return View(bookDetailsViewModel);

            // Json(model);
            // _bookRepository.GetBook(1).Title;*/
        }


        [HttpGet]
        [Authorize]
        public ViewResult Create()
        {
            return View();
        }


        [HttpGet]
        [Authorize]
        public ViewResult Edit(int id)
        // public ViewResult Edit(int? bookId)
        {
            Book book = _bookRepository.GetBook(id);
            //Book book = _bookRepository.GetBook(id ?? 1);
            BookEditViewModel bookEditViewModel = new BookEditViewModel
            {
                BookId = book.BookId,
                Title = book.Title,
                Author = book.Author,
                Edt = book.Edt,
                ExistingPhotoPath = book.PhotoPath

            };
            return View(bookEditViewModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(BookEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Book book = _bookRepository.GetBook(model.BookId);
                book.Title = model.Title;
                //book.Author = model.Author;
                book.Edt = model.Edt;
                if (model.Photo != null)
                {
                    if (model.ExistingPhotoPath != null)
                    {
                        string filePath = System.IO.Path.Combine(webHostEnvironment.WebRootPath,
                        "images", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }

                    book.PhotoPath = ProcessUploadedFile(model);
                }

                _bookRepository.Update(book);
                // Book updatedBook =  _bookRepository.Update(book);
                return RedirectToAction("index");
            }
            return View();

        }

        public string ProcessUploadedFile(BookCreateViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photo != null)
            {
                //if (model.Photo != null && model.Photo.Count > 0)
                //{
                //foreach (IFormFile photo in model.Photo)

                string uploadsFolder = System.IO.Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = System.IO.Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }



        [HttpPost]
        [Authorize]
        public IActionResult Create(BookCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadedFile(model);
                Book newBook = new Book
                {
                    Title = model.Title,
                    Author = model.Author,
                    Edt = model.Edt,
                    PhotoPath = uniqueFileName
                };

                _bookRepository.Add(newBook);
                return RedirectToAction("details", new { id = newBook.BookId });
            }
            return View();
        }


       

    }

}


