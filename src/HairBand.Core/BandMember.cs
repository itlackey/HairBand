using System;
using System.Collections.Generic;

namespace HairBand
{
    public class BandMember : IItem<Guid>, IEquatable<Guid>
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

        //Guid IItem<Guid>.Id
        //{
        //    get
        //    {
        //        throw new NotImplementedException();
        //    }

        //    set
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        string IItem<Guid>.Name
        {
            get
            {
                return UserName;
            }

            set
            {
                UserName = value;
            }
        }

        public bool Equals(Guid other)
        {
           return Id == other;
        }
    }












  
}