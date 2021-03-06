﻿#region [R# naming]
// ReSharper disable ArrangeTypeModifiers
// ReSharper disable UnusedMember.Local
// ReSharper disable FieldCanBeMadeReadOnly.Local
// ReSharper disable ArrangeTypeMemberModifiers
// ReSharper disable InconsistentNaming
#endregion
using System.Collections.Generic;
using NSpectator.Domain;
using NUnit.Framework;
using FluentAssertions;

namespace NSpectator.Specs
{
    [TestFixture]
    public class DescribeTags
    {
        [Test]
        public void Should_parses_single_tag()
        {
            var tags = Tags.ParseTags("mytag");
            tags.Should_contain_tag("mytag");
        }

        [Test]
        public void Should_parses_multiple_tags()
        {
            var tags = Tags.ParseTags("myTag1,myTag2");
            tags.Should_contain_tag("myTag1");
            tags.Should_contain_tag("myTag2");
            tags.Should().HaveCount(2);
        }

        [Test]
        public void Should_parses_single_include_tag_filters()
        {
            var tags = new Tags();
            tags.Parse("mytag");
            tags.Should_tag_as_included("mytag");
        }

        [Test]
        public void Should_parses_single_exclude_tag_filters()
        {
            var tags = new Tags();
            tags.Parse("~mytag");
            tags.Should_tag_as_excluded("mytag");
        }

        [Test]
        public void Should_parses_multiple_tags_filters()
        {
            var tags = new Tags();
            tags.Parse("myInclude1,~myExclude1,myInclude2,~myExclude2,");

            tags.Should_tag_as_included("myInclude1");
            tags.Should_tag_as_excluded("myExclude1");

            tags.Should_tag_as_included("myInclude2");
            tags.Should_tag_as_excluded("myExclude2");

            tags.IncludeTags.Count.Should().Be(2);
            tags.ExcludeTags.Count.Should().Be(2);
        }
    }

    public static class TagAssertions
    {
        public static List<string> Should_contain_tag(this List<string> collection, string tag)
        {
            CollectionAssert.Contains(collection, tag);
            return collection;
        }

        public static Tags Should_tag_as_included(this Tags tags, string tag)
        {
            CollectionAssert.Contains(tags.IncludeTags, tag);
            return tags;
        }

        public static Tags Should_tag_as_excluded(this Tags tags, string tag)
        {
            CollectionAssert.Contains(tags.ExcludeTags, tag);
            return tags;
        }
    }
}
