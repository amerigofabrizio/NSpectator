﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NSpectator.Domain
{
    public abstract class AsyncMethodLevelHook
    {
        public AsyncMethodLevelHook(MethodInfo method, string hookName)
        {
            runner = new AsyncMethodRunner(method, hookName);
        }

        public virtual void Run(nspec nspec)
        {
            runner.Run(nspec);
        }

        readonly AsyncMethodRunner runner;
    }

    public class AsyncMethodLevelBefore : AsyncMethodLevelHook
    {
        public AsyncMethodLevelBefore(MethodInfo method) : base(method, "before_each") { }
    }

    public class AsyncMethodLevelAct : AsyncMethodLevelHook
    {
        public AsyncMethodLevelAct(MethodInfo method) : base(method, "act_each") { }
    }

    public class AsyncMethodLevelAfter : AsyncMethodLevelHook
    {
        public AsyncMethodLevelAfter(MethodInfo method) : base(method, "after_each") { }
    }

    public class AsyncMethodLevelBeforeAll : AsyncMethodLevelHook
    {
        public AsyncMethodLevelBeforeAll(MethodInfo method) : base(method, "before_all") { }
    }

    public class AsyncMethodLevelAfterAll : AsyncMethodLevelHook
    {
        public AsyncMethodLevelAfterAll(MethodInfo method) : base(method, "after_all") { }
    }
}
