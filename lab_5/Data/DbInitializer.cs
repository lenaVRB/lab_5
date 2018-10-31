using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lab_5.Models;

namespace lab_5.Data
{
	public class DbInitializer
	{
		public static void Initialize(ProductContext context)
		{

			if (context.Product.Any())
			{
				return; 
			}

			var categories = new Category[]
			{
				new Category{CategoryName="Phones"},
				new Category{CategoryName="Cameras"},
			};
			foreach (Category c in categories)
			{
				context.Category.Add(c);
			}
			context.SaveChanges();

			var products = new Product[]
			{
				new Product{Brand="Apple",Model="Iphone 7",DateOfCreation=DateTime.Parse("2018-10-30"), Price=1200, Description=""},
				new Product{Brand="Samsung",Model="S9",DateOfCreation=DateTime.Parse("2018-09-29"), Price=1000, Description=""},
				new Product{Brand="Canon",Model="EOS",DateOfCreation=DateTime.Parse("2017-09-01"), Price=900, Description=""},

			};
			foreach (Product p in products)
			{
				context.Product.Add(p);
			}
			context.SaveChanges();
		}
	}
}
