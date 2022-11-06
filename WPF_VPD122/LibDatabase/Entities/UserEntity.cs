using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibDatabase.Entities
{
    [Table("tblUsers")]
    public class UserEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(150)]
        public string Name { get; set; }
        [StringLength(20)]
        public string Phone { get; set; }
        public Gender Gender { get; set; }
        [StringLength(255)]
        public string Image { get; set; }
        [StringLength(200)]
        public string Password { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}
