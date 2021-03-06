﻿using NSpectator;
using NSpectator.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpectator.Specs
{
    /// <summary>
    /// Abstract framework ExampleBase class implements functionalities common to both sync and async example implementations.
    /// This thin wrapper allows to test those functionalities.
    /// </summary>
    class ExampleBaseWrap : ExampleBase
    {
        public ExampleBaseWrap(string name)
            : base(name)
        {
        }

        public ExampleBaseWrap()
            : base()
        {
        }

        public override void Run(Spec spec)
        {
            throw new NotImplementedException();
        }

        public override bool IsAsync
        {
            get { throw new NotImplementedException(); }
        }
    }
}
