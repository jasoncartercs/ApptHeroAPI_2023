using System;

namespace ApptHeroAPI.Repositories.Context.Entities
{

    public class PaymentOrderDetail
    {
        public long PaymentOrderDetailId { get; set; }

        public long PaymentOrderId { get; set; }

        public long ProductId { get; set; }

        public decimal OrderPrice { get; set; }

        public bool IsDiscountApplied { get; set; }


        public double TipPercentage { get; set; }
    }
}
