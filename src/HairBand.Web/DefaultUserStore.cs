using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Identity;
using System.Threading;

namespace HairBand.Web
{
    public class DefaultUserStore : FileStoreBase<BandMember, Guid>,
        IUserStore<BandMember>,
        IUserPasswordStore<BandMember>,
        IQueryableUserStore<BandMember>
    {

        public DefaultUserStore(IHostingEnvironment host)
            : base(host, "secure/users")
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
            if (this.Users.Contains(user))
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
        public async Task SetPasswordHashAsync(BandMember user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            await base.SaveItemAsync(user);
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

    }
}
