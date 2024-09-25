using DAL.DTO.Req;
using DAL.DTO.Res;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Services.Interfaces
{
    public interface ILoanServices
    {
        Task<string> CreateLoan(ReqLoanDto loan);
        Task<string> UpdateStatus(string loanId, ReqStatusLoanDto loan);
        Task<List<ResListLoanDto>> LoanList(string status);
    }
}
