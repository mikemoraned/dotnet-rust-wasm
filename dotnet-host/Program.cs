using System;
using Wasmtime;

namespace Tutorial
{
    class Program
    {
        const int WASI_ERRNO_SUCCESS = 0;

        static void Main(string[] args)
        {
            using var host = new Host();

            host.DefineFunction(
                "wasi_snapshot_preview1",
                "fd_write",
                (int fd, int iovs, int iovs_len, int nwritten) =>
                {
                    Console.WriteLine("fd_write called");
                    return WASI_ERRNO_SUCCESS;
                }
            );

            using var module = host.LoadModule("../rust-lib/target/wasm32-wasi/release/rust_lib.wasm");
            Console.WriteLine("Loaded");

            using dynamic instance = host.Instantiate(module);
            Console.WriteLine("Instantiated");
            // Console.WriteLine($"gcd(27, 6) = {instance.gcd(27, 6)}");

        }
    }
}