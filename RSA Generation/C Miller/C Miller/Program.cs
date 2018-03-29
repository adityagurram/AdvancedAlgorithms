using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Miller
{
    class Program
    {
        long  mulmod(long  a, long  b, long  mod)
        {
            long  x = 0, y = a % mod;
            while (b > 0)
            {
                if (b % 2 == 1)
                {
                    x = (x + y) % mod;
                }
                y = (y * 2) % mod;
                b /= 2;
            }
            return x % mod;
        }
        /* 
         * modular exponentiation
         */
        long  modulo(long  base1, long  exponent,  long mod)
        {
            long  x = 1;
            long  y = base1;
            while (exponent > 0)
            {
                if (exponent % 2 == 1)
                    x = (x * y) % mod;
                y = (y * y) % mod;
                exponent = exponent / 2;
            }
            return x % mod;
        }

        /*
         * Miller-Rabin Primality test, iteration signifies the accuracy
         */
        int Miller(long  p)
        {

            int i;
            long  s;
            if (p < 2)
            {
                return 0;
            }
            if (p != 2 && p % 2 == 0)
            {
                return 0;
            }
            s = p - 1;
            while (s % 2 == 0)
            {
                s /= 2;
            }
            
                long  a = 2, temp = s;
                long  mod = modulo(a, temp, p);
                while (temp != p - 1 && mod != 1 && mod != p - 1)
                {
                    mod = mulmod(mod, mod, p);
                    temp *= 2;
                }
                if (mod != p - 1 && temp % 2 == 0)
                {
                    return 0;
                }
            
            return 1;
        }
        //Main
        static void Main(string[] args)
        {
            Program p1 = new Program();           
                int res = p1.Miller(127);
            Console.ReadKey();
            
        }
    }
}
