using DAL.DTO.Req;
using DAL.DTO.Res;
using DAL.Models;
using DAL.Repositories.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Services
{
    public class LoanServices : ILoanServices
    {
        private readonly PeerlandingContext _peerlandingContext;
        public LoanServices(PeerlandingContext peerlandingContext)
        {
            _peerlandingContext = peerlandingContext;
        }
        public async Task<string> CreateLoan(ReqLoanDto loan)
        {
            var newLoan = new MstLoans
            {
                BorrowerId = loan.BorrowerId,
                Amount = loan.Amount,
                InterestRate = loan.InterestRate,
                Duration = loan.Duration,
            };

            await _peerlandingContext.AddAsync(newLoan);
            await _peerlandingContext.SaveChangesAsync();

            return newLoan.BorrowerId;
        }

        public async Task<List<ResListLoanDto>> LoanList(string status)
        {
            var loans = await _peerlandingContext.MstLoans
                .Include(l => l.User)
                .Where(l => status == null || l.Status == status)
                .OrderByDescending(l => l.CreatedAt)
                .Select(loan => new ResListLoanDto
                {
                    LoanId = loan.Id,
                    BorrowerName = loan.User.Name,
                    Amount = loan.Amount,
                    InterestRate = loan.InterestRate,
                    Duration = loan.Duration,
                    Status = loan.Status,
                    CreatedAt = loan.CreatedAt,
                    UpdatedAt = loan.UpdatedAt,
                }).ToListAsync();

            return loans;
        }

        public async Task<string> UpdateStatus(string loanId, ReqStatusLoanDto loan)
        {
            var isThereAnyLoan = await _peerlandingContext.MstLoans.SingleOrDefaultAsync(l => l.Id == loanId);
            if (isThereAnyLoan == null)
            {
                throw new Exception("Loan not found");
            }

            isThereAnyLoan.Status = loan.Status;
            isThereAnyLoan.UpdatedAt = DateTime.UtcNow;

            
            _peerlandingContext.Update(isThereAnyLoan);
            await _peerlandingContext.SaveChangesAsync();
            return loan.Status;
        }

    }
}
