using SoftLearnV1.Entities;
using SoftLearnV1.Helpers;
using SoftLearnV1.InterfaceRepositories;
using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using SoftLearnV1.Reusables;
using SoftLearnV1.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Repositories
{
    public class CartRepo : ICartRepo
    {
        private readonly AppDbContext _context;
        public CartRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CartResponseModel> createCartAsync()
        {
            try
            {
                //Generates a Random AlphaNumeric for the cart Reference ID
                var cartReferenceId = RandomNumberGenerator.RandomString();
                var cart = new Cart
                {
                    CartReferenceId = cartReferenceId,
                    IsCheckedOut = false,
                    DateCreated = DateTime.Now,
                };
                await _context.Cart.AddAsync(cart);
                await _context.SaveChangesAsync();

                return new CartResponseModel { StatusCode = 200, StatusMessage = "Cart Created Successfully", Data = cart };
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new CartResponseModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }

        public async Task<CartResponseModel> addItemsToCartAsync(AddItemsToCartRequestModel obj)
        {
            try
            {
                //check the courseId
                var checkCourseId = new CheckerValidation(_context).checkCourseById(obj.CourseId);
                //check the cartId
                var checkCartId = new CheckerValidation(_context).checkCartById(obj.CartId);

                //check if an item exists in the cart
                var checkItemExist = _context.CartItems.FirstOrDefault(c => c.CartId == obj.CartId && c.CourseId == obj.CourseId);

                if (checkCourseId != true)
                {
                    return new CartResponseModel { StatusCode = 200, StatusMessage = "No Course with the specified ID" };
                }
                else if (checkCartId != true)
                {
                    return new CartResponseModel { StatusCode = 200, StatusMessage = "No Cart with the specified ID" };
                }
                else if (checkItemExist != null)
                {
                    return new CartResponseModel { StatusCode = 409, StatusMessage = "Course Exists in the Cart!" };
                }
                else
                {
                    var cartItems = new CartItems
                    {
                        CartId = obj.CartId,
                        CourseId = obj.CourseId,
                        DateCreated = DateTime.Now,
                    };
                    await _context.CartItems.AddAsync(cartItems);
                    await _context.SaveChangesAsync();

                    //get the all Courses added to the cart
                    var respCartItems = from cr in _context.CartItems where cr.CartId == obj.CartId
                                        select new
                                        {
                                            cr.Id,
                                            CartId = cr.Cart.Id,
                                            cr.Cart.CartReferenceId,
                                            cr.CourseId,
                                            cr.Courses.FacilitatorId,
                                            Facilitator = cr.Courses.Facilitators.FirstName + " " + cr.Courses.Facilitators.LastName,
                                            cr.Courses.CourseSubTitle,
                                            cr.Courses.CourseName,
                                            cr.Courses.CourseImageUrl,
                                            cr.Courses.CourseType.CourseTypeName,
                                            cr.Courses.CourseLevelTypes.LevelTypeName,
                                            cr.Courses.CourseCategory.CourseCategoryName,
                                            cr.Courses.CourseCategory.CategoryImageUrl,
                                            cr.Courses.CourseCategory.CategoryDescription,
                                            cr.Courses.CourseSubCategory.CourseSubCategoryName,
                                            cr.Courses.CourseSubCategory.CourseSubCategoryImageUrl,
                                            cr.Courses.CourseSubCategory.CourseSubCategoryDescription,
                                            cr.Courses.CourseAmount,
                                            cr.DateCreated,
                                        };
             
                    return new CartResponseModel { StatusCode = 200, StatusMessage = "Course Added To Cart!", CartItems = respCartItems.ToList() };
                }

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new CartResponseModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }

        public async Task<CartResponseModel> getCartItemsAsync(long cartId)
        {
            try
            {
                //check the cartId
                var checkCartId = new CheckerValidation(_context).checkCartById(cartId);

                if (checkCartId != true)
                {
                    return new CartResponseModel { StatusCode = 200, StatusMessage = "No Cart with the specified ID" };
                }
                else
                {
                    //get the all Courses added to the cart
                    var respCartItems = from cr in _context.CartItems
                                        where cr.CartId == cartId
                                        select new
                                        {
                                            cr.Id,
                                            CartId = cr.Cart.Id,
                                            cr.Cart.CartReferenceId,
                                            cr.CourseId,
                                            cr.Courses.FacilitatorId,
                                            Facilitator = cr.Courses.Facilitators.FirstName +" "+ cr.Courses.Facilitators.LastName,
                                            cr.Courses.CourseSubTitle,
                                            cr.Courses.CourseName,
                                            cr.Courses.CourseDescription,
                                            cr.Courses.CourseImageUrl,
                                            cr.Courses.CourseType.CourseTypeName,
                                            cr.Courses.CourseLevelTypes.LevelTypeName,
                                            cr.Courses.CourseCategory.CourseCategoryName,
                                            cr.Courses.CourseCategory.CategoryImageUrl,
                                            cr.Courses.CourseCategory.CategoryDescription,
                                            cr.Courses.CourseSubCategory.CourseSubCategoryName,
                                            cr.Courses.CourseSubCategory.CourseSubCategoryImageUrl,
                                            cr.Courses.CourseSubCategory.CourseSubCategoryDescription,
                                            cr.Courses.CourseAmount,
                                            cr.DateCreated,
                                        };

                    return new CartResponseModel { StatusCode = 200, StatusMessage = "Successful", CartItems = respCartItems.ToList() };
                }

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new CartResponseModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }

        public async Task<CartResponseModel> deleteCartItemsAsync(long cartItemId, long cartId)
        {
            try
            {
                //check the cartId
                var checkCartId = new CheckerValidation(_context).checkCartById(cartId);
                //check the cartItemId
                var checkCartItemId = new CheckerValidation(_context).checkCartItemById(cartItemId);

                if (checkCartId != true)
                {
                    return new CartResponseModel { StatusCode = 200, StatusMessage = "No Cart with the specified ID" };
                }
                else if (checkCartItemId != true)
                {
                    return new CartResponseModel { StatusCode = 200, StatusMessage = "No Cart Item with the specified ID" };
                }
                else
                {
                    //delete the item from the cart
                    var deleteItem = _context.CartItems.FirstOrDefault(c => c.Id == cartItemId && c.CartId == cartId);
                    if (deleteItem != null)
                    {
                        _context.CartItems.Remove(deleteItem);
                        await _context.SaveChangesAsync();
                    }

                    //get the all Courses added to the cart
                    var respCartItems = from cr in _context.CartItems
                                        where cr.CartId == cartId
                                        select new
                                        {
                                            cr.Id,
                                            CartId = cr.Cart.Id,
                                            cr.Cart.CartReferenceId,
                                            cr.CourseId,
                                            cr.Courses.FacilitatorId,
                                            Facilitator = cr.Courses.Facilitators.FirstName + " " + cr.Courses.Facilitators.LastName,
                                            cr.Courses.CourseSubTitle,
                                            cr.Courses.CourseName,
                                            cr.Courses.CourseImageUrl,
                                            cr.Courses.CourseType.CourseTypeName,
                                            cr.Courses.CourseLevelTypes.LevelTypeName,
                                            cr.Courses.CourseCategory.CourseCategoryName,
                                            cr.Courses.CourseCategory.CategoryImageUrl,
                                            cr.Courses.CourseCategory.CategoryDescription,
                                            cr.Courses.CourseSubCategory.CourseSubCategoryName,
                                            cr.Courses.CourseSubCategory.CourseSubCategoryImageUrl,
                                            cr.Courses.CourseSubCategory.CourseSubCategoryDescription,
                                            cr.Courses.CourseAmount,
                                            cr.DateCreated,
                                        };

                    return new CartResponseModel { StatusCode = 200, StatusMessage = "Course Deleted From Cart!", CartItems = respCartItems.ToList() };
                }

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new CartResponseModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }

        public async Task<CartResponseModel> getCartSubTotalAsync(long cartId)
        {
            try
            {
                //check the cartId
                var checkCartId = new CheckerValidation(_context).checkCartById(cartId);
               
                if (checkCartId != true)
                {
                    return new CartResponseModel { StatusCode = 200, StatusMessage = "No Cart with the specified ID" };
                }
                else
                {
                    var crt = from cr in _context.CartItems where cr.CartId == cartId select cr.Courses; //selects all the courses in the cart

                    long subTotal = 0;
                    long countCartItems = 0;
                    foreach (Courses crts in crt)
                    {
                        subTotal = subTotal + crts.CourseAmount;
                        countCartItems++;
                    }

                    var cart = _context.Cart.FirstOrDefault(c => c.Id == cartId);

                    if (cart != null) //update the CartItems
                    {
                        cart.SubTotal = subTotal;
                        cart.TotalCourse = countCartItems;
                        cart.CouponCode = "";
                        cart.TotalAmountPayable = subTotal; //To Paystack

                        await _context.SaveChangesAsync();

                    }

                    return new CartResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = cart };
                }

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new CartResponseModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }
    }
}
