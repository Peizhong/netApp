﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetApp.EventBus.Abstractions
{
    public interface IDynamicIntegrationEventHandler
    {
        Task Handle(dynamic eventData);
    }
}
