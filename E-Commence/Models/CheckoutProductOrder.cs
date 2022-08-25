using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Commence.Models
{
    public class CheckoutProductOrder
    {
        public string UserId { get; set; }
        public IEnumerable<int> Products { get; set; }
        public string OrderNo { get; set; }
        public string EmailAddress { get; set; }
    }
}