using E_Commence.Models;
using E_Commence.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace E_Commence.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> ListProduct()
        {
            ViewBag.UID = Request.QueryString["uid"].ToString();
            ProductDTO productDTO = new ProductDTO();
            var products = await ConsumeApi.GetAsync($"api/Products/ECommenceProducts", Settings.AppSession["token"].ToString());
            if (products != null)
            {
                productDTO.Products = JsonConvert.DeserializeObject<List<Product>>(products.ToString());
            }
            return View(productDTO);
        }
        public ActionResult Product()
        {
            return View();
        }

        private List<ProductQuantity> GetGroupedProductOrders(string productIds)
        {
            var prdids = productIds.Split(',').Select(
                                                        a => new ProductQuantity { 
                                                                 ProductId = Convert.ToInt32(a), 
                                                                 Quantity = 1 
                                                     });
            var grouped = prdids.GroupBy(x => x.ProductId).Select(
                                                                    y => new ProductQuantity 
                                                                         { 
                                                                             ProductId = y.Key, 
                                                                             Quantity = y.Sum(x => x.Quantity) 
                                                                 });
            return grouped.ToList();
        }
        public async Task<string> ViewShoppingCartByProductIds(string productIds)
        {
            try
            {
                var productQuantities = GetGroupedProductOrders(productIds);
                var shoppingCarts = await ConsumeApi.PostAsync($"api/ViewShoppingCart/ViewShoppingCartByProductQuantity", productQuantities, Settings.AppSession["token"].ToString());
                if (shoppingCarts != null)
                {
                    return shoppingCarts.ToString();
                }
                return "An error has occured, please you can contact the system administrator!";
            }
            catch(Exception)
            {
                return "An error has occured, please you can contact the system administrator!";
            }
            
        }

        public async Task<ActionResult> OrderProduct(string uid, string productIds, string orderNo)
        {
            try
            {
                StoreOrderNoUserProduct _storeOrderNoUserProduct = new StoreOrderNoUserProduct();
                _storeOrderNoUserProduct.products = productIds.Split(',').Select(a => Convert.ToInt32(a)).ToList();
                _storeOrderNoUserProduct.userId = uid;
                _storeOrderNoUserProduct.orderNo = orderNo;
                var productOrder = await ConsumeApi.PostAsync($"api/UserProductOrder/StoreUserProductOrder", _storeOrderNoUserProduct, Settings.AppSession["token"].ToString());
                Settings.AppSession.Add("StoreOrderNoUserProduct" + uid, _storeOrderNoUserProduct);

                return RedirectToAction("ListProduct", "Product", new { uid = _storeOrderNoUserProduct.userId });
            }
            catch(Exception)
            {
                return RedirectToAction("LogIn", "SignIn");
            }
        }

        public async Task<string> ProductsCheckOut(string uid)
        {
            try
            {
                StoreOrderNoUserProduct _storeOrderNoUserProduct = (StoreOrderNoUserProduct)Settings.AppSession["StoreOrderNoUserProduct" + uid];
                if (_storeOrderNoUserProduct != null)
                {
                    CheckoutProductOrder _checkoutProductOrder = new CheckoutProductOrder();
                    _checkoutProductOrder.UserId = _storeOrderNoUserProduct.userId;
                    _checkoutProductOrder.Products = _storeOrderNoUserProduct.products;
                    _checkoutProductOrder.EmailAddress = string.Empty;
                    _checkoutProductOrder.OrderNo = _storeOrderNoUserProduct.orderNo;
                    var checkOut = await ConsumeApi.PostAsync($"api/CheckOut/UserProductCheckOut", _checkoutProductOrder, Settings.AppSession["token"].ToString());

                    return checkOut.ToString();
                }
                return "An error has occured, please you can contact the system administrator!";
            }
            catch(Exception)
            {
                return "An error has occured, please you can contact the system administrator!";
            }
        }
    }
}