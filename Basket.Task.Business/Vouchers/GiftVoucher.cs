namespace Basket.Task.Business.Vouchers {
    using System;
    using System.Linq;

    public class GiftVoucher
        : Voucher {

        public GiftVoucher(Guid id, decimal totalDiscount, string code)
            : base(id, totalDiscount, code) { }

        public override VoucherValidationResult ApplyTo(IProductContainer basket) {
            var nonDiscountableTotal = basket.Products
                .Where(p => p.Category.Equals(ProductCategory.Voucher))
                .Sum(p => p.Price);

            if (nonDiscountableTotal >= basket.DiscountedTotal)
                return new MaximumDiscountAppliedResult(this);

            return new VoucherIsValidResult(this, TotalDiscount);
        }
    }
}