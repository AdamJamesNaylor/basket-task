namespace Basket.Task.Business {
    using System;

    public class Uncategorised
        : ProductCategory {

        public Uncategorised()
            : base(new Guid("cb1f3d30-4796-432f-bc71-9de359354d52"), "Uncategorised") { }
    }

    public class ProductCategory
        : IIdentifiable {
        public ProductCategory(Guid guid, string description)
            => (Id, Description) = (guid, description);

        public Guid Id { get; }
        public string Description { get; }

        public static ProductCategory HeadGear =>
            new ProductCategory(new Guid("92920e37-e5de-44cd-ae28-f6ba5915d146"), "Head Gear");
        public static ProductCategory Voucher =>
            new ProductCategory(new Guid("a1ab8b0f-bbdb-4a9a-89f6-1e5dac623730"), "Voucher");

        public override bool Equals(object o) {
            if (!(o is ProductCategory category))
                return false;

            return category.Id.Equals(Id);
        }

        protected bool Equals(ProductCategory other) {
            return other != null && Id.Equals(other.Id);
        }

        public override int GetHashCode() {
            return Id.GetHashCode();
        }
    }
}