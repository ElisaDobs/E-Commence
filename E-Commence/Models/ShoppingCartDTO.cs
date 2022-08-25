using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Commence.Models
{
    public class ShoppingCartDTO
    {
        public List<ShoppingCartProduct> ShoppingCarts { get; set; }
        public decimal TotalProductPrice { get; set; }
    }
}