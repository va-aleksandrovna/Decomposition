using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decomposition
{
    public class Factorization
    {
        public ulong Number { get; set; } // число для разложения
        public List<ulong> Factors = new List<ulong>(); // для хранения разложения

        public Factorization(ulong number)
        {
            Number = number;
        }

        public void Factorize() // разложение
        {
            while (Number > 1)
            {
                for (ulong i = 2; i <= Number; i++)
                {
                    if (Number % i == 0)
                    {
                        Factors.Add(i);
                        Number /= i;
                        break;
                    }
                }
            }
        }

        public string GetFactors() // генерирует строку, которая в читаемом виде содержит разложение
        {
            StringBuilder sb = new StringBuilder();
            foreach (ulong factor in Factors)
            {
                sb.Append(factor);
                sb.Append("*");
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        public ulong GetNumber() // вычисление числа по разложению
        {
            ulong number = 1;
            foreach (ulong factor in Factors)
            {
                number *= factor;
            }
            return number;
        }
    }
}
