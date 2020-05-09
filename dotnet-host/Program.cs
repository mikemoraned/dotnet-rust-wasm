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
                "__wbindgen_placeholder__",
                "__wbindgen_describe",
                (int ignore1) =>
                {
                    Console.WriteLine("__wbindgen_describe called");
                }
            );
            host.DefineFunction(
                "__wbindgen_anyref_xform__",
                "__wbindgen_anyref_table_grow",
                (int ignore) =>
                {
                    Console.WriteLine("__wbindgen_anyref_table_grow called");
                    return WASI_ERRNO_SUCCESS;
                }
            );
            host.DefineFunction(
                "__wbindgen_anyref_xform__",
                "__wbindgen_anyref_table_set_null",
                (int ignore) =>
                {
                    Console.WriteLine("__wbindgen_anyref_table_set_null called");
                }
            );

            using var module = host.LoadModule("../rust-lib/target/wasm32-unknown-unknown/release/rust_lib.wasm");
            Console.WriteLine("Loaded");

            using dynamic instance = host.Instantiate(module);
            Console.WriteLine("Instantiated");
            // Console.WriteLine($"gcd(27, 6) = {instance.gcd(27, 6)}");

        }
    }
}