using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HairBand
{

    //
    // Summary:
    //     Entity type for a user's login (i.e. facebook, google)
    //
    // Type parameters:
    //   TKey:
    public class BandMemberLogin
    {

        //
        // Summary:
        //     The login provider for the login (i.e. facebook, google)
        public virtual string LoginProvider { get; set; }
        //
        // Summary:
        //     Display name for the login
        public virtual string ProviderDisplayName { get; set; }
        //
        // Summary:
        //     Key representing the login for the provider
        public virtual string ProviderKey { get; set; }
        //
        // Summary:
        //     User Id for the user who owns this login
        public virtual Guid UserId { get; set; }
    }
}
