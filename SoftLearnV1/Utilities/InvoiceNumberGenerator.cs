using SoftLearnV1.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Utilities
{
    public class InvoiceNumberGenerator
    {
        private readonly AppDbContext dbContext;

        public InvoiceNumberGenerator(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public long GetInvoiceNumber(long schoolId)
        {
            long NewInvoiceRef = 200000;
            var checkIsTableEmpty = from invoice in dbContext.InvoiceTotal
                                    where invoice.SchoolId == schoolId
                                    select invoice;
            if (checkIsTableEmpty.Count() > 0)
            {
                var maxValue = dbContext.InvoiceTotal.Where(x => x.SchoolId == schoolId).Max(x => x.InvoiceCode);
                NewInvoiceRef = Convert.ToInt64(maxValue) + 1;
            }
            else
            {
                long newInvoice;
                newInvoice = NewInvoiceRef;
            }

            return NewInvoiceRef;
        }
    }
}
