module FileReader

open AppErrors

let readFile filePath =
    try
        System.IO.File.ReadAllText filePath |> Ok
    with exn -> FileReaderError exn.Message |> Error
