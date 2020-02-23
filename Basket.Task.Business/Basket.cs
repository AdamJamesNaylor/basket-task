namespace Basket.Task.Business {
    using System.Collections.Generic;
    using System.Linq;
    using Vouchers;

    public interface IProductContainer {
        decimal Total { get; }
        decimal DiscountedTotal { get; }
        void Add(Product product);
        void Remove(Product product);
        IReadOnlyCollection<Product> Products { get; }
    }

    public interface IDiscountable {
        void ApplyVoucher(Voucher voucher);
        IReadOnlyCollection<Voucher> AppliedVouchers { get; }
        IReadOnlyCollection<VoucherValidationResult> VoucherResults { get; }
    }

    public class Basket
        : IProductContainer, IDiscountable {

        public decimal Total { get { return Products.Sum(p => p.Price); } }
        public decimal DiscountedTotal { get; private set; }

        public IReadOnlyCollection<Voucher> AppliedVouchers => _appliedVouchers.AsReadOnly();
        public IReadOnlyCollection<VoucherValidationResult> VoucherResults => _voucherResults.AsReadOnly();
        public IReadOnlyCollection<Product> Products => _products.AsReadOnly();

        public void ApplyVoucher(Voucher voucher) {
            if (_appliedVouchers.Any(v => v.Id == voucher.Id))
                return;

            if (_appliedVouchers.Any(v => v is OfferVoucher))
                return;

            var result = voucher.ApplyTo(this);
            _appliedVouchers.Add(voucher);
            _voucherResults.Add(result);
            if (!(result is VoucherIsValidResult validDiscount))
                return;

            DiscountedTotal -= validDiscount.ApplicableDiscount;
            if (DiscountedTotal < 0)
                DiscountedTotal = 0;
        }

        private void ReapplyVouchers() {
            DiscountedTotal = Total;
            _voucherResults.Clear();

            foreach (var voucher in AppliedVouchers) {
                var result = voucher.ApplyTo(this);
                _voucherResults.Add(result);
                if (!(result is VoucherIsValidResult validDiscount))
                    break;

                DiscountedTotal -= validDiscount.ApplicableDiscount;
                if (DiscountedTotal < 0)
                    DiscountedTotal = 0;
            }
        }

        public void Add(Product product) {
            _products.Add(product);
            ReapplyVouchers();
        }

        public void Remove(Product product) {
            _products.RemoveAll(p => p.Id == product.Id);
            ReapplyVouchers();
        }

        private readonly List<Product> _products = new List<Product>();
        private readonly List<Voucher> _appliedVouchers = new List<Voucher>();
        private readonly List<VoucherValidationResult> _voucherResults = new List<VoucherValidationResult>();
    }
}