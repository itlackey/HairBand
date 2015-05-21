using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HairBand
{
    //
    // Summary:
    //     EntityType that represents a user belonging to a role
    //
    // Type parameters:
    //   TKey:
    public class BandMemberRole : IEquatable<Guid>
    {

        //
        // Summary:
        //     RoleId for the role
        public virtual Guid RoleId { get; set; }
        //
        // Summary:
        //     UserId for the user that is in the role
        public virtual Guid UserId { get; set; }

        public bool Equals(Guid other)
        {
            return RoleId == other;
        }
    }
}
