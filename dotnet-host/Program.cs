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
                (instance as dynamic).reverse(8, inputAddress, inputLength);

                Console.WriteLine("Invoked");

                // from wasm bindgen:
                // 
                // var r0 = getInt32Memory0()[8 / 4 + 0];
                // var r1 = getInt32Memory0()[8 / 4 + 1];

                var outputAddress = memory.ReadInt32(32 * (8 / 4 + 0));
                var outputLength = memory.ReadInt32(32 * (8 / 4 + 1));

                Console.WriteLine("{0}, {1}", outputAddress, outputLength);
                try
                {
                    for (int address = 0; address < outputAddress + outputLength; address += 8)
                    {
                        Console.WriteLine(memory.ReadByte(address));
                    }
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