using BaByBoi.DataAccess.Service;
using BaByBoi.DataAccess.Service.Interface;
using BaByBoi.Domain.Models;
using BaByBoi.Domain.Utils;
using Firebase.Storage;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
using System.Net.Http;
using System;

namespace BaByBoi_Project.Pages.Admin
{
    public class ProductModel : PageModel
    {
        private readonly IProductService _productService;

        public ProductModel(IProductService productService) {
            _productService = productService;
        }

        [BindProperty]
        public List<Product> ListProductPagin { get; set; }
        [BindProperty]
        public int CurrentIndex { get; set; }

        [BindProperty]
        public int PageIndex { get; set; }
        [BindProperty]
        public int PageSize { get; set; }
        [BindProperty]
        public int TotalPage { get; set; }

        [BindProperty]
        public string SearchValue { get; set; }

        [BindProperty]
        public Product AddProduct { get; set; }
        [BindProperty]
        public Product UpdateProduct { get; set; }

        [BindProperty]
        public List<Category> listCagetory { get; set; }
        [BindProperty]
        public List<Size> listSize { get; set; }
        [BindProperty]
        public string[] SelectedSize { get; set; }
        [BindProperty]
        public List<ProductImage> ProductImages { get; set; }

        [BindProperty]
        public int ProductIdRemove { get; set; }
       
        private async Task LoadDataAsync(string? PageIndex, string PageSize)
        {
            var listProduct = await _productService.GetAllProduct();
            try
            {
                if (PageIndex == null && PageSize == null)
                {
                    this.PageIndex = PageHelper.DefaultPageIndex;
                    this.PageSize = PageHelper.DefaultPageSize;
                }
                else if (int.Parse(PageIndex) <= 0)
                {
                    this.PageIndex = PageHelper.DefaultPageIndex;
                    this.PageSize = PageHelper.DefaultPageSize;
                }
                else if (int.Parse(PageIndex) > (int)Math.Ceiling(listProduct.Count / (double)int.Parse(PageSize)))
                {
                    this.PageIndex = TotalPage;
                    this.PageSize = int.Parse(PageSize);
                }
                else
                {
                    this.PageIndex = int.Parse(PageIndex);
                    this.PageSize = int.Parse(PageSize);
                }
                TotalPage = (int)Math.Ceiling(listProduct.Count / (double)this.PageSize);
                listCagetory = await _productService.GetAllCagetory();
                listSize = await _productService.GetAllSize();
                ProductImages = await _productService.GetImages();
                ListProductPagin = await _productService.GetAllProductsPagingAsync(this.PageIndex, this.PageSize);
                CurrentIndex = this.PageIndex;
            }
            catch (Exception)
            {
                this.PageIndex = PageHelper.DefaultPageIndex;
                this.PageSize = PageHelper.DefaultPageSize;
                TotalPage = (int)Math.Ceiling(listProduct.Count / (double)this.PageSize);
                ProductImages = await _productService.GetImages();
                ListProductPagin = await _productService.GetAllProductsPagingAsync(this.PageIndex, this.PageSize);
                listCagetory = await _productService.GetAllCagetory();
                listSize = await _productService.GetAllSize();
                CurrentIndex = this.PageIndex;
            }
        } 

        public async Task OnGet(string PageIndex, string PageSize)
        {
            await LoadDataAsync(PageIndex, PageSize);
        }

        public async Task<IActionResult> OnPostSearchProduct()
        {
            if(SearchValue == null)
            {
                SearchValue = "";
            }
            List<Product> result = await _productService.SearchProduct(SearchValue);
            TotalPage = (int)Math.Ceiling(result.Count / (double)5);
            ListProductPagin = result;
            ProductImages = await _productService.GetImages();
            listCagetory = await _productService.GetAllCagetory();
            listSize = await _productService.GetAllSize();
            CurrentIndex = this.PageIndex;
            return Page();

        }
       
