namespace Basket.Task.Tests {
    using System.Linq;
    using Business;
    using Business.Services;
    using Business.Vouchers;
    using Xunit;

    public class ScenarioTests
        : VoucherTests {

        [Fact]
        public void Basket1() {
            //Arrange
            var expectedTotal = 60.15m;

            var basket = new Basket();
            basket.Add(ProductService.Get(ProductService.ProductCode.Hat2));
            basket.Add(ProductService.Get(ProductService.ProductCode.Jumper2));

            var giftVoucher1 = VoucherService.Get("XXX-XXX");

            //Act
            basket.ApplyVoucher(giftVoucher1);

            //Assert
            Assert.Equal(expectedTotal, basket.DiscountedTotal);
        }

        [Fact]
        public void Basket2() {
            //Arrange
            var expectedTotal = 51.00m;

            var basket = new Basket();
            basket.Add(ProductService.Get(ProductService.ProductCode.Hat));
            basket.Add(ProductService.Get(ProductService.ProductCode.Jumper));

            var offerVoucher1 = VoucherService.Get("YYY-YYY");
            var expectedReason = new NoApplicableProductsReason(offerVoucher1);

            //Act
            basket.ApplyVoucher(offerVoucher1);

            //Assert
            Assert.Equal(expectedTotal, basket.DiscountedTotal);
            Assert.Equal(expectedReason.Message, basket.VoucherResults.First().Message);
        }

        [Fact]
        public void Basket3() {
            //Arrange
            var expectedTotal = 51.00m;

            var basket = new Basket();
            basket.Add(ProductService.Get(ProductService.ProductCode.Hat));
            basket.Add(ProductService.Get(ProductService.ProductCode.Jumper));
            basket.Add(ProductService.Get(ProductService.ProductCode.HeadLight));

            var offerVoucher1 = VoucherService.Get("YYY-YYY");

            //Act
            basket.ApplyVoucher(offerVoucher1);

            //Assert
            Assert.Equal(expectedTotal, basket.DiscountedTotal);
        }

        [Fact]
        public void Basket4() {
            //Arrange
            var expectedTotal = 41.00m;

            var basket = new Basket();
            basket.Add(ProductService.Get(ProductService.ProductCode.Hat));
            basket.Add(ProductService.Get(ProductService.ProductCode.Jumper));

            var giftVoucher1 = VoucherService.Get("XXX-XXX");
            var offerVoucher1 = VoucherService.Get("ZZZ-ZZZ");

            //Act
            basket.ApplyVoucher(giftVoucher1);
            basket.ApplyVoucher(offerVoucher1);

            //Assert
            Assert.Equal(expectedTotal, basket.DiscountedTotal);
        }

        [Fact]
        public void Basket5() {
            //Arrange
            var expectedTotal = 55.00m;

            var basket = new Basket();
            basket.Add(ProductService.Get(ProductService.ProductCode.Hat));
            basket.Add(ProductService.Get(ProductService.ProductCode.GiftVoucher));

            var offerVoucher1 = VoucherService.Get("ZZZ-ZZZ");
            var expectedReason = new BasketTotalBelowThresholdReason(offerVoucher1, 25.00m);

            //Act
            basket.ApplyVoucher(offerVoucher1);

            //Assert
            Assert.Equal(expectedTotal, basket.DiscountedTotal);
            Assert.Equal(expectedReason.Message, basket.VoucherResults.First().Message);
        }
    }
}