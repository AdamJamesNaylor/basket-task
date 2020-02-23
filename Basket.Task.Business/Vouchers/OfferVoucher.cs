namespace Basket.Task.Business.Vouchers {
    using System;
    using System.Linq;

    public class OfferVoucher
        : Voucher {

        public decimal Threshold { get; }
        public ProductCategory Category { get; }

        public OfferVoucher(Guid id, decimal totalDiscount, decimal threshold, string code)
            : this(id, totalDiscount, threshold, new Uncategorised(), code) {
            Threshold = threshold;
        }

        public OfferVoucher(Guid id, decimal totalDiscount, decimal threshold, ProductCategory category,
            string code)
            : base(id, totalDiscount, code)
            => (Threshold, Category) = (threshold, category);

        public override VoucherValidationResult ApplyTo(IProductContainer basket) {
            var discountableThreshold = basket.Products
                .Where(p => !p.Category.Equals(ProductCategory.Voucher))
                .Sum(p => p.Price);

            if (discountableThreshold < Threshold)
                return new BasketTotalBelowThresholdReason(this, Threshold - discountableThreshold);

            if (Category is Uncategorised)
                return new VoucherIsValidResult(this, TotalDiscount);

            var applicableTotal = basket.Products
                .Where(p => p.Category.Equals(Category))
                .Sum(p => p.Price);
            if (applicableTotal == 0.00m)
                return new NoApplicableProductsReason(this);
            var applicableDiscount = applicableTotal - TotalDiscount;
            return new VoucherIsValidResult(this, applicableDiscount <= 0.00m ? applicableTotal : TotalDiscount);

        }
    }
}