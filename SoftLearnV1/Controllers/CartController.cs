using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SoftLearnV1.InterfaceRepositories;
using SoftLearnV1.RequestModels;


namespace SoftLearnV1.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepo _cartRepo;

        public CartController(ICartRepo cartRepo)
        {
            _cartRepo = cartRepo;
        }

        [HttpPost("createCart")]
        [AllowAnonymous]
        public async Task<IActionResult> createCartAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _cartRepo.createCartAsync();

            return Ok(result);
        }

        [HttpPost("addItemsToCart")]
        [AllowAnonymous]
        public async Task<IActionResult> addItemsToCartAsync(AddItemsToCartRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _cartRepo.addItemsToCartAsync(obj);

            return Ok(result);
        }

        [HttpGet("cartItems")]
        [AllowAnonymous]
        public async Task<IActionResult> getCartItemsAsync(long cartId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _cartRepo.getCartItemsAsync(cartId);

            return Ok(result);
        }

        [HttpDelete("deleteCartItems")]
        [AllowAnonymous]
        public async Task<IActionResult> deleteCartItemsAsync(long cartItemId, long cartId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _cartRepo.deleteCartItemsAsync(cartItemId, cartId);

            return Ok(result);
        }

        [HttpGet("cartSubTotalCheckOut")]
        [AllowAnonymous]
        public async Task<IActionResult> getCartSubTotalAsync(long cartId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _cartRepo.getCartSubTotalAsync(cartId);

            return Ok(result);
        }
    }
}
