using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Commence.Models
{
    public class ShoppingCartProduct
    {
        public string ProductName { get; set; }
        public int ProductQuantity { get; set; }
        public decimal ProductPrice { get; set; }
    }
}