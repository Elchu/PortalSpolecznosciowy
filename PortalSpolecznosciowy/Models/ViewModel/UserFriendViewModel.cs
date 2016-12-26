using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalSpolecznosciowy.Models.ViewModel
{
    public class UserFriendViewModel
    {
        public ApplicationUser User { get; set; }
        public bool IsFriend { get; set; }
        public bool IsAccept { get; set; }
        public IEnumerable<ApplicationUser> AllFriendsUser { get; set; }
    }
}