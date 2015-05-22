using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HairBand
{

    //
    // Summary:
    //     Represents a Role entity
    //
    // Type parameters:
    //   TKey:
    public class Role : IItem<Guid>
    {

        //
        // Summary:
        //     Navigation property for claims in the role
        public virtual ICollection<Claim> Claims { get; }
        //
        // Summary:
        //     A random value that should change whenever a role is persisted to the store
        public virtual string ConcurrencyStamp { get; set; }
        //
        // Summary:
        //     Role id
        public virtual Guid Id { get; set; }
        //
        // Summary:
        //     Role name
        public virtual string Name { get; set; }

        public virtual string NormalizedName { get; set; }
        //
        // Summary:
        //     Navigation property for users in the role
        public virtual ICollection<BandMemberRole> Users { get; }

    }
}
