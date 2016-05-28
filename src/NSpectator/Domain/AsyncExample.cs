﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NSpectator.Domain
{
    public class AsyncExample : ExampleBase
    {
        public override void Run(Spec spec)
        {
            asyncAction.Offload();
        }

        public override bool IsAsync
        {
            get { return true; }
        }

        /* No need for the following: 
         * Async lambda expressions cannot be converted to expression trees

        public AsyncExample(Expression<Func<Task>> asyncExpr, bool pending = false) 
            : this(Parse(asyncExpr), null, asyncExpr.Compile(), pending) { }

         */

        public AsyncExample(string name = "", string tags = "", Func<Task> asyncAction = null, bool pending = false)
            : base(name, tags, pending)
        {
            this.asyncAction = asyncAction;
        }

        Func<Task> asyncAction;
    }
}