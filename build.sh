dotnet tool restore
dotnet fable clean --lang rust -e .rs --yes
dotnet fable --lang rust -e .rs -c Release --watch --runWatch cargo run
