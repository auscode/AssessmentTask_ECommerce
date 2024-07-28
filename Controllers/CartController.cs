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

        private CartItem ConvertToEntity(CartItemDTO dto)
        {
            return new CartItem
            {
                CartItemID = dto.CartItemID,
                ProductID = dto.ProductID,
                Quantity = dto.Quantity,
                Product = new Product
                {
                    ProductID = dto.Product.ProductID,
                    ProductName = dto.Product.ProductName,
                    Price = dto.Product.Price
                }
            };
        }

        private CartItemDTO ConvertToDto(CartItem entity)
        {
            return new CartItemDTO
            {
                CartItemID = entity.CartItemID,
                ProductID = entity.ProductID,
                Quantity = entity.Quantity,
                Product = new ProductDTO
                {
                    ProductID = entity.Product.ProductID,
                    ProductName = entity.Product.ProductName,
                    Price = entity.Product.Price
                }
            };
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartItemDTO>>> GetCartItems()
        {
            try
            {
                var cartItems = await _cartService.GetCartItemsAsync();
                return Ok(cartItems);
            }
            catch (Exception)
            {
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
        public async Task<ActionResult<CartItemDTO>> PostCartItem([FromBody] CartItemDTO cartItemDto)
        {
            if (cartItemDto == null)
            {
                return BadRequest("CartItemDTO is null.");
            }

            var product = await _cartService.GetProductByIdAsync(cartItemDto.ProductID);
            if (product == null)
            {
                return BadRequest("Product not found.");
            }

            var cartItem = new CartItem
            {
                ProductID = cartItemDto.ProductID,
                Quantity = cartItemDto.Quantity,
                Product = product
            };

            try
            {
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
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCartItem(int id, [FromBody] CartItemDTO updatedItemDto)
        {
            if (updatedItemDto == null)
            {
                return BadRequest("Invalid cart item data.");
            }

            if (id != updatedItemDto.CartItemID)
            {
                return BadRequest("ID mismatch.");
            }

            // Convert DTO to Entity
            var cartItem = ConvertToEntity(updatedItemDto);

            try
            {
                var updatedCartItem = await _cartService.UpdateCartItemAsync(cartItem);
                return Ok(updatedCartItem);
            }
            catch (Exception)
            {
                // Handle and log the exception
                if (await _cartService.GetCartItemByIdAsync(id) == null)
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(500, "Internal server error");
                }
            }
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

        // Consider removing if not needed
        [HttpPost("calculate")]
        public async Task<ActionResult<decimal>> CalculateCartTotal(int cartId, decimal discountPercentage)
        {
            var total = await _cartService.CalculateCartTotalAsync(cartId, discountPercentage);
            return Ok(total);
        }
    }
}
