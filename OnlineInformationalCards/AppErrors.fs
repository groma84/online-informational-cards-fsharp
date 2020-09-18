module AppErrors

type ApplicationError =
    | FileReaderError of string
    | ParserError of string
    | MetadataFileMissing of string

type ApplicationError with
    static member ToString(ae: ApplicationError): string =
        match ae with
        | FileReaderError err -> sprintf "FileReaderError: %s" err
        | ParserError err -> sprintf "ParserError: %s" err
        | MetadataFileMissing err -> sprintf "MetadataFileMissing: %s" err
