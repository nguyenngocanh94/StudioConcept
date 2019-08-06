using System;
using System.Collections.Generic;
using System.Linq;

namespace HackerRank
{
    class Program
    {
        public static Stack<int> inStack = new Stack<int>();
        public static int count;
        static void Main(string[] args)
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            int n = int.Parse(Console.ReadLine());
           
            for (int i = 0; i < n; i++)
            {
                string command = Console.ReadLine();
                switch (command[0])
                {
                    case '1':
                        Enqueue(int.Parse(command.Substring(2, command.Length - 2)));
                        break;
                    case '2':
                        Dequeue();
                        break;
                    case '3':
                        print();
                        break;
                        
                }
            }
        }

        static void Enqueue(int number)
        {
            inStack.Push(number);
        }

        static void Dequeue()
        {
            count++;
        }

        static void print()
        {
            Console.WriteLine(inStack.ElementAt(inStack.Count-count-1));
        }
    }
}
