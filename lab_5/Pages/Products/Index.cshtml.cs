using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using lab_5.Models;

namespace lab_5.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly lab_5.Models.ProductContext _context;

        public IndexModel(lab_5.Models.ProductContext context)
        {
            _context = context;
        }

		public string BrandSort { get; set; }
		public string DateSort { get; set; }
		public string PriceSort { get; set; }
		public string CurrentFilter { get; set; }
		public string CurrentSort { get; set; }

		public IList<Product> Product { get;set; }

        public async Task OnGetAsync(string sortOrder)
        {
			BrandSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
			PriceSort = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
			DateSort = sortOrder == "Date" ? "date_desc" : "Date";

			IQueryable<Student> studentIQ = from s in _context.Student
											select s;

			switch (sortOrder)
			{
				case "name_desc":
					studentIQ = studentIQ.OrderByDescending(s => s.LastName);
					break;
				case "Date":
					studentIQ = studentIQ.OrderBy(s => s.EnrollmentDate);
					break;
				case "date_desc":
					studentIQ = studentIQ.OrderByDescending(s => s.EnrollmentDate);
					break;
				default:
					studentIQ = studentIQ.OrderBy(s => s.LastName);
					break;
			}

			Student = await studentIQ.AsNoTracking().ToListAsync();
		}
    }
}
