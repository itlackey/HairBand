using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HairBand
{
    //
    // Summary:
    //     EntityType that represents one specific user claim
    //
    // Type parameters:
    //   TKey:

    public class Claim //: IEquatable<Guid>
    {

        //
        // Summary:
        //     Claim type
        public virtual string ClaimType { get; set; }
        //
        // Summary:
        //     Claim value
        public virtual string ClaimValue { get; set; }
        //
        // Summary:
        //     Primary key
        public virtual int Id { get; set; }
        //
        // Summary:
        //     User Id for the user who owns this claim
        public virtual Guid OwnerId { get; set; }


    }

}
