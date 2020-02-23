
namespace Basket.Task.Business {
    using System;

    public interface IIdentifiable {
        Guid Id { get; }
    }

    public interface ICategorisable {
        ProductCategory Category { get; }
    }

    public class Product
        : IIdentifiable, ICategorisable {

        public Guid Id { get; }
        public ProductCategory Category { get; }
        public decimal Price { get; }

        public Product(Guid id, decimal price)
            : this(id, price, new Uncategorised()) { }

        public Product(Guid id, decimal price, ProductCategory category)
            => (Id, Price, Category) = (id, price, category);
    }

}