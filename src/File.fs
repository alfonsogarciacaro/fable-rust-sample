module System.IO.File

#if FABLE_COMPILER_RUST
open Fable.Core.Rust

let imports() =
    import "std::io::{BufRead}" ""
    ()

open std

let ReadAllBytes (path: string): byte[] =
    match fs.read(path) with
    | Error e -> failwith $"File error: {e}"
    | Ok bytes -> bytes |> toArray

// throws an error if file encoding is not UTF8
let ReadAllText (path: string): string =
    match fs.read_to_string(path) with
    | Error e -> failwith $"File error: {e}"
    | Ok text -> toString &text

let ReadAllLines (path: string): string[] =
    match fs.read_to_string(path) with
    | Error e -> failwith $"File error: {e}"
    | Ok text -> text |> allLines |> toArray

let private unwrap (res: Result<String, String>) =
    match res with
    | Error e -> failwith $"File error: {e}"
    | Ok line -> toString &line

let private readLines (path: string): Result<string seq, String> =
    let file = fs.File.open_(path)
    let reader = io.BufReader.new_(file)
    let xs = reader.lines() |> iter_to_seq |> Seq.map unwrap
    Ok(xs)

let ReadLines (path: string): string seq =
    match readLines path with
    | Error e -> failwith $"File error: {e}"
    | Ok xs -> xs

#endif //FABLE_COMPILER_RUST
