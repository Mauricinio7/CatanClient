using CatanClient.AccountService;
using CatanClient.GuestAccountService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatanClient.Services
{
    internal interface IGuestAccountServiceClient
    {
        Task<OperationResultGuestAccountDto> LoginAsGuestAsync(string language);
    }
}
