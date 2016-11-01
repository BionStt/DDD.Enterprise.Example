﻿using Aggregates;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Configuration.Country
{
    public class Handler :
        IHandleMessages<Commands.Create>,
        IHandleMessages<Commands.UpdateName>,
        IHandleMessages<Commands.Destroy>
    {
        private readonly IUnitOfWork _uow;
        

        public Handler(IUnitOfWork uow)
        {
            _uow = uow;
           
        }

        public async Task Handle(Commands.Create command, IMessageHandlerContext ctx)
        {
            var country = await _uow.For<Country>().New(command.CountryId);
            country.Create(command.Code, command.Name);
        }

        public async Task Handle(Commands.Destroy command, IMessageHandlerContext ctx)
        {
            var country = await _uow.For<Country>().Get(command.CountryId);
            country.Destroy();
        }

        public async Task Handle(Commands.UpdateName command, IMessageHandlerContext ctx)
        {
            var country = await _uow.For<Country>().Get(command.CountryId);
            country.ChangeName(command.Name);
        }
    }
}