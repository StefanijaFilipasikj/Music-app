using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MusicPlatform.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        public int Song_Id { get; set; }
        public virtual Song Song { get; set; }

        [Required]
        [StringLength(300)]
        public string Content { get; set; }
        
        public string Email { get; set; }
        public string PhotoURL { get; set; }

        [ForeignKey("ParentComment")]
        public int? ParentCommentId { get; set; }
        public Comment ParentComment { get; set; }

        public virtual ICollection<Comment> Replies { get; set; }

        public Comment()
        {
            Replies = new List<Comment>();
        }
    }
}