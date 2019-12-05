using System;

namespace CastleProxy.Logic
{
    public class DbEntity
    {
        public Guid Id { get; set; }

        public object Data { get; set; }

        public override string ToString() => $"{nameof(Id)}: {Id} {nameof(Data)}: {Data}";
    }
}
