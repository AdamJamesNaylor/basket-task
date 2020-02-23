namespace Basket.Task.Business.Vouchers {
    using System.Collections.Generic;

    public interface IVoucherNotApplicableReason { }

    public abstract class VoucherValidationResult {
        public Voucher Voucher { get; }
        public string Message { get; protected set; }

        protected VoucherValidationResult(Voucher voucher) {
            Voucher = voucher;
        }
    }

    public class VoucherIsValidResult
        : VoucherValidationResult {
        public decimal ApplicableDiscount { get; }

        public VoucherIsValidResult(Voucher voucher, decimal applicableDiscount)
            : base(voucher) {
            ApplicableDiscount = applicableDiscount;
        }
    }


    public class BasketTotalBelowThresholdReason
        : VoucherValidationResult, IVoucherNotApplicableReason {
        public BasketTotalBelowThresholdReason(Voucher voucher, decimal difference)
            : base(voucher) {
            Message =
                $"You have not reached the spend threshold for voucher {voucher.Code}. Spend another £{difference} to receive £{voucher.TotalDiscount} discount from your basket total.";
        }
    }

    public class NoApplicableProductsReason
        : VoucherValidationResult, IVoucherNotApplicableReason {
        public NoApplicableProductsReason(Voucher voucher)
            : base(voucher) {
            Message = $"There are no products in your basket applicable to voucher Voucher {voucher.Code}.";
        }
    }

    public class MaximumDiscountAppliedResult
        : VoucherValidationResult, IVoucherNotApplicableReason {
        public MaximumDiscountAppliedResult(Voucher voucher)
            : base(voucher) {
            Message = "There are no more products in your basket that can be discounted.";
        }
    }

}