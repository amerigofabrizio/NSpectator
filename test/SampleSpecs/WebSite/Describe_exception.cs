﻿using System;
using NSpectator;

public class describe_exception : Spec
{
    void given_a_null_string()
    {
        it["should throw null-ref"] =
            expect<NullReferenceException>(() => nullString.Trim());
    }
    string nullString = null;
}

public static class describe_exception_output
{
    public static string Output = @"
describe exception
  given a null string
    should throw null-ref

1 Examples, 0 Failed, 0 Pending
";
    public static int ExitCode = 0;
}
