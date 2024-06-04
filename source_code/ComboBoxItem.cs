using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1
{

    public class ComboBoxItem<T>
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public T Value { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
