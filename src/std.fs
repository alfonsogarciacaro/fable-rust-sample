module std

// mini Rust std lib bindings (just an example, not guaranteed to be stable)

#if FABLE_COMPILER_RUST

open Fable.Core

[<AutoOpen>]
module string =
    [<Erase; Emit("String")>]
    type String =
        [<Emit("String::from($0.as_str())")>]
        member _.from(s: string): String = nativeOnly

[<AutoOpen>]
module vec =
    [<Erase; Emit("Vec<_>")>]
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

    [<Erase; Emit("std::io::BufReader<_>")>]
    type BufReader<'T> =
        [<Emit("std::io::BufReader::new($0)")>]
        static member new_(inner: 'T): BufReader<'T> = nativeOnly

        [<Emit("$0.lines().map(|res| res.map_err(|e| e.to_string()))")>]
        member _.lines() = nativeOnly

module fs =
    [<Struct>]
    [<Erase; Emit("std::fs::File")>]
    type File =
        [<Emit("std::fs::File::open($0.as_str()).map_err(|e| e.to_string())?")>]
        static member open_(path: string) = nativeOnly

    [<Emit("std::fs::read($0.as_str()).map_err(|e| e.to_string())")>]
    let read(path: string): Result<Vec<byte>, String> = nativeOnly

    [<Emit("std::fs::read_to_string($0.as_str()).map_err(|e| e.to_string())")>]
    let read_to_string(path: string): Result<String, String> = nativeOnly

[<AutoOpen>]
module Native =

    [<Emit("$0.map_err(|e| e.to_string())")>]
    let toStringError (s: Result<'T, io.Error>): Result<'T, String> = nativeOnly

    [<Emit("$0.lines().map(toString).collect()")>]
    let allLines (s: String): Vec<string> = nativeOnly

    [<Import("String_::toString", "fable_library_rust")>]
    let toString (v: String inref): string = nativeOnly

    [<Import("NativeArray_::array_from", "fable_library_rust")>]
    let toArray (v: Vec<'T>): 'T[] = nativeOnly

    [<Import("Native_::iter_to_seq", "fable_library_rust")>]
    let iter_to_seq (iter: obj): 'T seq = nativeOnly

#endif //FABLE_COMPILER_RUST
