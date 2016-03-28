﻿using NSpectator;

public class Describe_class_level : Spec
{
    string sequence;
    //before, act, and it can also be declared at the class level like so
    void before_each()
    {
        sequence = "arrange, ";
    }

    void act_each()
    {
        sequence += "act";
    }

    //prefixing a method with "it_" or "specify_"
    //will tell nspec to treat the method as an example
    void specify_given_befores_and_acts_run_in_the_correct_sequence()
    {
        sequence.should_be("arrange, act");
    }
}

public static class Describe_class_level_output
{
    public static string Output = @"
describe class level
  specify given befores and acts run in the correct sequence

1 Examples, 0 Failed, 0 Pending
";
    public static int ExitCode = 0;
}