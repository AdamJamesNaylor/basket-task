
namespace Basket.Task.Business.Services {
    using System;
    using Vouchers;

    public class VoucherService {
        public Voucher Get(string voucherCode) {
            switch (voucherCode) {
                case "XXX-XXX":
                    return new GiftVoucher(Guid.NewGuid(), 5.00m, voucherCode);
                case "YYY-YYY":
                    return new OfferVoucher(Guid.NewGuid(), 5.00m, 50.00m, ProductCategory.HeadGear, voucherCode);
                case "ZZZ-ZZZ":
                    return new OfferVoucher(Guid.NewGuid(), 5.00m, 50.00m, voucherCode);
                default:
                    throw new Exception($"Unknown voucher code {voucherCode}.");
            }
        }
    }
}