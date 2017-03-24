using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace BeltS.Models

{
    public class User: BaseEntity
    {
        [Key]
        public int UserId { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        [Display(Name = "Description")]
        public string Description {get; set;}
        [InverseProperty("CurUser")]
        public List<Connection> Connections {get; set;}

        public User(){
            Connections = new List<Connection>();
        }
    }
}