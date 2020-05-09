## Prerequisites

    cargo install cargo-wasi

## Build

    cargo wasi build --release

## Test

    cargo test

## Inspect

Exports:

    wasm2wat target/wasm32-wasi/release/rust_lib.wasm --enable-all | grep "export"

Imports:

    wasm2wat target/wasm32-wasi/release/rust_lib.wasm --enable-all | grep "import"
