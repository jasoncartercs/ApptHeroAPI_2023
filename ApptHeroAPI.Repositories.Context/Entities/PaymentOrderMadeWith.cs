using System;

namespace ApptHeroAPI.Repositories.Context.Entities
{

    public class PaymentOrderMadeWith
    {
        public long PaymentOrderMadeWithId { get; set; }


        public long PaymentOrderId { get; set; }

        public int PaymentMadeWith { get; set; }

        public string CheckNumber { get; set; }

        public int Amount { get; set; }

        public string OtherDescription { get; set; }
    }
}
