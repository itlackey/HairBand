using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Identity;
using System.Threading;
using System.Security.Claims;

namespace HairBand.Web
{
    public class DefaultUserStore : FileStoreBase<BandMember, Guid>,
        IUserStore<BandMember>,
        IUserPasswordStore<BandMember>,
        IQueryableUserStore<BandMember>,
        IUserPhoneNumberStore<BandMember>,
        IUserTwoFactorStore<BandMember>,
        IUserLoginStore<BandMember>,
        IUserClaimStore<BandMember>,
        IUserRoleStore<BandMember>
    {

        public DefaultUserStore(IHostingEnvironment host)
            : base(host, "_secure/users")
        {
        }

        #region IQueryableUserStore
        public IQueryable<BandMember> Users
        {
            get
            {
                return base.Items.AsQueryable();
            }
        }
        #endregion

        #region IUserStore

        public async Task<IdentityResult> CreateAsync(BandMember user, CancellationToken cancellationToken)
        {
            var path = base.GetFilePathFromUsername(user.UserName);

            if (!String.IsNullOrEmpty(path) ||  this.Users.Contains(user))
            {
                return IdentityResult.Failed(
                        new IdentityError() { Code = "Exists", Description = "User already exists" });
            }


            user.Id = Guid.NewGuid();

            await base.CreateAsync(user);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(BandMember user, CancellationToken cancellationToken)
        {
            if (this.Users.Contains(user))
            {
                await base.DeleteAsync(user);
                return IdentityResult.Success;
            }


            return IdentityResult.Failed(
                    new IdentityError() { Code = "NotFound", Description = "User does not exists" });

        }

        public async Task<BandMember> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var item = await base.GetItemById(userId);

            return item;
        }

        public async Task<BandMember> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var item = await base.GetItemByName(normalizedUserName);

            return item;
        }

        public Task<string> GetNormalizedUserNameAsync(BandMember user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetUserIdAsync(BandMember user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(BandMember user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName.ToLowerInvariant());
        }

        public Task SetNormalizedUserNameAsync(BandMember user, string normalizedName, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.NormalizedUserName = normalizedName);

        }

        public Task SetUserNameAsync(BandMember user, string userName, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.UserName = userName);
        }

        public async Task<IdentityResult> UpdateAsync(BandMember user, CancellationToken cancellationToken)
        {
            try
            {
                var currentUser = await FindByIdAsync(user.Id.ToString(), cancellationToken);

                currentUser.UserName = user.UserName;

                //ToDo map other properties

                await base.SaveItemAsync(user);

                return IdentityResult.Success;
            }
            catch (Exception ex)
            {

                return IdentityResult.Failed(new IdentityError() { Code = ex.HResult.ToString(), Description = ex.Message });
            }

        }
        #endregion

        #region IUserPasswordStore
        public Task SetPasswordHashAsync(BandMember user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;

            return Task.FromResult<object>(null);

            //await base.SaveItemAsync(user);
        }

        public Task<string> GetPasswordHashAsync(BandMember user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(BandMember user, CancellationToken cancellationToken)
        {
            return Task.FromResult(!String.IsNullOrEmpty(user.PasswordHash));
        }
        #endregion

        #region IUserPhoneNumberStore
        public async Task SetPhoneNumberAsync(BandMember user, string phoneNumber, CancellationToken cancellationToken)
        {
            user.PhoneNumber = phoneNumber;
            await base.SaveItemAsync(user);
        }

        public Task<string> GetPhoneNumberAsync(BandMember user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(BandMember user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public async Task SetPhoneNumberConfirmedAsync(BandMember user, bool confirmed, CancellationToken cancellationToken)
        {
            user.PhoneNumberConfirmed = confirmed;
            await base.SaveItemAsync(user);
        }

        #endregion

        #region ITwoFactorUserStore
        public async Task SetTwoFactorEnabledAsync(BandMember user, bool enabled, CancellationToken cancellationToken)
        {
            user.TwoFactorEnabled = enabled;
            await base.SaveItemAsync(user);
        }

        public Task<bool> GetTwoFactorEnabledAsync(BandMember user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.TwoFactorEnabled);
        }

        #endregion

        #region IUserLoginStore
        public async Task AddLoginAsync(BandMember user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            user.Logins.Add(login);
            await base.SaveItemAsync(user);
        }

        public async Task RemoveLoginAsync(BandMember user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            var login = user.Logins.FirstOrDefault(l => l.LoginProvider == loginProvider && l.ProviderKey == providerKey);

            if (login != null)
            {
                user.Logins.Remove(login);
                await base.SaveItemAsync(user);
            }
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(BandMember user, CancellationToken cancellationToken)
        {
            var logins = await Task.Run(() => user.Logins ?? new List<UserLoginInfo>());

            return logins.ToList();
        }

        public Task<BandMember> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {

            var user = base.Items
                .Where(u => u.Logins.Any(l => l.LoginProvider == loginProvider && l.ProviderKey == providerKey))
                .FirstOrDefault();

            return Task.FromResult(user);


        }
        #endregion

        #region IUserClaimsStore
        public Task<IList<Claim>> GetClaimsAsync(BandMember user, CancellationToken cancellationToken)
        {
            return Task.FromResult<IList<Claim>>(user.Claims.ToList());
        }

        public async Task AddClaimsAsync(BandMember user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            foreach (var claim in claims)
            {
                user.Claims.Add(claim);
            }

            await base.SaveItemAsync(user);
        }

        public async Task ReplaceClaimAsync(BandMember user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {

            user.Claims.Remove(claim);
            user.Claims.Add(newClaim);

            await base.SaveItemAsync(user);

        }

        public async Task RemoveClaimsAsync(BandMember user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            foreach (var claim in claims)
            {
                user.Claims.Remove(claim);
            }

            await base.SaveItemAsync(user);
        }

        public Task<IList<BandMember>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            var users = this.Items.Where(u => u.Claims.Contains(claim)).ToList();

            return Task.FromResult<IList<BandMember>>(users);
        }

        #endregion

        #region IUserRoleStore
        public async Task AddToRoleAsync(BandMember user, string roleName, CancellationToken cancellationToken)
        {
            user.Roles.Add(roleName);
            await SaveItemAsync(user);
        }

        public async Task RemoveFromRoleAsync(BandMember user, string roleName, CancellationToken cancellationToken)
        {
            user.Roles.Remove(roleName);
            await SaveItemAsync(user);
        }

        public Task<IList<string>> GetRolesAsync(BandMember user, CancellationToken cancellationToken)
        {
            return Task.FromResult<IList<string>>(user.Roles.ToList());
        }

        public Task<bool> IsInRoleAsync(BandMember user, string roleName, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Roles.Contains(roleName));
        }

        public Task<IList<BandMember>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            return Task.FromResult<IList<BandMember>>(Items.Where(i => i.Roles.Contains(roleName)).ToList());
        }

        #endregion

    }
}
