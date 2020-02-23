namespace Basket.Task.Business.Vouchers {
    using System;

    public abstract class Voucher
        : IIdentifiable {

        public Guid Id { get; }
        public decimal TotalDiscount { get; }
        public string Code { get; }

        protected Voucher(Guid id, decimal totalDiscount, string code)
            => (Id, TotalDiscount, Code) = (id, totalDiscount, code);

        public abstract VoucherValidationResult ApplyTo(IProductContainer basket);
    }
}