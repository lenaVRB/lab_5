using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lab_5.Models
{
	public class Product
	{
		public int ProductID { get; set; }
		public Category Category { get; set; } 
		public string Brand { get; set; } 
		public string Model { get; set; }
		public int Price { get; set; }
		public string Description { get; set; }
		public DateTime DateOfCreation { get; set; }
		public string Photo { get; set; }
	}
}
