using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppSimple.Models
{
    public class MyComboBoxItem
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
