using CatanClient.AccountService;
using CatanClient.UIHelpers;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CatanClient.Services
{
    public interface IAccountServiceClient
    {

        OperationResultProfileDto IsValidAuthentication(AccountDto account);

        bool VerifyUserAccount(AccountDto account, string token);
        
        Task<bool> CreateAccountInServerAsync(AccountDto account);

        OperationResultChangeRegisterEmailOrPhone ChangeEmailOrPhone(AccountDto account);

        OperationResultChangeRegisterEmailOrPhone ConfirmEmailOrPhone(AccountDto account);

        OperationResultDto ChangePassword(AccountDto account);

        OperationResultDto ConfirmPassword(AccountDto account);
    }
}
