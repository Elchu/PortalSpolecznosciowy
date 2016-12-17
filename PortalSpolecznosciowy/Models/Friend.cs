﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PortalSpolecznosciowy.Models
{
    public class Friend
    {
        [Key]
        [Display(Name = "Id")]
        public int FriendId { get; set; }

        [Index("IX_UserId", 1, IsUnique = true)]
        [Display(Name = "Użytkownik")]
        public string UserId { get; set; }

        [Index("IX_UserFriendId", 2, IsUnique = true)]
        [Display(Name = "Przyjaciel")]
        public string UserFriendId { get; set; }

        [DefaultValue (false)]
        [Display(Name = "Zaakceptowane")]
        public bool Accepted { get; set; }
        [Display(Name = "Data dodania")]
        public DateTime Time { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}