module AppErrors

type ApplicationError =
    | FileReaderError of string
    | ParserError of string
    | MetadataFileMissing of string
