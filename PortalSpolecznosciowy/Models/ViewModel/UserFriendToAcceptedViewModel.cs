using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalSpolecznosciowy.Models.ViewModel
{
    public class UserFriendToAcceptedViewModel
    {
        public ApplicationUser User { get; set; }
        public List<UserFriendTemp> FriendsToAccepted { get; set; }

        public IEnumerable<ApplicationUser> AllFriendsUser { get; set; }
    }
}