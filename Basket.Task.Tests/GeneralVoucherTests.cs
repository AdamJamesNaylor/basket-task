namespace Basket.Task.Tests {
    using System.Linq;
    using Business;
    using Business.Services;
    using Business.Vouchers;
    using Xunit;

    public class VoucherTests {
        protected readonly VoucherService VoucherService;
        protected readonly ProductService ProductService;

        public VoucherTests() {
            VoucherService = new VoucherService();
            ProductService = new ProductService();
        }
    }

    public class GeneralVoucherTests
        : VoucherTests {

        [Fact]
        public void CannotApplyTheSameVoucherTwice() {
            //Arrange
            var expectedTotal = 46.00m;

            var basket = new Basket();
            basket.Add(ProductService.Get(ProductService.ProductCode.Hat));
            basket.Add(ProductService.Get(ProductService.ProductCode.Jumper));

            var giftVoucher1 = VoucherService.Get("XXX-XXX");

            //Act
            basket.ApplyVoucher(giftVoucher1);
            basket.ApplyVoucher(giftVoucher1);

            //Assert
            Assert.Equal(expectedTotal, basket.DiscountedTotal);
        }

        [Fact]
        public void OfferAndGiftVouchersCanBeUsedInConjunction() {
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
        public void InvalidatedVouchersDisplayCorrectMessage() {
            //Arrange
            var expectedTotal = 46.00m;

            var headlight = ProductService.Get(ProductService.ProductCode.HeadLight);

            var basket = new Basket();
            basket.Add(ProductService.Get(ProductService.ProductCode.Hat));
            basket.Add(ProductService.Get(ProductService.ProductCode.Jumper));
            basket.Add(headlight);

            var offerVoucher1 = VoucherService.Get("YYY-YYY");
            var expectedReason = new NoApplicableProductsReason(offerVoucher1 as OfferVoucher);

            //Act
            basket.ApplyVoucher(offerVoucher1);
            Assert.DoesNotContain(basket.VoucherResults, r => r is NoApplicableProductsReason);
            basket.Remove(headlight);

            //Assert
            Assert.Equal(expectedReason.Message, basket.VoucherResults.First().Message);

        }

        [Fact]
        public void DiscountCannotBeNegative() {
            //Arrange
            var expectedTotal = 0.00m;

            var basket = new Basket();
            basket.Add(ProductService.Get(ProductService.ProductCode.HeadLight));

            var offerVoucher1 = VoucherService.Get("XXX-XXX");

            //Act
            basket.ApplyVoucher(offerVoucher1);

            //Assert
            Assert.Equal(expectedTotal, basket.DiscountedTotal);
        }

        [Fact]
        public void AddingAnItemToTheBasketAmendsDiscountRetroactively() {
            //Arrange
            var expectedTotal = 51.00m;

            var basket = new Basket();
            basket.Add(ProductService.Get(ProductService.ProductCode.Hat));
            basket.Add(ProductService.Get(ProductService.ProductCode.Jumper));

            var offerVoucher1 = VoucherService.Get("YYY-YYY");

            //Act
            basket.ApplyVoucher(offerVoucher1);
            basket.Add(ProductService.Get(ProductService.ProductCode.HeadLight));

            //Assert
            Assert.Equal(expectedTotal, basket.DiscountedTotal);
        }

        [Fact]
        public void RemovingAnItemFromTheBasketAmendsDiscountRetroactively() {
            //Arrange
            var expectedTotal = 51.00m;

            var headlight = ProductService.Get(ProductService.ProductCode.HeadLight);

            var basket = new Basket();
            basket.Add(ProductService.Get(ProductService.ProductCode.Hat));
            basket.Add(ProductService.Get(ProductService.ProductCode.Jumper));
            basket.Add(headlight);

            var offerVoucher1 = VoucherService.Get("YYY-YYY");

            //Act
            basket.ApplyVoucher(offerVoucher1);
            basket.Remove(headlight);

            //Assert
            Assert.Equal(expectedTotal, basket.DiscountedTotal);
        }

    }

}