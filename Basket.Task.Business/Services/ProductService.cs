namespace Basket.Task.Business.Services {
    using System;

    public class ProductService {
        public enum ProductCode {
            Hat,
            Hat2,
            Jumper,
            Jumper2,
            HeadLight, 
            GiftVoucher
        }

        public Product Get(ProductCode productCode) {
            switch (productCode) {
                case ProductCode.Hat:
                    return new Product(Guid.NewGuid(), 25.00m);
                case ProductCode.Hat2:
                    return new Product(Guid.NewGuid(), 10.50m);
                case ProductCode.Jumper:
                    return new Product(Guid.NewGuid(), 26.00m);
                case ProductCode.Jumper2:
                    return new Product(Guid.NewGuid(), 54.65m);
                case ProductCode.HeadLight:
                    return new Product(Guid.NewGuid(), 3.50m, ProductCategory.HeadGear);
                case ProductCode.GiftVoucher:
                    return new Product(Guid.NewGuid(), 30.00m, ProductCategory.Voucher);
                default:
                    throw new ArgumentException($"Unknown product code {productCode}");
            }
        }
    }
}