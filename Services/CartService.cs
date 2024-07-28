using E_Commerce.DTO; // Add this namespace for DTOs
using E_Commerce.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Services
{
    public class CartService
    {
        private readonly ApplicationDbContext _context;

        public CartService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CartItemDTO>> GetCartItemsAsync()
        {
            var cartItems = await _context.CartItems
                .Include(ci => ci.Product)
                .ToListAsync();

            return cartItems.Select(ci => new CartItemDTO
            {
                CartItemID = ci.CartItemID,
                ProductID = ci.ProductID,
                Quantity = ci.Quantity,
                Product = new ProductDTO
                {
                    ProductID = ci.Product.ProductID,
                    ProductName = ci.Product.ProductName,
                    Price = ci.Product.Price
                }
            }).ToList();
        }
        public async Task<Product> GetProductByIdAsync(int productId)
        {
            return await _context.Products.FindAsync(productId);
        }

        public async Task<CartItemDTO> GetCartItemByIdAsync(int id)
        {
            var cartItem = await _context.CartItems
                .Include(ci => ci.Product)
                .FirstOrDefaultAsync(ci => ci.CartItemID == id);

            if (cartItem == null) return null;

            return new CartItemDTO
            {
                CartItemID = cartItem.CartItemID,
                ProductID = cartItem.ProductID,
                Quantity = cartItem.Quantity,
                Product = new ProductDTO
                {
                    ProductID = cartItem.Product.ProductID,
                    ProductName = cartItem.Product.ProductName,
                    Price = cartItem.Product.Price
                }
            };
        }

        public async Task<CartItemDTO> AddCartItemAsync(CartItem cartItem)
        {
            // Check if the Product exists in the database
            var product = await _context.Products.FindAsync(cartItem.ProductID);
            if (product == null)
            {
                throw new Exception("Product not found.");
            }

            cartItem.Product = product; // Ensure the Product is assigned

            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();

            return new CartItemDTO
            {
                CartItemID = cartItem.CartItemID,
                ProductID = cartItem.ProductID,
                Quantity = cartItem.Quantity,
                Product = new ProductDTO
                {
                    ProductID = cartItem.Product.ProductID,
                    ProductName = cartItem.Product.ProductName,
                    Price = cartItem.Product.Price
                }
            };
        }


        public async Task<CartItemDTO> UpdateCartItemAsync(CartItem cartItem)
        {
            var existingItem = await _context.CartItems.FindAsync(cartItem.CartItemID);
            if (existingItem == null)
            {
                throw new Exception("Cart item not found.");
            }

            existingItem.Quantity = cartItem.Quantity;
            _context.CartItems.Update(existingItem);
            await _context.SaveChangesAsync();

            return new CartItemDTO
            {
                CartItemID = existingItem.CartItemID,
                ProductID = existingItem.ProductID,
                Quantity = existingItem.Quantity,
                Product = new ProductDTO
                {
                    ProductID = existingItem.Product.ProductID,
                    ProductName = existingItem.Product.ProductName,
                    Price = existingItem.Product.Price
                }
            };
        }



        public async Task<bool> DeleteCartItemAsync(int id)
        {
            var cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem == null)
            {
                return false;
            }

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<decimal> CalculateCartTotalAsync(int cartId, decimal discountPercentage)
        {
            var cartItems = await _context.CartItems
                .Where(ci => ci.CartItemID == cartId)
                .Include(ci => ci.Product)
                .ToListAsync();

            var total = cartItems.Sum(ci => ci.Quantity * ci.Product.Price);
            var discountedTotal = total - (total * (discountPercentage / 100));
            return discountedTotal;
        }

        internal async Task UpdateCartItemAsync(CartItemDTO existingItem)
        {
            throw new NotImplementedException();
        }
    }
}
