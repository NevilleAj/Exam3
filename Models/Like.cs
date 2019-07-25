using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using exam3.Models;

namespace exam3
{
    public class Like 
    {
        [Key]
        public int LikeId { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int IdeaId { get; set; }
        public Idea Idea { get; set; }
    }
}