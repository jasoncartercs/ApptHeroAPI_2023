using System;

namespace ApptHeroAPI.Repositories.Context.Entities
{

    public class PaymentOrder
    {
        public long PaymentOrderId { get; set; }

        public DateTime OrderDate { get; set; }

        public int? Total { get; set; }


        public string PaymentTransactionId { get; set; }

        public long PersonId { get; set; }

        public long CompanyId { get; set; }

        public int SoldAt { get; set; }

        public int PaymentMadeWith { get; set; }

        public string CheckNumber { get; set; }

        public bool? IsDiscountApplied { get; set; }

        public int TipAmount { get; set; }

        public long? AppointmentId { get; set; }

        public int PaymentOrderTypeId { get; set; }
    }
}
