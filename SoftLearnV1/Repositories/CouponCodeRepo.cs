using Microsoft.Extensions.Configuration;
using SoftLearnV1.Entities;
using SoftLearnV1.Helpers;
using SoftLearnV1.InterfaceRepositories;
using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using SoftLearnV1.Reusables;
using SoftLearnV1.Security;
using SoftLearnV1.Services.Email;
using SoftLearnV1.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Repositories
{
    public class CouponCodeRepo : ICouponCodeRepo
    {
        private readonly AppDbContext _context;
        public CouponCodeRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<GenericResponseModel> applyCouponCodesAsync(ApplyCouponCodeRequestModel obj)
        {
            try
            {
                var couponCode = _context.CouponCodes.Where(c => c.CouponCode.ToUpper() == obj.CouponCode.ToUpper()).FirstOrDefault();
                var couponCodeUsed = _context.CouponCodes.Where(c => c.CouponCode.ToUpper() == obj.CouponCode.ToUpper() && c.IsUsed == true).FirstOrDefault();
                var cart = _context.Cart.Where(c => c.Id == obj.CartId).FirstOrDefault();
               
                if (cart == null)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Cart with the specified ID" };
                }
                else if (couponCode == null || couponCodeUsed != null)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Used/Invalid Coupon Code" };
                }
                else
                {
                    //get the coupon Percentage
                    long couponPercentage = couponCode.CouponPercentage;
                    //get the cartSubtotal
                    long cartSubtotal = cart.SubTotal;
                    //calculate the amount payable after applying the coupon code
                    long amountPayable = (couponPercentage * cartSubtotal) / 100;

                    //update the TotalamountPayable of the cart
                    cart.TotalAmountPayable = amountPayable;
                    cart.CouponCode = obj.CouponCode.ToUpper();
                    await _context.SaveChangesAsync();

                    //update Couponcode isUsed to true
                    couponCode.IsUsed = true;
                    await _context.SaveChangesAsync();

                    //save the coupon codes used by learners
                    var appCoup = new UsedCouponCodes
                    {
                        CouponCode = obj.CouponCode.ToUpper(),
                        CartId = obj.CartId,
                        LearnerId = obj.LearnerId,
                        DateUsed = DateTime.Now,

                    };
                    await _context.UsedCouponCodes.AddAsync(appCoup);
                    await _context.SaveChangesAsync();

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Coupon Code Applied" };

                }
            }
            catch (Exception exMessage)
            {
                var error = new ErrorLog
                {
                    ErrorMessage = exMessage.Message,
                    ErrorSource = exMessage.Source,
                    ErrorStackTrace = exMessage.StackTrace,
                    ErrorDate = DateTime.Now
                };
                await _context.ErrorLog.AddAsync(error);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericResponseModel> createCouponCodesAsync(CouponCreateRequestModel obj)
        {
            try
            {
                var checkCoupon = _context.CouponCodes.Where(c => c.CouponCode == obj.CouponCode.ToUpper()).FirstOrDefault();

                if (checkCoupon != null)
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "This Coupon Code Already Exists" };
                }
                else if (obj.CouponPercentage > 100)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Coupon Percentage Cannot Exceed 100%" };
                }
                else
                {
                    //CouponCode 
                    var coup = new CouponCodes
                    {
                        CouponCode = obj.CouponCode.ToUpper(),
                        CouponPercentage = obj.CouponPercentage,
                        CreatedById = obj.CreatedById,
                        IsApproved = true,
                        IsUsed = false,
                        DateCreated = DateTime.Now
                    };
                    await _context.CouponCodes.AddAsync(coup);
                    await _context.SaveChangesAsync();

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = " Coupon Code Created Successfully" };

                }
            }
            catch (Exception exMessage)
            {
                var error = new ErrorLog
                {
                    ErrorMessage = exMessage.Message,
                    ErrorSource = exMessage.Source,
                    ErrorStackTrace = exMessage.StackTrace,
                    ErrorDate = DateTime.Now
                };
                await _context.ErrorLog.AddAsync(error);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericResponseModel> getCouponCodesByIdAsync(long couponCodeId)
        {
            try
            {
                var result = from cp in _context.CouponCodes where cp.Id == couponCodeId
                             select new
                             {
                                 cp.Id,
                                 cp.CouponCode,
                                 cp.CouponPercentage,
                                 cp.IsApproved,
                                 cp.IsUsed,
                                 cp.CreatedById,
                                 cp.DateCreated
                             };


                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault(), };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericResponseModel> getAllCouponCodesAsync()
        {
            try
            {
                var result = from cp in _context.CouponCodes
                             select new
                             {
                                 cp.Id,
                                 cp.CouponCode,
                                 cp.CouponPercentage,
                                 cp.IsApproved,
                                 cp.IsUsed,
                                 cp.CreatedById,
                                 cp.DateCreated
                             };
                            

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericResponseModel> getCouponCodesByCouponCodeAsync(string couponCode)
        {
            try
            {
                var result = from cp in _context.CouponCodes where cp.CouponCode.ToUpper().Trim() == couponCode.ToUpper().Trim()
                             select new
                             {
                                 cp.Id,
                                 cp.CouponCode,
                                 cp.CouponPercentage,
                                 cp.IsApproved,
                                 cp.IsUsed,
                                 cp.CreatedById,
                                 cp.DateCreated
                             };


                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault(), };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }
    }
}
