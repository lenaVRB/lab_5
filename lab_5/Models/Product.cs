using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace lab_5.Models
{
	public class Product
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ProductID { get; set; }
		public Category Category { get; set; } 
		public int CategoryID { get; set; }
		public string Brand { get; set; } 
		public string Model { get; set; }
		public int Price { get; set; }
		public string Description { get; set; }

		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		[Display(Name = "Date of creation")]
		public DateTime DateOfCreation { get; set; }
		public string Photo { get; set; }
	}
}
