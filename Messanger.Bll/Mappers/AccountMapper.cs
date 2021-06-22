using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messanger.Bll.Contracts.User;
using Messanger.Dal.Entities;

namespace Messanger.Bll.Mappers
{
    public static class AccountMapper
    {
        public static Account Map(AccountDto obj)
        {
            return obj == null
                ? null
                : new Account
                {
                    Id = obj.Id,
                    Login = obj.Login,
                    PasswordHash = obj.PasswordHash,
                    AccountCreationTime = obj.AccountCreationTime,
                    AccountType = obj.AccountType,
                    AvatarLink = obj.AvatarLink,
                    PhoneNumber = obj.MobilePhone
                };
        }

        public static AccountDto Map(Account obj)
        {
            return obj == null
                ? null
                : new AccountDto
                {
                    Id = obj.Id,
                    Login = obj.Login,
                    PasswordHash = obj.PasswordHash,
                    AccountCreationTime = obj.AccountCreationTime,
                    AccountType = obj.AccountType,
                    AvatarLink = obj.AvatarLink,
                    MobilePhone = obj.PhoneNumber
                };
        }
        public static AccountViewDto MapForView(Account obj)
        {
            return obj == null
                ? null
                : new AccountViewDto
                {
                    Id = obj.Id,
                    Login = obj.Login,
                    CreationTime = obj.AccountCreationTime,
                    MobilePhone = obj.PhoneNumber
                };
        }
    }
}
