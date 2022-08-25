using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Commence.Models
{
    public class StoreOrderNoUserProduct
    {
        public List<int> products { get; set; }
        public string userId { get; set; }
        public string orderNo { get; set; }
    }
}