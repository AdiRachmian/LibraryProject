using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class Sale
    {
        public SaleType Type;
        public string Input;
        public int DiscountPrecentage;
     
        public Sale(SaleType type, string input, int discountPrecentage)
        {
            Type = type;
            Input = input;
            DiscountPrecentage = discountPrecentage;
        }

        public override string ToString()
        {
            return $"{Type}: {Input}, {DiscountPrecentage}%";
        }
    }
}
