using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using lab_5.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace lab_5.Controllers
{
    public class ProductController : Controller
    {
		private readonly ProductContext _context;

		public ProductController(ProductContext context)
		{
			_context = context;
		}

		[HttpPost]
		[Route("importXML")]
		public async Task<IActionResult> ImportXMLAsync(IFormFile xmlFile)
		{
			try
			{
				var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/xmls", xmlFile.FileName);
				using (var stream = new FileStream(path, FileMode.Create))
				{
					await xmlFile.CopyToAsync(stream);
				}
				ViewBag.products = ProcessImport(path);
				ViewBag.result = "Done";
			}
			catch (Exception e)
			{
				ViewBag.result = "Done";
			}
			
			return View("Index");
		}

		private List<Product> ProcessImport(string path)
		{
			XDocument xDocument = XDocument.Load(path);
			List<Product> products = xDocument.Descendants("product").Select(p =>
				new Product()
				{
					ProductID = Convert.ToInt32(p.Element("id").Value),
					Model = p.Element("model").Value,
					Brand = p.Element("brand").Value,
					Price = Int32.Parse(p.Element("price").Value),
					Description = p.Element("description").Value
				}).ToList();
			foreach(var product in products)
			{
				var newProduct = _context.Product.SingleOrDefault(p => p.ProductID.Equals(product.ProductID));
				if (newProduct == null)
				{
					_context.Product.Add(newProduct);
					_context.SaveChanges();
				}				
				
			}

			return products;
		}
    }
}