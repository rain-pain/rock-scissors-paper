using System;
using System.Linq;
using System.Security.Cryptography;

namespace task3
{
    class Program
    {
        static void Main(string[] moves)
        {
            if (moves.Length % 2 == 0 || moves.Length < 3 || moves.Distinct().Count() != moves.Count())
            {
                Console.WriteLine("Error. \nRequirements: odd number of parameteres, parameters are not repeated" +
                    "\nExample: rock, paper, scissors, lisard, spock");

                System.Environment.Exit(0);
            }
            else
            {
                string key = GenerateKey();
                string ai_choice = ComputerMove(moves);
               
                Console.WriteLine("HMAC: " + CreateHMAC(key, ai_choice));
                CreateMenu(moves);
                
                int choice = MyMove(moves);
                PrintMoves(moves, choice, ai_choice);

                moves = SortArray(moves, choice);
                Win(moves, ai_choice);
                Console.WriteLine("HMAC key: " + key);
            }
        }

        public static void CreateMenu(string[] moves)
        {
            Console.WriteLine("Available moves:");
            for (int i = 0; i < moves.Length; i++)
            {
                Console.WriteLine((i + 1) + $" - {moves[i]}");
            }
            Console.WriteLine(0 + " - exit");
        }

        public static int MyMove(string[] moves)
        {
            Console.Write("Enter your move: ");
            int choice = Convert.ToInt32(Console.ReadLine());
            if(choice == 0)
            {
                System.Environment.Exit(0);
            }
            return choice;
        }

        public static string ComputerMove(string[] moves)
        {
            Random rnd = new Random();
            int ai = rnd.Next(0, moves.Length) + 1;
            return moves[ai - 1];
        }

        public static void PrintMoves(string [] moves, int choice, string ai_choice)
        {
            Console.WriteLine("Your move: " + moves[choice - 1]);
            Console.WriteLine("Computer move: " + ai_choice);
        }

        public static void Win(string[] moves, string ai_choice)
        {   

            if (Array.IndexOf(moves, ai_choice) == (moves.Length - 1)/2)
            {
                Console.WriteLine("It's a draw");
            }
            else if (Array.IndexOf(moves, ai_choice) > (moves.Length - 1) / 2)
            {
                Console.WriteLine("You win!");
            }
            else
            {
                Console.WriteLine("You lose!");
            }
        }

        public static string[] SortArray(string[] moves, int choice)
        {
            if(choice <= (moves.Length - 1) / 2)
            {
                moves = moves.Skip(choice + (moves.Length - 1) / 2).Concat(moves.Take(choice + (moves.Length - 1) / 2)).ToArray();
            }
            else
            {
                moves = moves.Skip(choice - 1 - (moves.Length - 1) / 2).Concat(moves.Take(choice - 1 - (moves.Length - 1) / 2)).ToArray();
            }
            return moves;
        }

        private static string GenerateKey()
        {
            using (var random = new RNGCryptoServiceProvider())
            {
                var key = new byte[16];
                random.GetBytes(key);
                return ByteToString(key);
            }
        }

        private static string CreateHMAC(string str, string key)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(key);
            byte[] strBytes = encoding.GetBytes(str);

            HMACSHA256 hmac = new HMACSHA256(keyByte);
            byte[] hash = hmac.ComputeHash(strBytes);
            return ByteToString(hash);
        }

        public static string ByteToString(byte[] str)
        {
            string sbinary = "";
            for (int i = 0; i < str.Length; i++)
            {
                sbinary += str[i].ToString("X2");
            }
            return (sbinary);
        }
    }
}
