﻿using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Configuration.Country.Events
{
    public interface Created : IEvent
    {
        Guid CountryId { get; set; }

        String Code { get; set; }

        String Name { get; set; }
    }
}