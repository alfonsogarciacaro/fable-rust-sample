git clean -fdX
dotnet tool restore
#dotnet run -c Release --project ../Fable/src/Fable.Cli -- --lang Rust --outDir . --noCache
dotnet fable --lang Rust --outDir .
cargo fmt
cargo build
