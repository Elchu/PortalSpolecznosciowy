using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalSpolecznosciowy.Models.ViewModel
{
    public class SinglePostUser
    {
        public Post Post { get; set; }
        public ApplicationUser User { get; set; }
    }
}