        public async Task<IActionResult> OnPostAddProduct(List<IFormFile> ListImgURL, IFormCollection form)
        {
            var newProduct = new Product();
            if (AddProduct != null)
            {
                var status = form["productStatus"];
                AddProduct.Status = int.Parse(status.ToString());
                AddProduct.CreateDate = DateTime.Now;
                newProduct = await _productService.AddProduct(AddProduct);
            }
            List<ProductImage> ListDownloadURL = new List<ProductImage>();
            List<ProductSize> ListProductSize = new List<ProductSize>();

            var listAddSize = form["SelectedSize"].ToArray();
            var listAmountProduct = form["productAmount"].ToArray();
            var listPriceProduct = form["productPrice"].ToArray();
            int i = 0;
            int j = 0;
            int k = 0;
            foreach (var size in listAddSize)
            {
                while (j < listAmountProduct.Length)
                {
                    if (string.IsNullOrEmpty(listAmountProduct[j]))
                    {
                        listAmountProduct[i] = listAmountProduct[++j];
                        if(string.IsNullOrEmpty(listAmountProduct[i]))
                        {
                            j++;
                        } else
                        {
                            break;
                        }
                    } else
                    {
                        j++;
                        break;
                    }
                }
                while (k < listPriceProduct.Length)
                {
                    if (string.IsNullOrEmpty(listPriceProduct[k]))
                    {
                        listPriceProduct[i] = listPriceProduct[++k];
                        if (string.IsNullOrEmpty(listPriceProduct[i]))
                        {
                            k++;
                        }
                        else
                        {
                            break;
                        }
                    } else
                    {
                        k++;
                        break;
                    }
                }


                var productSize = new ProductSize()
                {
                    SizeId = int.Parse(size.ToString()),
                    Quantity = int.Parse(listAmountProduct[i]),
                    Price = int.Parse(listPriceProduct[i]),
                    ProductId = newProduct.ProductId
                };
                ListProductSize.Add(productSize);
                i++;
            }

            foreach (IFormFile postedFile in ListImgURL)
            {
                string fileName = Path.GetFileName(postedFile.FileName);
                var firebaseStorage = new FirebaseStorage(FirebaseConfig.STORAGE_BUCKET);
                await firebaseStorage.Child("product").Child(fileName).PutAsync(postedFile.OpenReadStream());
                var downloadUrl = await firebaseStorage.Child("product").Child(fileName).GetDownloadUrlAsync();
                var productImage = new ProductImage()
                {
                    ImageName = postedFile.FileName,
                    ImageUrl = downloadUrl,
                    ProductId = newProduct.ProductId,
                };
                ListDownloadURL.Add(productImage);
            }
            var checkAdd = await _productService.AddImagesAndSize(newProduct, ListDownloadURL, ListProductSize);
            if (checkAdd)
            {
                ViewData["Message"] = "Add Product Success";

            }
            else
            {
                ViewData["ErrorMessage"] = "Add Product Failed";
            }
            
            PageSize = 5;
            var listProduct = await _productService.GetAllProduct();
            TotalPage = (int)Math.Ceiling(listProduct.Count / (double)5);
            PageIndex = TotalPage - 1;
            var temp = await _productService.GetAllProductsPagingAsync(PageIndex, 5);
            ProductImages = await _productService.GetImages();
            ListProductPagin = temp.OrderByDescending(x => x.ProductId).ToList();
            listCagetory = await _productService.GetAllCagetory();
            listSize = await _productService.GetAllSize();
            CurrentIndex = this.PageIndex;
            return Page();
           
        }

