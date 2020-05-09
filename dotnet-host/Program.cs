using System;
using Wasmtime;

namespace Tutorial
{
    class Program
    {
        static void Main(string[] args)
        {
            using var host = new Host();
            using var module = host.LoadModuleText("gcd.wat");

            using dynamic instance = host.Instantiate(module);
            Console.WriteLine($"gcd(27, 6) = {instance.gcd(27, 6)}");
        }
    }
}