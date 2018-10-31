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
    public class DeleteModel : PageModel
    {
        private readonly lab_5.Models.ProductContext _context;

        public DeleteModel(lab_5.Models.ProductContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Product Product { get; set; }
		public string ErrorMessage { get; set; }

		public async Task<IActionResult> OnGetAsync(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product = await _context.Product.AsNoTracking().FirstOrDefaultAsync(m => m.ProductID == id);

            if (Product == null)
            {
                return NotFound();
            }

			if (saveChangesError.GetValueOrDefault())
			{
				ErrorMessage = "Delete failed. Try again";
			}
			return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.AsNoTracking().FirstOrDefaultAsync(p=>p.ProductID==id);

			if (product == null)
			{
				return NotFound();
			}

			try
			{
				_context.Product.Remove(product);
				await _context.SaveChangesAsync();
				return RedirectToPage("./Index");
			}
			catch (DbUpdateException  ex )
			{
				//Log the error 
				return RedirectToAction("./Delete",
									 new { id, saveChangesError = true });
			}
		}
    }
}
