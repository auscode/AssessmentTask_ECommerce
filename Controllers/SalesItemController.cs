using Microsoft.AspNetCore.Mvc;
using E_Commerce.Models;
using E_Commerce.Services;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesItemController : ControllerBase
    {
        private readonly ISalesItemService _salesItemService;

        public SalesItemController(ISalesItemService salesItemService)
        {
            _salesItemService = salesItemService;
        }

        [HttpPost]
        public IActionResult CreateSalesItem([FromBody] SalesItemCreateDTO salesItemDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _salesItemService.CreateSalesItem(salesItemDto);

            if (result == null)
                return BadRequest("Unable to create SalesItem.");

            return CreatedAtAction(nameof(CreateSalesItem), new { id = result.SalesItemID }, result);
        }
    }
}
