using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNet.Hosting;
using System.IO;
using Newtonsoft.Json;

namespace HairBand.Web
{
    public class DefaultUserStore : IUserStore<BandMember>, IUserPasswordStore<BandMember>, IQueryableUserStore<BandMember>
    {

       
        public DefaultUserStore(IHostingEnvironment host)
        {
            this._host = host;
            this._rootPath = this._host.MapPath("App_Data/secure");
            _filePath = Path.Combine(this._rootPath, "user.json");
        }



        #region IQueryableUserStore
        public IQueryable<BandMember> Users
        {
            get
            {
                if (this._users == null)
                {
                    this._users = GetBandMembers();
                }
                return _users.AsQueryable();
            }
        }
        #endregion

        #region IUserStore

        public Task<IdentityResult> CreateAsync(BandMember user, CancellationToken cancellationToken)
        {
            if (this.Users.Contains(user))
            {
                return Task.FromResult(
                    IdentityResult.Failed(
                        new IdentityError() { Code = "Exists", Description = "User already exists" }));
            }

            this._users.Add(user);
            SaveBandMembers();
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> DeleteAsync(BandMember user, CancellationToken cancellationToken)
        {
            if (this.Users.Contains(user))
            {

                this._users.Remove(user);
                SaveBandMembers();
                return Task.FromResult(IdentityResult.Success);
            }


            return Task.FromResult(
                IdentityResult.Failed(
                    new IdentityError() { Code = "NotFound", Description = "User does not exists" }));

        }

        public Task<BandMember> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var user = _users.FirstOrDefault(u => u.Id.ToString() == userId);

            return Task.FromResult(user);
        }

        public Task<BandMember> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var user = _users.FirstOrDefault(u => u.UserName.ToLowerInvariant() == normalizedUserName);

            return Task.FromResult(user);
        }

        public Task<string> GetNormalizedUserNameAsync(BandMember user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName.ToLowerInvariant());
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
            return Task.Run(() => user.UserName = normalizedName);
    
        }

        public Task SetUserNameAsync(BandMember user, string userName, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.UserName = userName);
        }

        public async Task<IdentityResult> UpdateAsync(BandMember user, CancellationToken cancellationToken)
        {
            var currentUser = await FindByIdAsync(user.Id.ToString(), cancellationToken);

            currentUser.UserName = user.UserName;

            SaveBandMembers();

            return IdentityResult.Success;

        }
        #endregion

        #region IUserPasswordStore
        public Task SetPasswordHashAsync(BandMember user, string passwordHash, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPasswordHashAsync(BandMember user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasPasswordAsync(BandMember user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls
      
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~DefaultUserStore() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }


        #endregion

        #region Private Members

        private string _filePath;

        private IHostingEnvironment _host;
        private string _rootPath;
        private List<BandMember> _users;


        private List<BandMember> GetBandMembers()
        {

            var data = File.ReadAllText(_filePath);

            var users = JsonConvert.DeserializeObject<BandMember[]>(data);

            return users.ToList();

        }

        private void SaveBandMembers()
        {
            var data = JsonConvert.SerializeObject(this._users);
            File.WriteAllText(_filePath, data);
        }

        #endregion



    }
}
