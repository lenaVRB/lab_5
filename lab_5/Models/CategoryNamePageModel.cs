using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace lab_5.Models
{
	public class CategoryNamePageModel : PageModel
	{
		public SelectList CategoryList { get; set; }

		public void PopulateCategoriesDropDownList(ProductContext _context,
			object selectedCategory = null)
		{
			var categoriesQuery = from c in _context.Category
								  orderby c.CategoryName
								  select c;

			CategoryList = new SelectList(categoriesQuery.AsNoTracking(),
						"CategoryID", "CategoryName", selectedCategory);
		}
	}
}