        public async Task<IFormFile> ConvertUrlToIFormFile(string UrlRaw)
        {
            using (var httpClient = new HttpClient())
            {
                var uriBuilder = new UriBuilder();
                if (!UrlRaw.StartsWith("blob:"))
                {
                    uriBuilder = new UriBuilder(UrlRaw);
                    var response = await httpClient.GetAsync(uriBuilder.Uri);
                    var bytes = await response.Content.ReadAsByteArrayAsync();
                    var stream = new MemoryStream(bytes);
                    var fileName = Path.GetFileName(response.Content.Headers.ContentDisposition.FileNameStar);
                    var contentType = response.Content.Headers.ContentType?.MediaType;

                    return new FormFile(stream, 0, stream.Length, null, fileName)
                    {
                        Headers = new HeaderDictionary
                        {
                            { "Content-Type", contentType }
                        }
                    };
                } else
                {
                    return null;
                }
                    
            }
        }

        public async Task<IActionResult> OnPostUpdateProduct(List<IFormFile> imageUrls, IFormCollection form)
        {

            var oldProduct = await _productService.GetProductByProductId(UpdateProduct.ProductId);
            var newProduct = new Product();
            if (oldProduct != null)
            {
                var status = form["productStatus"];
                UpdateProduct.Status = int.Parse(status.ToString());
                UpdateProduct.UpdateDate = DateTime.Now;
                UpdateProduct.CreateDate = oldProduct.CreateDate;
                newProduct = await _productService.UpdateProduct(UpdateProduct);
            }
            List<ProductImage> ListDownloadURL = new List<ProductImage>();
            List<ProductSize> ListProductSize = new List<ProductSize>();
           
          

            var listAddSize = form["SelectedUpdateSize"].ToArray();
            var listAmountProduct = form["productUpdateAmount"].ToArray();
            var listPriceProduct = form["productUpdatePrice"].ToArray();
            int i = 0;
            int j = 0;
            int k = 0;
            foreach (var size in listAddSize)
            {
                while (j < listAmountProduct.Length)
                {
                    if (string.IsNullOrEmpty(listAmountProduct[j]))
                    {
                        listAmountProduct[i] = listAmountProduct[++j];
                        if (string.IsNullOrEmpty(listAmountProduct[i]))
                        {
                            j++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        j++;
                        break;
                    }
                }
                while (k < listPriceProduct.Length)
                {
                    if (string.IsNullOrEmpty(listPriceProduct[k]))
                    {
                        listPriceProduct[i] = listPriceProduct[++k];
                        if (string.IsNullOrEmpty(listPriceProduct[i]))
                        {
                            k++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        k++;
                        break;
                    }
                }


                var productSize = new ProductSize()
                {
                    SizeId = int.Parse(size.ToString()),
                    Quantity = int.Parse(listAmountProduct[i]),
                    Price = int.Parse(listPriceProduct[i]),
                    ProductId = oldProduct.ProductId
                };
                ListProductSize.Add(productSize);
                i++;
            }

          

            foreach (var postedFile in imageUrls)
            {
                string fileName = Path.GetFileName(postedFile.FileName);
                var firebaseStorage = new FirebaseStorage(FirebaseConfig.STORAGE_BUCKET);
                await firebaseStorage.Child("product").Child(fileName).PutAsync(postedFile.OpenReadStream());
                var downloadUrl = await firebaseStorage.Child("product").Child(fileName).GetDownloadUrlAsync();
                var productImage = new ProductImage()
                {
                    ImageName = postedFile.FileName,
                    ImageUrl = downloadUrl,
                    ProductId = oldProduct.ProductId,
                };
                ListDownloadURL.Add(productImage);
            }

            List<IFormFile> CovertURLToIFormFIle = new List<IFormFile>();
            foreach (var postedFile in form["imageUrlsUpLoad"])
            {
                var converted = await ConvertUrlToIFormFile(postedFile);
                if (converted != null)
                {
                    CovertURLToIFormFIle.Add(converted);
                }
            }

            
           
                try
                {
                  foreach(var deleteImages in oldProduct.ProductImages)
                    {
                            // Create a Firebase Storage client
                            var firebaseStorage = new FirebaseStorage(FirebaseConfig.STORAGE_BUCKET);
                            // Parse the image URL to get the file name
                            var fileName = deleteImages.ImageUrl.Substring(deleteImages.ImageUrl.LastIndexOf('/') + 1);
                            fileName = fileName.Split('?')[0]; // Remove the query parameters
                            var encodedFileName = Path.GetFileName(fileName);
                            var fileNameOfficial = Uri.UnescapeDataString(encodedFileName);


                            // Delete the image from Firebase Storage
                            var fileRef = firebaseStorage.Child(fileNameOfficial);
                            await fileRef.DeleteAsync();
                    }

                }
                catch (Exception ex)
                {
                    // Handle any errors that occur during the deletion process
                    Console.WriteLine($"Error deleting image: {ex.Message}");
                }

            foreach (var postedFile in CovertURLToIFormFIle)
            {
                string fileName = Path.GetFileName(postedFile.FileName);
                var firebaseStorage = new FirebaseStorage(FirebaseConfig.STORAGE_BUCKET);
                await firebaseStorage.Child("product").Child(fileName).PutAsync(postedFile.OpenReadStream());
                var downloadUrl = await firebaseStorage.Child("product").Child(fileName).GetDownloadUrlAsync();
                var productImage = new ProductImage()
                {
                    ImageName = postedFile.FileName,
                    ImageUrl = downloadUrl,
                    ProductId = oldProduct.ProductId,
                };
                ListDownloadURL.Add(productImage);
            }

            var checkRemoveOldImagesAndSize = await _productService.RemoveImagesAndSize(oldProduct.ProductId);
            if (checkRemoveOldImagesAndSize)
            {
                var checkAdd = await _productService.AddImagesAndSize(oldProduct, ListDownloadURL, ListProductSize);
                if(checkAdd)
                {
                    ViewData["Message"] = "Update Product Success";
                } else
                {
                    ViewData["ErrorMessage"] = "Update Product Failed";
                }
            }
            else
            {
                ViewData["ErrorMessage"] = "Update Product Failed";
            }

            PageSize = 5;
            var listProduct = await _productService.GetAllProduct();
            TotalPage = (int)Math.Ceiling(listProduct.Count / (double)5);
            PageIndex = CurrentIndex;
            ProductImages = await _productService.GetImages();
            ListProductPagin = await _productService.GetAllProductsPagingAsync(PageIndex, 5);
            listCagetory = await _productService.GetAllCagetory();
            listSize = await _productService.GetAllSize();
            CurrentIndex = this.PageIndex;
            return Page();
        }

        public async Task<IActionResult> OnPostRemoveProduct()
        {
            if(ProductIdRemove != 0)
            {
                var getProductRemove = await _productService.GetProductByProductId(ProductIdRemove);
                if(getProductRemove != null) {
                    var checkDeleteImageAndSize = await _productService.RemoveImagesAndSize(getProductRemove.ProductId);
                    if(checkDeleteImageAndSize) 
                    {
                        var checkDeleteProduct = await _productService.DeleteProduct(getProductRemove.ProductId);
                        if( checkDeleteProduct )
                        {
                            ViewData["Message"] = "Delete Product Success";
                        } else
                        {
                            ViewData["ErrorMessage"] = "Delete Product Failed";
                        }
                    }
                }
            }
            PageSize = 5;
            var listProduct = await _productService.GetAllProduct();
            TotalPage = (int)Math.Ceiling(listProduct.Count / (double)5);
            PageIndex = CurrentIndex;
            ProductImages = await _productService.GetImages();
            ListProductPagin = await _productService.GetAllProductsPagingAsync(PageIndex, 5);
            listCagetory = await _productService.GetAllCagetory();
            listSize = await _productService.GetAllSize();
            CurrentIndex = this.PageIndex;
            return Page();
        }

        public async Task<IActionResult> OnPostResetProduct()
        {
            await LoadDataAsync("1", "5");
            return Page();
        }
    }
   
}
