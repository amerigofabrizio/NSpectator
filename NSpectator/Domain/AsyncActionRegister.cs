﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpectator.Domain
{
    public class AsyncActionRegister
    {
        public AsyncActionRegister(Action<string, string, Func<Task>> asyncActionSetter)
        {
            this.asyncActionSetter = asyncActionSetter;
        }

        Action<string, string, Func<Task>> asyncActionSetter;

        public Func<Task> this[string key]
        {
            set { asyncActionSetter(key, null, value); }
        }

        public Func<Task> this[string key, string tags]
        {
            set { asyncActionSetter(key, tags, value); }
        }
    }
}
