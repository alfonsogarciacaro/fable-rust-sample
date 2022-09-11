module TestApp

open System.IO
open type System.Console

[<EntryPoint>]
let main _args =
    let path = "./data.txt"

    WriteLine("File path: {0}", path)

    // read the whole file as bytes array
    let bytes = File.ReadAllBytes(path)
    WriteLine("File size (in bytes): {0} bytes", bytes.Length)

    // read the whole file as UTF8 string (error if file is not UTF8)
    let text = File.ReadAllText(path)
    WriteLine("File size (in chars): {0} chars", text.Length)

    // read the whole file as string lines array
    let lines = File.ReadAllLines(path)
    WriteLine("File size (in lines): {0} lines", lines.Length)

    // read the file line by line (to lower memory usage on large files)
    WriteLine("\nFile contents:")
    let lines = File.ReadLines(path)
    lines |> Seq.iteri (fun i line ->
        WriteLine("line {0}: {1}", i + 1, line))

    0
