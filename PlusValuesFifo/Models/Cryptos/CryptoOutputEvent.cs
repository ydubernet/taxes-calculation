using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlusValuesFifo.Models.Cryptos
{
    public class CryptoOutputEvent : IOutputEvent, ICryptoEvent
    {
        public string AssetName => throw new NotImplementedException();

        public BuySell ActionEvent => throw new NotImplementedException();

        public decimal Amount => throw new NotImplementedException();

        public decimal Price => throw new NotImplementedException();

        public DateTime Date => throw new NotImplementedException();

        public decimal Fee => throw new NotImplementedException();

        public decimal PlusValue => throw new NotImplementedException();
    }
}
