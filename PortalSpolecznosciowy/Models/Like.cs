using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PortalSpolecznosciowy.Models
{
    public class Like
    {
        [Key]
        [Display(Name = "ID")]
        public int LikeId { get; set; }

        [Required]
        [Display(Name = "Użytkownik")]
        [Column(TypeName = "NVARCHAR")]
        [StringLength(128)]
        public string UserId { get; set; }

        [Display(Name = "Post")]
        public int? PostId { get; set; }

        [Display(Name = "Komentarz")]
        public int? CommentId { get; set; }

        [Display(Name = "Data dodania")]
        public DateTime Data { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Post Post { get; set; }
        public virtual Comment Comment { get; set; }
    }
}