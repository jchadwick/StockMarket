using System;
using Common;

namespace Messages
{
    [Serializable]
    public class MarketStateChange
    {
        public MarketState CurrentState { get; set; }

        public MarketStateChange()
        {
        }

        public MarketStateChange(MarketState currentState)
        {
            CurrentState = currentState;
        }
    }
}
