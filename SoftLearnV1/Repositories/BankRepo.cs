using Microsoft.EntityFrameworkCore;
using SoftLearnV1.Entities;
using SoftLearnV1.Helpers;
using SoftLearnV1.InterfaceRepositories;
using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using SoftLearnV1.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Repositories
{
    public class BankRepo : IBankRepo
    {
        private readonly AppDbContext _context;

        public BankRepo(AppDbContext context)
        {
            this._context = context;
        }

        public async Task<GenericResponseModel> createBankAsync(BankRequestModel obj)
        {
            try
            {
                var bankExist = await _context.Banks.Where(x => x.BankName == obj.BankName && x.Code == obj.Code).FirstOrDefaultAsync();

                if (bankExist == null)
                {
                    //Save the Bank
                    var newbank = new Bank
                    {
                        BankName = obj.BankName,
                        Code = obj.Code,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };
                    await _context.Banks.AddAsync(newbank);
                    await _context.SaveChangesAsync();


                    //return the bank Created
                    var result = from cr in _context.Banks.Where(c => c.Id == newbank.Id)
                                 select new
                                 {
                                     cr.Id,
                                     cr.BankName,
                                     cr.Code,
                                     cr.CreatedAt,
                                     cr.UpdatedAt
                                 };

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Bank Created Successfully!", Data = result.FirstOrDefault() };

                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "This Bank Already Exists!" };

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

        public async Task<GenericResponseModel> deleteBankAsync(long bankId)
        {
            try
            {
                var bankExist = await _context.Banks.Where(x => x.Id == bankId).FirstOrDefaultAsync();

                if (bankExist != null)
                {
                    //Save the Bank
                    _context.Banks.Remove(bankExist);
                    await _context.SaveChangesAsync();

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Bank Deleted Successfully!"};

                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "Bank not found!" };

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

        public async Task<GenericResponseModel> getAllBankAsync()
        {
            try
            {
                //return all the banks Created
                var result = from cr in _context.Banks
                             select new
                             {
                                 cr.Id,
                                 cr.BankName,
                                 cr.Code,
                                 cr.CreatedAt,
                                 cr.UpdatedAt
                             };
                if (result.Count() > 0)
                {

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful!", Data = await result.ToListAsync() };
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Bank found!" };

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

        public async Task<GenericResponseModel> getBankByIdAsync(long bankId)
        {
            try
            {
                //return all the banks Created
                var result = from cr in _context.Banks.Where(x=>x.Id == bankId)
                             select new
                             {
                                 cr.Id,
                                 cr.BankName,
                                 cr.Code,
                                 cr.CreatedAt,
                                 cr.UpdatedAt
                             };
                if (result.Count() > 0)
                {

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful!", Data = await result.FirstOrDefaultAsync() };
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Bank found!" };

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

        public async Task<GenericResponseModel> updateBankAsync(long bankId, BankRequestModel obj)
        {
            try
            {
                var bankExist = await _context.Banks.Where(x => x.Id == bankId).FirstOrDefaultAsync();

                if (bankExist != null)
                {
                    //Save the Bank
                    bankExist.BankName = obj.BankName;
                    bankExist.Code = obj.Code;
                    bankExist.UpdatedAt = DateTime.Now;

                    await _context.SaveChangesAsync();


                    //return the bank Created
                    var result = from cr in _context.Banks.Where(c => c.Id == bankExist.Id)
                                 select new
                                 {
                                     cr.Id,
                                     cr.BankName,
                                     cr.Code,
                                     cr.CreatedAt,
                                     cr.UpdatedAt
                                 };

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Bank Updated Successfully!", Data = result.FirstOrDefault() };

                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "This Bank Doesn't Exists!" };

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
