using ColtSmart.Encrypt;
using System;

namespace ConsoleApp1.test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var s= EncryptHelper.Instance.PassEncryption("guest123", "654321");
            Console.WriteLine(s);
            Console.ReadLine();
        }
    }
}
