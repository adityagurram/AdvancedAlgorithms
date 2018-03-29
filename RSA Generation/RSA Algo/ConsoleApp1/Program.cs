using System;

namespace ConsoleApp1
{
    class Program
    {
        // fun calculates the GCD of two numbers a $ b
        long euclidGCD(long a, long b) //Calculate	GCD
        {
            if (b == 0)
                return a;
            else
            {
                long rem = a % b;
                return euclidGCD(b, rem);
            }
        }

        // This fn is to calculate the module inverse using extended euclidean algo
        long ExtendedEuclidean(long a, long b)
        {

            long x = 0, prevx = 1, temp;
            while (b != 0)
            {
                long quo = a / b;
                long rem = a % b;

                a = b;
                b = rem;

                temp = x;
                x = prevx - quo * x;
                prevx = temp;
            }

            return prevx;
        }

        // Rabin Miller Algorithm to check for prime
        int IsPrime(long num)
        {
            long subNum;
            if ((num != 2 && num % 2 == 0) || (num < 2))
            {
                return 0;
            }
            else if (num == 2)
            {
                return 1;
            }
            subNum = num - 1;
            while (subNum % 2 == 0)
            {
                subNum /= 2;
            }

            long a = 2, m = subNum;
            long mod = modExpoPower(a, m, num);
            while (m != num - 1 && mod != 1 && mod != num - 1)
            {
                mod = powercalc(mod, 2, num);
                m *= 2;
            }
            if (mod != num - 1 && m % 2 == 0) // breaking cond for prime & check for m bcz whether it has enter atleast 1 time in whileloop if mod 1 it wont, 
            {
                return 0;
            }

            return 1;

        }

        // normal x power n algo 
        long powercalc(long x, long pow, long mod)
        {
            long rem = 1;
            while (pow > 0)
            {
                if (pow % 2 == 0)
                {
                    x = x * x;
                    pow = pow / 2;
                }
                else
                {
                    rem = rem * x;
                    pow = pow - 1;
                }
            }
            return rem % mod;
        }
        // This is modular exponentiation fn to calculate c and m
        long modExpoPower(long baseValue, long powerVal, long mod)
        {
            long x = 1;
            long y = baseValue;
            while (powerVal > 0)
            {
                if (powerVal % 2 == 1)
                    x = (x * y) % mod;
                y = (y * y) % mod;
                powerVal = powerVal / 2;
            }
            return x % mod;
        }

        // string to number format with base 27
        long GetIntUsingBearcat(string msg, int baseval)
        {
            long total = 0;
            for (int i = (int)msg.Length - 1; i >= 0; i--)
            {
                if (msg[i] != ' ')
                {
                    if (msg[i] >= 'A' && msg[i] <= 'Z')
                    {
                        int t = msg[i] - 'A' + 1;
                        long k = 1;
                        for (int j = 1; j < msg.Length - i; j++)
                        {
                            k = k * baseval;
                        }
                        total = total + t * k;
                    }

                }
            }
            return total;
        }

        // once getting number as final output. This fn returns the string out of the integer value.
        string BearcatIntegerToString(long message)
        {
            string output = string.Empty;
            if (message < 27)
            {
                char x1 = Convert.ToChar(65 + message - 1);// 65 is A                
                output = output + x1;
                return output;
            }
            else
            {
                while (message >= 1)
                {
                    long mod = message % 27;
                    message = message / 27;
                    char x1 = Convert.ToChar(65 + mod - 1);
                    output = output + x1;
                }
                return output;
            }
        }

        static void Main(string[] args)
        {

            Program p1 = new Program();
            long ePublicKey, p, q, dPrivateKey;
            Console.WriteLine("\n RSA Encrytpion \n");
            Console.WriteLine("Finding Two Primes P & Q ");
            Random rnd = new Random();
            int prime1 = 0;
            int prime2 = 0;
            do
            {
                p = rnd.Next(5000, 5500);
                prime1 = p1.IsPrime(p);
            } while (prime1 == 0);
            do
            {
                q = rnd.Next(6500, 7000);
                prime2 = p1.IsPrime(q);
            } while (prime2 == 0);

            if (prime1 == 1 && prime2 == 1)
            {
                Console.WriteLine("The prime numbers are  p {0} q {1}", p, q);
            }


            long Nmodulus = p * q;
            long totient_n = (p - 1) * (q - 1);
            Console.WriteLine("The Totient is {0}", totient_n);
            Console.WriteLine("\n Please enter the Integer E");
            ePublicKey = Convert.ToInt64(Console.ReadLine());
            while (p1.euclidGCD(ePublicKey, totient_n) != 1)
            {
                Console.WriteLine("Public Key  and totient are not coprime, please re-enter\n");
                ePublicKey = Convert.ToInt64(Console.ReadLine());
            }
            Console.WriteLine(" Private key {0} Totient {1}", ePublicKey, totient_n);
            Console.WriteLine("please input the Message  ");
            string Message = Console.ReadLine();
            int baseval = 27;
            long MessageVal = p1.GetIntUsingBearcat(Message, baseval);
            Console.WriteLine("Message {0} in Integer {1} with base {2}", Message, MessageVal, baseval);
            dPrivateKey = p1.ExtendedEuclidean(ePublicKey, totient_n);

            if (dPrivateKey < 0)
                dPrivateKey = dPrivateKey + totient_n;
            Console.WriteLine(" The Private Key is {0}", dPrivateKey);
            //Encryption

            long cipher = p1.modExpoPower(MessageVal, ePublicKey, Nmodulus);
            Console.WriteLine("The Encrypted Key C is {0}", cipher);

            //Decryption
            long decrypt = p1.modExpoPower(cipher, dPrivateKey, Nmodulus);
            Console.WriteLine("The Decrypted Key M is {0}", decrypt);

            string revoutput = p1.BearcatIntegerToString(decrypt);
            int strlen = revoutput.Length - 1;
            string output = "";
            while (strlen >= 0)
            {
                output = output + revoutput[strlen];
                strlen--;
            }
            Console.WriteLine("The BearCat Int {1} to string  is {0}", output, MessageVal);
            Console.ReadKey();

        }
    }
}