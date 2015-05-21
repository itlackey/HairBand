using System;
using System.Collections.Generic;

namespace HairBand.Web
{
    public class BandMember : IEquatable<Guid>
    {
        public BandMember()
        {

        }

        //
        // Summary:
        //     Used to record failures for the purposes of lockout
        public virtual int AccessFailedCount { get; set; }
        //
        // Summary:
        //     Navigation property for users claims
        public virtual ICollection<Claim> Claims { get; }
        //
        // Summary:
        //     A random value that should change whenever a user is persisted to the store
        public virtual string ConcurrencyStamp { get; set; }
        //
        // Summary:
        //     Email
        public virtual string Email { get; set; }
        //
        // Summary:
        //     True if the email is confirmed, default is false
        public virtual bool EmailConfirmed { get; set; }
        public virtual Guid Id { get; set; }
        //
        // Summary:
        //     Is lockout enabled for this user
        public virtual bool LockoutEnabled { get; set; }
        //
        // Summary:
        //     DateTime in UTC when lockout ends, any time in the past is considered not locked
        //     out.
        public virtual DateTimeOffset? LockoutEnd { get; set; }
        //
        // Summary:
        //     Navigation property for users logins
        public virtual ICollection<BandMemberLogin> Logins { get; }

        public virtual string NormalizedEmail { get; set; }
        public virtual string NormalizedUserName { get; set; }
        //
        // Summary:
        //     The salted/hashed form of the user password
        public virtual string PasswordHash { get; set; }
        //
        // Summary:
        //     PhoneNumber for the user
        public virtual string PhoneNumber { get; set; }
        //
        // Summary:
        //     True if the phone number is confirmed, default is false
        public virtual bool PhoneNumberConfirmed { get; set; }
        //
        // Summary:
        //     Navigation property for users in the role
        public virtual ICollection<BandMemberRole> Roles { get; }
        //
        // Summary:
        //     A random value that should change whenever a users credentials change (password
        //     changed, login removed)
        public virtual string SecurityStamp { get; set; }
        //
        // Summary:
        //     Is two factor enabled for the user
        public virtual bool TwoFactorEnabled { get; set; }
        public virtual string UserName { get; set; }

        public bool Equals(Guid other)
        {
           return Id == other;
        }
    }


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




    //
    // Summary:
    //     Represents a Role entity
    //
    // Type parameters:
    //   TKey:
    public class Role
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