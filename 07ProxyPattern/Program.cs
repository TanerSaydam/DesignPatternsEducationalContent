Console.WriteLine("Proxy Pattern");

// Asıl nesneye gitmeden önce araya bir kontrol katmanı koymanı önerir (Authentcation, Authorization, Validation, Log, Cache vb)

#region Problem
//ProductRepository productRepository = new();
//productRepository.Create(string.Empty); //Ürün adı boş olmamalı
#endregion

#region Solution
ProductProxy productProxy = new();
productProxy.CreateProduct(string.Empty);
#endregion

#region Initialize
class ProductRepository
{
    public void Create(string productName)
    {
        Console.WriteLine("I created product");
    }
}
#endregion

#region Proxy Pattern
class ProductProxy
{
    public void CreateProduct(string productName)
    {
        if (string.IsNullOrWhiteSpace(productName))
        {
            throw new ArgumentNullException(nameof(productName));
        }

        ProductRepository productRepository = new();
        productRepository.Create(productName);
    }
}
#endregion