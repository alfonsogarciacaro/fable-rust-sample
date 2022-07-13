module TestApp
open type System.Console

open Fable.Core

// mini Rust std lib bindings (just an example, not guaranteed to be stable)
module std =

    [<AutoOpen>]
    module string =
        [<Erase; Emit("String")>]
        type String =
            [<Emit("String::from($0.as_ref())")>]
            static member from(s: string): String = nativeOnly

    [<AutoOpen>]
    module vec =
        [<Erase; Emit("Vec")>]
        type Vec<'T> =
            [<Emit("Vec::new()")>]
            static member new_(): Vec<'T> = nativeOnly

    module io =
        [<Erase; Emit("std::io::Error")>]
        type Error =
            [<Emit("std::io::Error::last_os_error()")>]
            static member last_os_error(): Error = nativeOnly
            [<Emit("$0.raw_os_error()")>]
            member _.raw_os_error(): int option = nativeOnly

        [<Erase; Emit("std::io::BufReader")>]
        type BufReader<'T> =
            [<Emit("std::io::BufReader::new($0)")>]
            static member new_(inner: 'T): BufReader<'T> = nativeOnly

            [<Emit("$0.lines().map(|res| res.map(|s| string(&s))).collect::<Result<Vec<_>, _>>().map(array)")>]
            member _.lines(): Result<string[], Error> = nativeOnly

    module fs =
        [<Erase; Emit("std::fs::File")>]
        type File =
            [<Emit("std::fs::File::open($0.as_ref())")>]
            static member open_(path: string): Result<File, io.Error> = nativeOnly

        [<Emit("std::fs::read($0.as_ref()).map(array)")>]
        let read(path: string): Result<byte[], io.Error> = nativeOnly

        [<Emit("std::fs::read_to_string($0.as_ref()).map(|s| string(&s))")>]
        let read_to_string(path: string): Result<string, io.Error> = nativeOnly

[<AutoOpen>]
module Imports =
    open std

    module Native =
        [<Import("String_::string", "fable_library_rust")>]
        let toString (v: String inref): string = nativeOnly

        [<Import("Native_::array", "fable_library_rust")>]
        let toArray (v: Vec<'T>): 'T[] = nativeOnly

    module Imports =
        [<Emit("()")>]
        let emitUnit args: unit = nativeOnly

        let inline private import selector path =
            emitUnit (Rust.import selector path)

        // this adds some imports used in the bindings above
        let imports(): unit =
            import "Native_::array" "fable_library_rust"
            import "String_::string" "fable_library_rust"
            import "std::io::BufRead" ""

open std

[<EntryPoint>]
let main _args =
    let path = "./data.txt"

    WriteLine("\nFile {0}:", path)

    // read the whole file as UTF8 string (error if file is not UTF8)
    match fs.read_to_string(path) with
    | Error e -> WriteLine("File error: {0:?}", e)
    | Ok text -> WriteLine(text)

    // read the whole file as bytes
    match fs.read(path) with
    | Error e ->  WriteLine("File error: {0:?}", e)
    | Ok bytes -> WriteLine("File size: {0} bytes", bytes.Length)

    // read file line by line
    match fs.File.open_(path) with
    | Error e ->
        WriteLine("File error: {0:?}", e)
    | Ok file ->
        let reader = io.BufReader.new_(file)
        match reader.lines() with
        | Error e -> WriteLine("File error: {0:?}", e)
        | Ok lines ->
            WriteLine("File size: {0} lines", lines.Length)
            for i=1 to lines.Length do
                WriteLine("Line {0}: {1}", i, lines[i-1])
    0