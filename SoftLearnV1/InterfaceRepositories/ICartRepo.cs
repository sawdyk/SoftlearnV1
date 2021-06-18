using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.InterfaceRepositories
{
    public interface ICartRepo
    {
        Task<CartResponseModel> createCartAsync();
        Task<CartResponseModel> addItemsToCartAsync(AddItemsToCartRequestModel obj);
        Task<CartResponseModel> getCartItemsAsync(long cartId);
        Task<CartResponseModel> deleteCartItemsAsync(long cartItemId, long cartId);
        Task<CartResponseModel> getCartSubTotalAsync(long cartId);
        //Task<CartResponseModel> addCouponCodeAsync(long cartSubtotalId, string couponCode);

    }
}
