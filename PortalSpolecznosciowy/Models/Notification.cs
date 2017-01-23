using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PortalSpolecznosciowy.Models
{
    public class Notification
    {
        [Key]
        [Display(Name = "Id")]
        public int NotificationId { get; set; }

        [Display(Name = "Od kogo ID")]
        [Column(TypeName = "NVARCHAR")]
        [StringLength(128)]
        public string FromWhoId { get; set; }

        [Display(Name = "Od kogo")]
        public string FullNameFromWho { get; set; }

        [Display(Name = "Do kogo ID")]
        [Column(TypeName = "NVARCHAR")]
        [StringLength(128)]
        public string ToWhomId { get; set; }

        [Display(Name = "Do kogo")]
        public string FullNameToWhom { get; set; }

        [Display(Name = "Post ID")]
        public int? PostId { get; set; }

        [Display(Name = "Komentarz ID")]
        public int? CommentId { get; set; }

        [Display(Name = "Wiadomość")]
        public string Message { get; set; }

        [Display(Name = "Data dodania")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "Odbiorca przeczytał")]
        public bool RecipientHasRead { get; set; }

        [Display(Name = "Nadawca powiadomiony")]
        public bool SenderInformed { get; set; }


    }
}