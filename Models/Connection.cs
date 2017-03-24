using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeltS.Models

{
    public class Connection : BaseEntity
    {
        [Key]
        public int ConnecId { get; set; }
        [ForeignKey("UserId")]
        public int CurUserId { get; set; } //the user who is primary current
        [ForeignKey("UserId")]
        public int NetUserId {get; set;} //the user you want to network with
        [InverseProperty("Connections")]
        public User CurUser {get; set;}
        public User NetUser {get; set;}
        public string Status {get; set;}//state of user relationship
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        
        public Connection()
        {
            CurUser = new User();
            NetUser = new User();
        }
    }
}