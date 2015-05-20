using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Identity;
using Microsoft.Framework.Logging;
using Microsoft.Framework.OptionsModel;

namespace HairBand.Web
{
    public class DefaultUserManager : UserManager<HairBandMember>
    {
        public DefaultUserManager(IUserStore<HairBandMember> store, 
            IOptions<IdentityOptions> optionsAccessor, 
            IPasswordHasher<HairBandMember> passwordHasher, 
            IEnumerable<IUserValidator<HairBandMember>> userValidators,
            IEnumerable<IPasswordValidator<HairBandMember>> passwordValidators, 
            ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, 
            IEnumerable<IUserTokenProvider<HairBandMember>> tokenProviders, 
            ILoggerFactory logger, IHttpContextAccessor contextAccessor) 
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, tokenProviders, logger, contextAccessor)
        {
        }

        public override Task<IdentityResult> CreateAsync(HairBandMember user)
        {
            return base.CreateAsync(user);
        }
    }
}
