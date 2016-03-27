﻿using System;
using System.Collections.Generic;
using System.Linq;
using NSpectator;
using NSpectator.Domain;
using NSpecSpecs.describe_RunningSpecs;
using NUnit.Framework;

namespace NSpecSpecs.WhenRunningSpecs
{
    [TestFixture]
    public class When_running_specs
    {
        [SetUp]
        public void InitializeRunnerInvocation()
        {
            formatter = new FormatterStub();
        }

        protected When_running_specs Run(params Type[] types)
        {
            //if (types.Count() == 1) tags = types.First().Name;

            this.types = types;

            var tagsFilter = new Tags().Parse(tags);

            builder = new ContextBuilder(new SpecFinder(types), tagsFilter, new DefaultConventions());

            runner = new ContextRunner(tagsFilter, formatter, failFast);

            contextCollection = builder.Contexts();

            contextCollection.Build();

            classContext = contextCollection
                .AllContexts()
                .Where(c => c is ClassContext)
                .Cast<ClassContext>()
                .FirstOrDefault(c => types.Contains(c.type));

            methodContext = contextCollection.AllContexts().FirstOrDefault(c => c is MethodContext);

            runner.Run(contextCollection);

            return this;
        }

        protected Context TheContext(string name)
        {
            var theContext = contextCollection
                .SelectMany(rootContext => rootContext.AllContexts())
                .SelectMany(contexts => contexts.AllContexts().Where(context => context.Name == name)).First();

            theContext.Name.should_be(name);

            return theContext;
        }

        protected IEnumerable<ExampleBase> AllExamples()
        {
            return contextCollection
                .SelectMany(rootContext => rootContext.AllExamples());
        }

        protected ExampleBase TheExample(string name)
        {
            var theExample = contextCollection
                .SelectMany(rootContext => rootContext.AllContexts())
                .SelectMany(contexts => contexts.AllExamples().Where(example => example.Spec == name)).FirstOrDefault();

            if (theExample == null) Assert.Fail("Did not find example named: " + name);

            theExample.Spec.should_be(name);

            return theExample;
        }

        protected int TheExampleCount()
        {
            var theExamples = contextCollection
                .SelectMany(rootContext => rootContext.AllContexts())
                .SelectMany(contexts => contexts.AllExamples())
                    .Distinct();

            return theExamples.Count();
        }

        protected ContextBuilder builder;
        protected ContextCollection contextCollection;
        protected ClassContext classContext;
        protected bool failFast;
        protected Context methodContext;
        protected ContextCollection contexts;
        protected FormatterStub formatter;
        ContextRunner runner;
        protected Type[] types;
        protected string tags;
    }
}