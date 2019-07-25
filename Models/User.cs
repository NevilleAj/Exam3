using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using exam3.Models;

namespace exam3
{
    public class User 
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserAlias { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Like> UserLikes { get; set; }
        public List<Idea> UsersIdeas { get; set; }
    }
}