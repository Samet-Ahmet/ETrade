using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Business.Constants;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.VisualBasic.CompilerServices;
using WebUI.Models;

namespace WebUI.Controllers
{
    [Authorize(Roles = "Manager, Worker")]
    public class AdminProductController : Controller
    {
        private readonly IHostingEnvironment _environment;
        private IProductService _productService;

        public AdminProductController(IHostingEnvironment hostingEnvironment, IProductService productService)
        {
            _environment = hostingEnvironment;
            _productService = productService;
        }

        [HttpGet]
        public IActionResult Add()
        {
            var model = new AddProductViewModel
            {
                ProductDetailDto = new ProductDetailDto
                {
                    Product = new Product(),
                    ProductPhotoPaths = new List<ProductPhotoPath>()
                }
            };
            return View(model);
        }

        

        [HttpPost]
        public IActionResult Add(ProductDetailDto productDetailDto)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var uploadedProductPhotoPaths = new List<ProductPhotoPath>();
            if (HttpContext.Session.GetString("sayac") != null)
            {
                var sayacString = HttpContext.Session.GetString("sayac");
                int sayac = Convert.ToInt32(sayacString);
                for (int i = 1; i < sayac; i++)
                {
                    uploadedProductPhotoPaths.Add(new ProductPhotoPath
                    {
                        PhotoPath = HttpContext.Session.GetString(i.ToString())
                    });
                }
                HttpContext.Session.Remove("sayac");
                productDetailDto.ProductPhotoPaths = uploadedProductPhotoPaths;
            }

            var result = _productService.Add(productDetailDto);
            if (!result.Success)
            {
                return RedirectToAction("InternalError", "Error", new { errorMessage = result.Message });
            }
            TempData.Add(TempDataTypes.AdminInfo, Messages.ProductAddedSuccessfully);
            return RedirectToAction("Add", "AdminProduct");
        }


        [HttpPost]
        public IActionResult Upload(IFormFile files)
        {
            if (files != null)
            {
                if (files.Length > 0)
                {
                    //Getting FileName
                    var fileName = Path.GetFileName(files.FileName);

                    //Assigning Unique Filename (Guid)
                    var myUniqueFileName = Convert.ToString(Guid.NewGuid());

                    //Getting file Extension
                    var fileExtension = Path.GetExtension(fileName);

                    // concatenating  FileName + FileExtension
                    var newFileName = String.Concat(myUniqueFileName, fileExtension);

                    // Combines two strings into a path.
                    var filepath =
                        new PhysicalFileProvider(Path.Combine(/*Directory.GetCurrentDirectory(),*/ _environment.WebRootPath,/*"wwwroot",*/ "ProductPhotos")).Root + $@"\{newFileName}";

                  /*  string toBeSearched = "wwwroot";
                    int ix = filepath.IndexOf(toBeSearched);
                    string relativePath = new string("");
                    if (ix != -1)
                    {
                        fileName = filepath.Substring(ix + toBeSearched.Length);
                        // do something here
                   }
                   */
                    if (HttpContext.Session.GetString("sayac") != null)
                    {
                        var sayacString = HttpContext.Session.GetString("sayac");
                        int sayac = Convert.ToInt32(sayacString);
                        HttpContext.Session.SetString(sayac.ToString(), newFileName);
                        HttpContext.Session.SetString("sayac",(sayac+1).ToString());
                    }
                    else
                    {
                        HttpContext.Session.SetString("sayac",2.ToString());
                        HttpContext.Session.SetString("1", newFileName);
                    }
                    
                    using (FileStream fs = System.IO.File.Create(filepath))
                    {
                        files.CopyTo(fs);
                        fs.Flush();
                    }
                }
            }
            TempData.Add(TempDataTypes.PhotoUploaded, Messages.PhotoUploaded);
            // return RedirectToAction("Add", "AdminProduct");
           // return Ok();
           return RedirectToAction("GoBack", "AdminProduct");
        }

        public IActionResult GoBack()
        {
            return View();
        }
    }
}

