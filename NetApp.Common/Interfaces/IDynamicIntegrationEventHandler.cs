﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetApp.Common.Interfaces
{
    public interface IDynamicIntegrationEventHandler
    {
        Task Handle(dynamic eventData);
    }
}
