﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBus
{
    public interface IMessageBus
    {
        void Start();
        void Stop();
        Task Publish<TMessage>(TMessage message) where TMessage : class;
    }
}
