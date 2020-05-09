## Prerequisites

wasm-pack

## Build

    wasm-pack build --scope mike_moran --release

## Inspect

Exports:

    wasm2wat target/wasm32-unknown-unknown/release/rust_lib.wasm | grep "export"

Imports:

    wasm2wat target/wasm32-unknown-unknown/release/rust_lib.wasm | grep "import"
