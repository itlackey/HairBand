using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNet.Hosting;

namespace HairBand.Web
{
    public class DefaultRoleStore : FileStoreBase<Role, Guid>, IRoleStore<Role>
    {
        public DefaultRoleStore(IHostingEnvironment host)
            : base(host, "secure/roles")
        {

        }



        public async Task<IdentityResult> CreateAsync(Role role, CancellationToken cancellationToken)
        {
            try
            {
                role.Id = Guid.NewGuid();

                var item = await base.CreateAsync(role);

                return IdentityResult.Success;
            }
            catch (InvalidOperationException)
            {
                return IdentityResult.Failed(new IdentityError() { Description = "Role already exists" });
            }
        }

        public async Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancellationToken)
        {
            try
            {
                await base.DeleteAsync(role);

                return IdentityResult.Success;
            }
            catch (KeyNotFoundException)
            {
                return IdentityResult.Failed(new IdentityError() { Description = "Role not found" });
            }
        }

        public async Task<Role> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            var role = await base.GetItemById(roleId);

            return role;
        }

        public async Task<Role> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            var role = await base.GetItemByName(normalizedRoleName);

            return role;
        }

        public Task<string> GetNormalizedRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.NormalizedName);
        }

        public Task<string> GetRoleIdAsync(Role role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id.ToString());
        }

        public Task<string> GetRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public async Task SetNormalizedRoleNameAsync(Role role, string normalizedName, CancellationToken cancellationToken)
        {

            role.NormalizedName = normalizedName;
            await base.SaveItemAsync(role);

        }

        public async Task SetRoleNameAsync(Role role, string roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;
            await base.SaveItemAsync(role);
        }

        public async Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancellationToken)
        {
            var currentRole = await GetItemById(role.Id.ToString());

            if (currentRole == null)
                return IdentityResult.Failed(new IdentityError() { Description = "Role not found" });

            currentRole.Claims.Clear();
            foreach (var item in role.Claims)
                currentRole.Claims.Add(item);

            currentRole.ConcurrencyStamp = role.ConcurrencyStamp;

            //ToDo finish this...

            await SaveItemAsync(currentRole);

            return IdentityResult.Success;
        }
    }
}
