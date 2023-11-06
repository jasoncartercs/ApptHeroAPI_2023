using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Abstraction.Enum
{
    public enum PaymentOrderModeEnum
    {
        OnlineWebsite = 1,
        InPerson = 2,
        OnlineBot = 3,
    }


    public enum PaymentOrderMadeEnum
    {
        CreditCard = 1,
        Cash = 2,
        Check = 3,
        Certificate = 4,
        Other = 5,
        StoreCredit = 6,
    }
}
