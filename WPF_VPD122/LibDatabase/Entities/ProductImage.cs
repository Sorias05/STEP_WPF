using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibDatabase.Entities
{
    [Table("tblProductImages")]
    public class ProductImage
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(255)]
        public string Name { get; set; }
        public int Priority { get; set; }
        [ForeignKey("ProductEntity")]
        public int ProductId { get; set; }
        public virtual ProductEntity Product { get; set; }
    }
}
