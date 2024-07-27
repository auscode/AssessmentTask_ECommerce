using E_Commerce.DTO;
using E_Commerce.Services;
using E_Commerce.Models;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : ControllerBase
    {
        private readonly CartService _cartService;

        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartItemDTO>>> GetCartItems()
        {
            try
            {
                var cartItems = await _cartService.GetCartItemsAsync();
                return Ok(cartItems);
            }
            catch (Exception ex)
            {
                // Log the exception or return an appropriate error response
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CartItemDTO>> GetCartItem(int id)
        {
            var cartItem = await _cartService.GetCartItemByIdAsync(id);
            if (cartItem == null)
            {
                return NotFound();
            }
            return Ok(cartItem);
        }

        [HttpPost]
        public async Task<ActionResult<CartItemDTO>> PostCartItem(CartItemDTO cartItemDto)
        {
            if (cartItemDto == null)
            {
                return BadRequest("CartItemDTO is null.");
            }

            // Ensure the Product exists
            var product = await _cartService.GetProductByIdAsync(cartItemDto.ProductID);
            if (product == null)
            {
                return BadRequest("Product not found.");
            }

            var cartItem = new CartItem
            {
                ProductID = cartItemDto.ProductID,
                Quantity = cartItemDto.Quantity,
                Product = product  // Initialize the Product property
            };

            var createdCartItem = await _cartService.AddCartItemAsync(cartItem);
            var createdCartItemDto = new CartItemDTO
            {
                CartItemID = createdCartItem.CartItemID,
                ProductID = createdCartItem.ProductID,
                Quantity = createdCartItem.Quantity,
                Product = new ProductDTO
                {
                    ProductID = createdCartItem.Product.ProductID,
                    ProductName = createdCartItem.Product.ProductName,
                    Price = createdCartItem.Product.Price
                }
            };

            return CreatedAtAction(nameof(GetCartItem), new { id = createdCartItemDto.CartItemID }, createdCartItemDto);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> PutCartItem(int id, CartItem cartItem)
        {
            if (id != cartItem.CartItemID)
            {
                return BadRequest();
            }

            try
            {
                await _cartService.UpdateCartItemAsync(cartItem);
            }
            catch (Exception)
            {
                if (await _cartService.GetCartItemByIdAsync(id) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCartItem(int id)
        {
            var result = await _cartService.DeleteCartItemAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPost("calculate")]
        public async Task<ActionResult<decimal>> CalculateCartTotal(int cartId, decimal discountPercentage)
        {
            var total = await _cartService.CalculateCartTotalAsync(cartId, discountPercentage);
            return Ok(total);
        }
    }
}

