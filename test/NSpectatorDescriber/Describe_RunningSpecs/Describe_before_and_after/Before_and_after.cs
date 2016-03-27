﻿using NSpectator;
using NSpectator.Describer.WhenRunningSpecs;
using NUnit.Framework;

namespace NSpectator.Describer.describe_RunningSpecs.describe_before_and_after
{
    class sequence_spec : nspec { public static string sequence; }

    [TestFixture]
    [Category("RunningSpecs")]
    public class before_and_after : When_running_specs
    {
        class SpecClass : sequence_spec
        {
            void as_long_as_the_world_has_not_come_to_an_end()
            {
                beforeAll = () => sequence = "A";
                before = () => sequence += "B";
                it["spec 1"] = () => sequence += "1";
                it["spec 2"] = () => sequence += "2"; //two specs cause before_each and after_each to run twice
                after = () => sequence += "C";
                afterAll = () => sequence += "D";
            }
        }

        [Test]
        public void everything_runs_in_the_correct_order_and_with_the_correct_frequency()
        {
            Run(typeof(SpecClass));

            SpecClass.sequence.Is("AB1CB2CD");
        }
    }

    [TestFixture]
    [Category("RunningSpecs")]
    public class before_and_after_aliases : When_running_specs
    {
        class SpecClass : sequence_spec
        {
            void as_long_as_the_world_has_not_come_to_an_end()
            {
                beforeAll = () => sequence = "A";
                beforeEach = () => sequence += "B";
                it["spec 1"] = () => sequence += "1";
                it["spec 2"] = () => sequence += "2"; //two specs cause before_each and after_each to run twice
                afterEach = () => sequence += "C";
                afterAll = () => sequence += "D";
            }
        }

        [Test]
        public void everything_runs_in_the_correct_order_and_with_the_correct_frequency()
        {
            Run(typeof(SpecClass));

            SpecClass.sequence.Is("AB1CB2CD");
        }
    }
}
