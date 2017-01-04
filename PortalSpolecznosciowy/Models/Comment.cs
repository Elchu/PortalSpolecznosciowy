using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PortalSpolecznosciowy.Models
{
    public class Comment
    {
        [Key]
        [Display(Name = "ID")]
        public int CommentId { get; set; }

        [Required]
        [Display(Name = "Użytkownik")]
        [Column(TypeName = "NVARCHAR")]
        [StringLength(128)]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "Post ID")]
        public int PostId { get; set; }

        [Required]
        [Column(TypeName = "text")]
        [Display(Name = "Treść")]
        public string Content { get; set; }

        [Display(Name = "Data dodania")]
        public DateTime Date { get; set; }

        [Display(Name = "Czy usunięty")]
        public bool IsDeleted { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Post Post { get; set; }
    }
}