using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using lab_5.Models;

namespace lab_5.Pages.Products
{
    public class CreateModel : PageModel
    {
        private readonly lab_5.Models.ProductContext _context;

        public CreateModel(lab_5.Models.ProductContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Product Product { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

			var emptyProduct = new Product();

			if (await TryUpdateModelAsync<Product>(
				emptyProduct,
				"product",   // Prefix for form value.
				s => s.Model, s => s.Price, s => s.Brand, s=>s.DateOfCreation, s=>s.Photo, s=>s.Description))
			{
				_context.Product.Add(emptyProduct);
				await _context.SaveChangesAsync();
				return RedirectToPage("./Index");
			}

			return null;
		}
    }
}