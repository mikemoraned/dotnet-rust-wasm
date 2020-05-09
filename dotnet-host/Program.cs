using System;
using Wasmtime;
using System.Linq;

namespace DotnetHost
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

            using var instance = host.Instantiate(module);
            Console.WriteLine("Instantiated");

            var memory = instance.Externs.Memories.SingleOrDefault() ??
                throw new InvalidOperationException("Module must export a memory.");

            var allocator = new Allocator(memory, instance.Externs.Functions);

            (var inputAddress, var inputLength) = allocator.AllocateString("foo");

            try
            {
                object[] results = (instance as dynamic).reverse(inputAddress, inputLength);

                var outputAddress = (int)results[0];
                var outputLength = (int)results[1];

                try
                {
                    Console.WriteLine(memory.ReadString(outputAddress, outputLength));
                }
                finally
                {
                    allocator.Free(outputAddress, outputLength);
                }
            }
            finally
            {
                allocator.Free(inputAddress, inputLength);
            }
        }
    }
}