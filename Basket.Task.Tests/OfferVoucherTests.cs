
namespace Basket.Task.Tests {
    using System.Linq;
    using Business;
    using Business.Vouchers;
    using Xunit;
    using Business.Services;

    public class OfferVoucherTests
        : VoucherTests {

        [Fact]
        public void ThresholdMustBeMatchedOrExceeded() {
            //Arrange
            var expectedTotal = 25.00m;

            var basket = new Basket();
            basket.Add(ProductService.Get(ProductService.ProductCode.Hat));

            var offerVoucher1 = VoucherService.Get("ZZZ-ZZZ");

            //Act
            basket.ApplyVoucher(offerVoucher1);

            //Assert
            Assert.Equal(expectedTotal, basket.DiscountedTotal);

            basket.Add(ProductService.Get(ProductService.ProductCode.Hat));
            expectedTotal = 45.00m;

            Assert.Equal(expectedTotal, basket.DiscountedTotal);
        }

        [Fact]
        public void OnlyASingleOfferVoucherCanBeAppliedToABasket() {
            //Arrange
            var expectedTotal = 46.00m;

            var basket = new Basket();
            basket.Add(ProductService.Get(ProductService.ProductCode.Hat));
            basket.Add(ProductService.Get(ProductService.ProductCode.Jumper));

            var offerVoucher1 = VoucherService.Get("ZZZ-ZZZ");
            var offerVoucher2 = VoucherService.Get("ZZZ-ZZZ");

            //Act
            basket.ApplyVoucher(offerVoucher1);
            basket.ApplyVoucher(offerVoucher2);

            //Assert
            Assert.Equal(expectedTotal, basket.DiscountedTotal);
        }

        [Fact]
        public void ApplicableOnlyToASubsetOfProducts() {
            //Arrange
            var expectedTotal = 51.00m;

            var basket = new Basket();
            basket.Add(ProductService.Get(ProductService.ProductCode.Hat));
            basket.Add(ProductService.Get(ProductService.ProductCode.Jumper));

            var offerVoucher1 = VoucherService.Get("YYY-YYY");

            //Act
            basket.ApplyVoucher(offerVoucher1);

            //Assert
            Assert.Equal(expectedTotal, basket.DiscountedTotal);

            basket.Add(ProductService.Get(ProductService.ProductCode.HeadLight));
            expectedTotal = 51.00m;

            Assert.Equal(expectedTotal, basket.DiscountedTotal);
        }

        [Fact]
        public void SpendThresholdMessageIsDisplayed() {
            //Arrange
            var expectedTotal = 55.00m;

            var basket = new Basket();
            basket.Add(ProductService.Get(ProductService.ProductCode.Hat));
            basket.Add(ProductService.Get(ProductService.ProductCode.GiftVoucher));

            var offerVoucher1 = VoucherService.Get("YYY-YYY");
            var expectedReason = new BasketTotalBelowThresholdReason(offerVoucher1, 25.00m);

            //Act
            basket.ApplyVoucher(offerVoucher1);

            //Assert
            Assert.Equal(expectedTotal, basket.DiscountedTotal);
            Assert.Equal(expectedReason.Message, basket.VoucherResults.First().Message);
        }
    }
}