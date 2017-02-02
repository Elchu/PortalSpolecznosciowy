using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalSpolecznosciowy.Models.ViewModel
{
    public class SingleCommentUser
    {
        public Comment Comment { get; set; }
        public ApplicationUser User { get; set; }
    }
}