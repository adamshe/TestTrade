namespace TradeMagic.Pats
{
    using System;

    internal enum ChangeType
    {
        ChangeBid = 1,
        ChangeBidDOM = 0x800,
        ChangeClosing = 0x400,
        ChangeHigh = 0x80,
        ChangeImpliedBid = 4,
        ChangeImpliedOffer = 8,
        ChangeLast = 0x20,
        ChangeLow = 0x100,
        ChangeOffer = 2,
        ChangeOfferDOM = 0x1000,
        ChangeOpening = 0x200,
        ChangeRFQ = 0x10,
        ChangeTotal = 0x40
    }
}

