
namespace Basket.Task.Tests {
    using Business;
    using Xunit;
    using Business.Services;

    public class GiftVoucherTests
        : VoucherTests {

        [Fact]
        public void MultipleGiftVouchersCanBeAppliedToABasket() {
            //Arrange
            var expectedTotal = 41.00m;

            var basket = new Basket();
            basket.Add(ProductService.Get(ProductService.ProductCode.Hat));
            basket.Add(ProductService.Get(ProductService.ProductCode.Jumper));

            var giftVoucher1 = VoucherService.Get("XXX-XXX");
            var giftVoucher2 = VoucherService.Get("XXX-XXX");

            //Act
            basket.ApplyVoucher(giftVoucher1);
            basket.ApplyVoucher(giftVoucher2);

            //Assert
            Assert.Equal(expectedTotal, basket.DiscountedTotal);
        }

        [Fact]
        public void GiftVouchersCanOnlyBeRedeemedAgainstNonGiftVoucherProducts() {
            //Arrange
            var expectedTotal = 30.00m;

            var basket = new Basket();
            basket.Add(ProductService.Get(ProductService.ProductCode.Hat));
            basket.Add(ProductService.Get(ProductService.ProductCode.GiftVoucher));

            var giftVoucher1 = VoucherService.Get("XXX-XXX");
            var giftVoucher2 = VoucherService.Get("XXX-XXX");
            var giftVoucher3 = VoucherService.Get("XXX-XXX");
            var giftVoucher4 = VoucherService.Get("XXX-XXX");
            var giftVoucher5 = VoucherService.Get("XXX-XXX");
            var giftVoucher6 = VoucherService.Get("XXX-XXX");

            //Act
            basket.ApplyVoucher(giftVoucher1);
            basket.ApplyVoucher(giftVoucher2);
            basket.ApplyVoucher(giftVoucher3);
            basket.ApplyVoucher(giftVoucher4);
            basket.ApplyVoucher(giftVoucher5);
            basket.ApplyVoucher(giftVoucher6); //Edge case: sixth voucher should not be applied

            //Assert
            Assert.Equal(expectedTotal, basket.DiscountedTotal);
        }

        [Fact]
        public void PurchaseOfGiftVouchersDoNotContributeToDiscountableBasketTotal() {
            //Arrange
            var expectedTotal = 55.00m;

            var basket = new Basket();
            basket.Add(ProductService.Get(ProductService.ProductCode.Hat));
            basket.Add(ProductService.Get(ProductService.ProductCode.GiftVoucher));

            var offerVoucher1 = VoucherService.Get("ZZZ-ZZZ");

            //Act
            basket.ApplyVoucher(offerVoucher1);

            //Assert
            Assert.Equal(expectedTotal, basket.DiscountedTotal);
        }
    }
}
