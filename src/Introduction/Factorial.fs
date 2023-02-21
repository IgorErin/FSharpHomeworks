namespace Introduction

module Factorial =
    let run count =
        if count > 0 then Seq.reduce (*) [ 1..count ] else 1
