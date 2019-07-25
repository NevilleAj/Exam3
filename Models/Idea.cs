using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using exam3.Models;

namespace exam3
{
    public class Idea 
    {
        [Key]
        public int IdeaId { get; set; }
        public int UserId { get; set; }
        public string UserIdea { get; set; }
        public User Owner { get; set; }
        public List<Like> LikedBy { get; set; }
        
    }
}