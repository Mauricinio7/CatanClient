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
        Task<OperationResultProfileDto> IsValidAuthenticationAsync(AccountDto account);

        Task<bool> VerifyUserAccountAsync(AccountDto account, string token);

        Task<OperationResultCreateAccountDto> CreateAccountInServerAsync(AccountDto account);

        Task<OperationResultChangeRegisterEmailOrPhone> ChangeEmailOrPhoneAsync(AccountDto account);

        Task<OperationResultChangeRegisterEmailOrPhone> ConfirmEmailOrPhoneAsync(AccountDto account);

        Task<OperationResultDto> ChangePasswordAsync(AccountDto account);

        Task<OperationResultDto> ConfirmPasswordAsync(AccountDto account);

        OperationResultAccountDto GetAccount(ProfileDto profile);

        Task<bool> ResendCodeAsync(AccountDto account);
        Task<bool> SendNeedHelpCode(string email);
        Task<bool> ChangeForotPassword(string email, string newPassword, string verificationCode);
    }
}
