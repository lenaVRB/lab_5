using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using lab_5.Models;

namespace lab_5.Pages.Products
{
    public class EditModel : CategoryNamePageModel
    {
        private readonly lab_5.Models.ProductContext _context;

        public EditModel(lab_5.Models.ProductContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Product Product { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

			Product = await _context.Product.Include(p=>p.Category).FirstOrDefaultAsync(p=>p.ProductID==id);

			if (Product == null)
            {
                return NotFound();
            }
			PopulateCategoriesDropDownList(_context, Product.CategoryID);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

			var productToUpdate = await _context.Product.FindAsync(id);

			if (await TryUpdateModelAsync<Product>(
				productToUpdate,
				"product",   // Prefix for form value.
				s => s.CategoryID, s => s.Model, s => s.Price, s => s.Brand, s => s.DateOfCreation, s => s.Photo, s => s.Description))
			{
				await _context.SaveChangesAsync();
				return RedirectToPage("./Index");
			}

			PopulateCategoriesDropDownList(_context, productToUpdate.CategoryID);
			return Page();
		}
    }
}
