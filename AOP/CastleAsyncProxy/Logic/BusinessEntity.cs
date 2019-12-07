﻿using System;

namespace CastleAsyncProxy.Logic
{
    public class BusinessEntity
    {
        public Guid Id { get; set; }

        public object Data { get; set; }

        public override string ToString() => $"{nameof(Id)}: {Id} {nameof(Data)}: {Data}";
    }
}
