﻿module ilf.pgn.PgnParsers.MoveSeries

open FParsec
open ilf.pgn.MoveSeries
open ilf.pgn.PgnParsers.Basic
open ilf.pgn.PgnParsers.Move


let pSinglePeriod = pchar '.' >>. preturn true
let pEllipsis = (str "..." .>> manyChars (pchar '.')) <|> str "…" >>. preturn false

let pMoveNumberIndicator = pint32 .>> ws .>>. (pEllipsis <|> pSinglePeriod) <!> "pMoveNumberIndicator"
let pFullMoveEntry =
    opt pMoveNumberIndicator .>> ws .>>. pMove .>> ws1 .>>. pMove 
    |>> fun ((moveInd, moveWhite), moveBlack) -> 
            match moveInd with 
            | None -> MoveEntry(White= Some moveWhite, Black= Some moveBlack)
            | Some(num, _) -> MoveEntry(MoveNumber=Some num, White= Some moveWhite, Black= Some moveBlack)
    <!> "pFullMoveEntry"

let pSplitMoveEntry = 
    pMoveNumberIndicator .>> ws .>>. pMove
    |>> fun ((num, firstMove), move) -> 
            match firstMove with 
            | true -> MoveEntry(MoveNumber= Some num, White= Some move)
            | false -> MoveEntry(MoveNumber=Some num, Black= Some move)
    <!> "pSplitMoveEntry"

let pCommentary = 
    between (str "{") (str "}") (many (noneOf "}")) 
    <|> between (str "[") (str "]") (many (noneOf "]"))
    <|> between (str "(") (str ")") (many (noneOf ")"))
    <|> between (str ";") newline (many (noneOf "\n")) //to end of line comment
    |>>charList2String
    
let pMoveSeriesEntry= 
    attempt(pFullMoveEntry) 
    <|> pSplitMoveEntry
    .>> ws .>>. opt pCommentary
    |>> fun (entry, comment) -> 
            match comment with
            | None -> entry
            | Some(c) -> entry.Comment <- Some c; entry
    <!> "pMoveSeriesEntry"

let pMoveSeries = sepEndBy pMoveSeriesEntry ws
let appyPCommentary (p: string)= run pCommentary p
