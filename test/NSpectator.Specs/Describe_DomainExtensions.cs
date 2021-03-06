#region [R# naming]
// ReSharper disable ArrangeTypeModifiers
// ReSharper disable UnusedMember.Local
// ReSharper disable FieldCanBeMadeReadOnly.Local
// ReSharper disable ArrangeTypeMemberModifiers
// ReSharper disable InconsistentNaming
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using NSpectator.Domain.Extensions;
using NUnit.Framework;
using System.Threading.Tasks;
using FluentAssertions;

namespace NSpectator.Specs
{
    [TestFixture]
    [Category("DomainExtensions")]
    public class Describe_DomainExtensions
    {
        abstract class IndirectAbstractAncestor : Spec
        {
            void indirect_ancestor_method() { }
        }

        class ConcreteAncestor : IndirectAbstractAncestor
        {
            void concrete_ancestor_method() { }
        }

        abstract class abstractAncestor : ConcreteAncestor
        {
            void ancestor_method() { }
        }

        abstract class abstractParent : abstractAncestor
        {
            void parent_method() { }
        }

        class child : abstractParent
        {
            public void public_child_method() { }

            void private_child_method() { }

            void helper_method_with_paramter(int i) { }

            void NoUnderscores() { }

            public async Task public_async_child_method()
            {
                await Task.Delay(0);
            }

            public async Task<long> async_method_with_result()
            {
                await Task.Delay(0); return 0L;
            }

            public async void async_void_method()
            {
                await Task.Delay(0);
            }

            async Task private_async_child_method()
            {
                await Task.Delay(0);
            }

            async Task async_helper_method_with_paramter(int i)
            {
                await Task.Delay(0);
            }

            async Task NoUnderscoresAsync()
            {
                await Task.Delay(0);
            }
        }

        class grandChild : child
        {
        }

        [Test]
        public void Should_include_direct_private_methods()
        {
            ShouldContain("private_child_method");
        }

        [Test]
        public void Should_include_direct_async_private_methods()
        {
            AsyncShouldContain("private_async_child_method");
        }

        [Test]
        public void Should_include_direct_public_methods()
        {
            ShouldContain("public_child_method");
        }

        [Test]
        public void Should_include_direct_async_public_methods()
        {
            AsyncShouldContain("public_async_child_method");
        }

        [Test]
        public void Should_include_async_methods_with_result()
        {
            AsyncShouldContain("async_method_with_result");
        }

        [Test]
        public void Should_include_async_void_methods()
        {
            AsyncShouldContain("async_void_method");
        }

        [Test]
        public void Should_disregard_methods_with_parameters()
        {
            ShouldNotContain("helper_method_with_paramter", typeof(child));
        }

        [Test]
        public void Should_disregard_async_methods_with_parameters()
        {
            AsyncShouldNotContain("async_helper_method_with_paramter", typeof(child));
        }

        [Test]
        public void Should_disregard_methods_with_out_underscores()
        {
            ShouldNotContain("NoUnderscores", typeof(child));
        }

        [Test]
        public void Should_disregard_async_methods_with_out_underscores()
        {
            AsyncShouldNotContain("NoUnderscoresAsync", typeof(child));
        }

        [Test]
        public void Should_include_methods_from_abstract_parent()
        {
            ShouldContain("parent_method");
        }

        [Test]
        public void Should_include_methods_from_direct_abstract_ancestor()
        {
            ShouldContain("ancestor_method");
        }

        [Test]
        public void Should_disregard_methods_from_concrete_ancestor()
        {
            ShouldNotContain("concrete_ancestor_method", typeof(child));
        }

        [Test]
        public void Should_disregard_methods_from_indirect_abstract_ancestor()
        {
            ShouldNotContain("indirect_ancestor_method", typeof(child));
        }

        [Test]
        public void Should_disregard_methods_from_concrete_parent()
        {
            ShouldNotContain("private_child_method", typeof(grandChild));
        }

        [Test]
        public void Should_disregard_async_methods_from_concrete_parent()
        {
            AsyncShouldNotContain("private_async_child_method", typeof(grandChild));
        }

        public void ShouldContain(string name)
        {
            var methodInfos = typeof(child).AllMethods();

            methodInfos.Should().Contain(m => m.Name == name);
        }

        public void ShouldNotContain(string name, Type type)
        {
            var methodInfos = type.AllMethods();

            methodInfos.Any(m => m.Name == name).Should().BeFalse();
        }

        public void AsyncShouldContain(string name)
        {
            var methodInfos = typeof(child).AsyncMethods();

            methodInfos.Any(m => m.Name == name).Should().BeTrue();
        }

        public void AsyncShouldNotContain(string name, Type type)
        {
            var methodInfos = type.AsyncMethods();

            methodInfos.Any(m => m.Name == name).Should().BeFalse();
        }

        class Foo1{}

        abstract class Foo2 : Foo1{}

        class Foo3 : Foo2{}

        abstract class Foo4 : Foo3{}

        abstract class Foo5 : Foo4{}

        class Foo6 : Foo5{}

        [Test,
         TestCase(typeof(Foo1), new [] {typeof(Foo1)}),
         TestCase(typeof(Foo2), new [] {typeof(Foo2)}),
         TestCase(typeof(Foo3), new [] {typeof(Foo2), typeof(Foo3)}),
         TestCase(typeof(Foo4), new [] {typeof(Foo4)}),
         TestCase(typeof(Foo5), new [] {typeof(Foo4), typeof(Foo5)}),
         TestCase(typeof(Foo6), new [] {typeof(Foo4), typeof(Foo5), typeof(Foo6)})]
        public void Should_build_immediate_abstract_class_chain(Type type, Type[] chain)
        {
            IEnumerable<Type> generatedChain = type.GetAbstractBaseClassChainWithClass();

            generatedChain.SequenceEqual(chain).Should().BeTrue();
        }

        class Bar1{}

        class Bar11 : Bar1{}

        class Bar2<TBaz1>{}

        class Bar21 : Bar2<Bar1>{}

        class Bar3<TBaz1, TBaz2>{}

        class Bar31 : Bar3<Bar1, Bar1>{}

        class Bar32 : Bar3<Bar1, Bar2<Bar1>>{}

        [Test,
         TestCase(typeof(Bar11), "Bar1"),
         TestCase(typeof(Bar21), "Bar2<Bar1>"),
         TestCase(typeof(Bar31), "Bar3<Bar1, Bar1>"),
         TestCase(typeof(Bar32), "Bar3<Bar1, Bar2<Bar1>>")]
        public void Should_generate_pretty_type_names(Type derivedType, string expectedNameForBaseType)
        {
            string name = derivedType.BaseType.CleanName();

            name.Should().Be(expectedNameForBaseType);
        }
    }
}
