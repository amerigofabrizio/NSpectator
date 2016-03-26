﻿using System.Collections.Generic;
using System.Linq;
using NSpectator.Domain.Formatters;

namespace NSpectator.Domain
{
    public class ContextCollection : List<Context>
    {
        public IEnumerable<ExampleBase> Examples()
        {
            return this.SelectMany(c => c.AllExamples());
        }

        public IEnumerable<ExampleBase> Failures()
        {
            return Examples().Where(e => e.Exception != null);
        }

        public IEnumerable<ExampleBase> Pendings()
        {
            return Examples().Where(e => e.Pending);
        }

        public ContextCollection Build()
        {
            this.Do(c => c.Build());

            return this;
        }

        public void Run(bool failFast = false)
        {
            Run(new SilentLiveFormatter(), failFast);
        }

        public void Run(ILiveFormatter formatter, bool failFast)
        {
            this.Do(c => c.Run(formatter, failFast: failFast));

            this.Do(c => c.AssignExceptions());
        }

        public void TrimSkippedContexts()
        {
            this.Do(c => c.TrimSkippedDescendants());

            this.RemoveAll(c => !c.HasAnyExecutedExample());
        }

        public IEnumerable<Context> AllContexts()
        {
            return this.SelectMany(c => c.AllContexts());
        }

        public Context Find(string name)
        {
            return AllContexts().FirstOrDefault(c => c.Name == name);
        }

        public ExampleBase FindExample(string name)
        {
            return Examples().FirstOrDefault(e => e.Spec == name);
        }

        public ContextCollection(IEnumerable<Context> contexts) : base(contexts) {}

        public ContextCollection() {}

        public bool AnyTaggedWithFocus()
        {
            return AnyTaggedWith(Tags.Focus);
        }

        public bool AnyTaggedWith(string tag)
        {
            return AnyExamplesTaggedWith(tag) || AnyContextsTaggedWith(tag);
        }

        public bool AnyContextsTaggedWith(string tag)
        {
            return AllContexts().Any(s => s.Tags.Contains(tag));
        }

        public bool AnyExamplesTaggedWith(string tag)
        {
            return AllContexts().SelectMany(s => s.AllExamples()).Any(s => s.Tags.Contains(tag));
        }
    }
}