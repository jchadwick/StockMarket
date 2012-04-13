using System;
using Common;

namespace Messages
{
    [Serializable]
    public class MarketStateChangeRequest
    {
        public MarketState NewState { get; set; }

        public MarketStateChangeRequest()
        {
        }

        public MarketStateChangeRequest(MarketState newState)
        {
            NewState = newState;
        }
    }
}