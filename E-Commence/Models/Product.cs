using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Commence.Models
{
    public class Product
    {
        [Required]
        [Display(Name = "Product name")]
        public string ProductName { get; set; }
        [Required]
        [Display(Name = "Product image")]
        public string ProductImage { get; set; }
        [Required]
        [Display(Name = "Product price")]
        public decimal ProductPrice { get; set; }
        [Required]
        [Display(Name = "Product quantity")]
        public int ProductQuantity { get; set; }

        public int ProductId { get; set; }
    }
}