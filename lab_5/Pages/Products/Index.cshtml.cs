using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using lab_5.Models;
using lab_5.Helpers;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Xml.Linq;

namespace lab_5.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly lab_5.Models.ProductContext _context;
		private IHostingEnvironment _env;
		private IHttpContextAccessor _httpContextAccessor;

		public IndexModel(lab_5.Models.ProductContext context, IHostingEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
			_env = env;
			_httpContextAccessor = httpContextAccessor;
			_context = context;
        }

		public string BrandSort { get; set; }
		public string DateSort { get; set; }
		public string CurrentFilter { get; set; }
		public string CurrentSort { get; set; }

		public PaginatedList<Product> Product { get; set; }


		[BindProperty]
		public IFormFile Upload { get; set; }

		public async Task OnGetAsync(string sortOrder, string currentFilter, string searchString, int? pageIndex)
        {
			CurrentSort = sortOrder;
			BrandSort = String.IsNullOrEmpty(sortOrder) ? "brand_desc" : "";
			DateSort = sortOrder == "Date" ? "date_desc" : "Date";
			CurrentFilter = searchString;

			if (searchString != null)
			{
				pageIndex = 1;
			}
			else
			{
				searchString = currentFilter;
			}

			IQueryable<Product> productIQ = from p in _context.Product.Include(p=>p.Category)
											select p;

			if (!String.IsNullOrEmpty(searchString))
			{
				productIQ = productIQ.Where(s => s.Brand.Contains(searchString)
									   || s.Model.Contains(searchString)
									   ||s.Description.Contains(searchString) 
									   ||s.Category.CategoryName.Contains(searchString));
			}

			switch (sortOrder)
			{
				case "brand_desc":
					productIQ = productIQ.OrderByDescending(s => s.Brand);
					break;
				case "Date":
					productIQ = productIQ.OrderBy(s => s.DateOfCreation);
					break;
				case "date_desc":
					productIQ = productIQ.OrderByDescending(s => s.DateOfCreation);
					break;
				default:
					productIQ = productIQ.OrderBy(s => s.Brand);
					break;
			}
			int pageSize = 5;
			Product = await PaginatedList<Product>.CreateAsync(
				productIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
		
		}

		public async Task OnPostUploadAsync(int? pageIndex)
		{
			try
			{
				var file = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/xmls", Upload.FileName);
				using (var fileStream = new FileStream(file, FileMode.Create))
				{
					await Upload.CopyToAsync(fileStream);
				}
				ProcessImport(file);
			}catch(Exception ex)
			{
			}


			IQueryable<Product> productIQ = from p in _context.Product.Include(p => p.Category)
											select p;
			int pageSize = 5;
			Product = await PaginatedList<Product>.CreateAsync(
				productIQ.AsNoTracking(), pageIndex ?? 1, pageSize);

		}

		public async Task OnPostExportAsync(int? pageIndex)
		{
		
			List<Product> productList = _context.Product.ToList();
			if (productList.Count > 0)
			{
				var xElement = new XElement("products",
					from p in productList
					select new XElement("product",
						new XElement("id", p.ProductID),
						new XElement("categoryid", p.CategoryID),
						new XElement("brand", p.Brand),
						new XElement("model", p.Model),
						new XElement("price", p.Price),
						new XElement("description", p.Description)
					));
				var contentRoot = _env.ContentRootPath;
				string pathToDataFile = contentRoot + "\\Data\\Products.xml";
				xElement.Save(pathToDataFile);

				_httpContextAccessor.HttpContext.Response.Headers.Add("Content-Disposition", "attachment; filename=Products.xml");
				_httpContextAccessor.HttpContext.Response.ContentType = "application/xml";
				var objFile = new FileInfo(pathToDataFile);
				var stream = objFile.OpenRead();
				var objBytes = new byte[stream.Length];
				stream.Read(objBytes, 0, (int)objFile.Length);
				await _httpContextAccessor.HttpContext.Response.Body.WriteAsync(objBytes);

			}
			IQueryable<Product> productIQ = from p in _context.Product.Include(p => p.Category)
											select p;
			int pageSize = 5;
			Product = await PaginatedList<Product>.CreateAsync(
				productIQ.AsNoTracking(), pageIndex ?? 1, pageSize);

		}

		private void ProcessImport(string path)
		{
			XDocument xDocument = XDocument.Load(path);
			List<Product> products = xDocument.Descendants("product").Select(p =>
				new Product()
				{
					ProductID = Convert.ToInt32(p.Element("id").Value),
					CategoryID = Convert.ToInt32(p.Element("categoryid").Value),
					Model = p.Element("model").Value,
					Brand = p.Element("brand").Value,
					Price = Convert.ToInt32(p.Element("price").Value),
					Description = p.Element("description").Value
				}).ToList();
			foreach (var product in products)
			{
				var newProduct = _context.Product.SingleOrDefault(p => p.ProductID.Equals(product.ProductID));
				if (newProduct != null)
				{
					newProduct.Brand = product.Brand;
					newProduct.CategoryID = product.CategoryID;
					newProduct.Description = product.Description;
					newProduct.Model = product.Model;
					newProduct.Price = product.Price;
					_context.Product.Add(newProduct);
				}
				else
				{
					_context.Product.Add(product);
				}			
				_context.SaveChanges();
			}


		}

	}
}
