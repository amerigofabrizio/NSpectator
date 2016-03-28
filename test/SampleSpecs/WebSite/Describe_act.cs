﻿#region [R# naming]

// ReSharper disable ArrangeTypeModifiers
// ReSharper disable UnusedMember.Local
// ReSharper disable FieldCanBeMadeReadOnly.Local
// ReSharper disable ArrangeTypeMemberModifiers
// ReSharper disable InconsistentNaming

#endregion

using NSpectator;

namespace SampleSpecs.WebSite
{
    [Tag("describe_act")]
    public class Describe_batman_sound_effects_as_text : Spec
    {
        string sound;

        void they_are_loud_and_emphatic()
        {
            //act runs after all the befores, and before each spec
            //declares a common act (arrange, act, assert) for all subcontexts
            act = () => sound = sound.ToUpper() + "!!!";
            context["given bam"] = () =>
            {
                before = () => sound = "bam";
                it["should be BAM!!!"] =
                    () => sound.should_be("BAM!!!");
            };
            context["given whack"] = () =>
            {
                before = () => sound = "whack";
                it["should be WHACK!!!"] =
                    () => sound.should_be("WHACK!!!");
            };
        }
    }

    public static class Describe_batman_sound_effects_as_text_output
    {
        public static string Output = @"
describe batman sound effects as text
  they are loud and emphatic
    given bam
      should be BAM!!!
    given whack
      should be WHACK!!!

2 Examples, 0 Failed, 0 Pending
";
        public static int ExitCode = 0;
    }
}