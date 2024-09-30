using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CheburekInfrastructure.Models;

namespace CheburekInfrastructure.Controllers
{
    public class CartsController : Controller
    {
        private readonly CheburekMVCContext _context;

        public CartsController(CheburekMVCContext context)
        {
            _context = context;
        }

        // GET: Carts
        public async Task<IActionResult> Index()
        {
            var cartItems = await _context.Carts
                .Include(c => c.Product)
                .ThenInclude(p => p.Category)
                .ToListAsync();

            return View(cartItems);
        }

        // POST: Carts/UpdateQuantity
        [HttpPost]
        public IActionResult UpdateQuantity(int id, int quantity)
        {
            var cartItem = _context.Carts
                .Include(c => c.Product)
                .FirstOrDefault(c => c.Id == id);

            if (cartItem == null || cartItem.Product == null)
            {
                return NotFound();
            }

            cartItem.Quantity = quantity;
            cartItem.TotalPrice = cartItem.Product.Price * quantity;
            _context.SaveChanges();

            return Ok(new { totalPrice = cartItem.TotalPrice });
        }

        // POST: Carts/AddToCart
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            var existingCartItem = await _context.Carts
                .FirstOrDefaultAsync(c => c.ProductId == productId);

            if (existingCartItem != null)
            {
                TempData["Message"] = "Товар додано до кошика.";
                return RedirectToAction("Index", "Products");
            }

            var cartItem = new Cart
            {
                ProductId = product.Id,
                Quantity = 1,
                TotalPrice = product.Price
            };

            _context.Carts.Add(cartItem);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Carts");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmOrder()
        {
            var cartItems = await _context.Carts.Include(c => c.Product).ToListAsync();
            if (!cartItems.Any())
            {
                TempData["Message"] = "Ваш кошик порожній.";
                return RedirectToAction("Index", "Carts");
            }

            // Створюємо нове замовлення
            var order = new Order
            {
                OrderDate = DateTime.Now,
                Status = "Підтверджено" // Можна додати статус
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Очищуємо кошик після створення замовлення
            _context.Carts.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            // Перекидаємо користувача на сторінку замовлень з показом нового замовлення
            return RedirectToAction("Index", "Orders", new { orderId = order.Id });
        }

        private bool CartExists(int id)
        {
            return _context.Carts.Any(e => e.Id == id);
        }
    }
}
