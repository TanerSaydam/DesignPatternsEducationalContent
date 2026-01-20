Console.WriteLine("Facade Pattern");

//Karmaşık iş süreçlerini bir ara class ile gizleyip istek tek yerden tek bir objeye yapılsın o obje arka planda karmaşıklığı yönetsin amaçlar

#region Problem
//ProductSystem productSystem = new();
//bool isHaveProduct = productSystem.IsHaveProduct("Masaüstü Bilgisayar", 1);

//if (isHaveProduct)
//{
//    OrderSystem orderSystem = new();
//    bool orderIsSuccesful = orderSystem.CreateOrder("Masaüstü Bilgisayar", 1);

//    if (orderIsSuccesful)
//    {
//        PaymentSystem paymentSystem = new();
//        bool paymentIsSuccessful = paymentSystem.Pay("Taner Saydam", "111", 500);
//        if (paymentIsSuccessful)
//        {
//            DeliverySystem deliverySystem = new();
//            bool deliveryIsSuccessful = deliverySystem.ScheduleDelivery("Taner Saydam", "Masaüstü Bilgisayar", 1, "Kayseri");

//            if (deliveryIsSuccessful)
//            {
//                Console.WriteLine("Order is completed");
//            }
//            else
//            {
//                throw new ArgumentException("Something went wrong!");
//            }
//        }

//        else
//        {
//            throw new ArgumentException("Something went wrong!");
//        }
//    }
//    else
//    {
//        throw new ArgumentException("Something went wrong!");
//    }
//}
//else
//{
//    throw new ArgumentException("Something went wrong!");
//}
#endregion

#region Solution
ProductFacade productFacade = new();
productFacade.BuyProduct("Masaüstü Bilgisayar", 1, "Taner Saydam", "Kayseri", "111", 500);
#endregion


#region Initialize
class ProductSystem
{
    public bool IsHaveProduct(string productName, int quantity)
    {
        return true; //Eğer elimizde ürün varsa
    }
}

class OrderSystem
{
    public bool CreateOrder(string productName, int quantity)
    {
        Console.WriteLine("We created order for {0}", productName);
        return true;
    }
}

class PaymentSystem
{
    public bool Pay(string customerName, string cardNumber, decimal amount)
    {
        Console.WriteLine("{0}, payment is successful for {1}$ ", customerName, amount);
        return true;
    }
}

class DeliverySystem
{
    public bool ScheduleDelivery(string customerName, string product, int quantity, string address)
    {
        Console.WriteLine("We scheduled {0} quantity {1} product for {2}. Delivery address: {3}", product, quantity, customerName, address);
        return true;
    }
}

#endregion

#region Facade Pattern
class ProductFacade
{
    public bool BuyProduct(string productName, int quantity, string customerName, string address, string cardNumber, decimal amount)
    {
        ProductSystem productSystem = new();
        bool isHaveProduct = productSystem.IsHaveProduct(productName, quantity);

        if (isHaveProduct)
        {
            OrderSystem orderSystem = new();
            bool orderIsSuccesful = orderSystem.CreateOrder(productName, quantity);

            if (orderIsSuccesful)
            {
                PaymentSystem paymentSystem = new();
                bool paymentIsSuccessful = paymentSystem.Pay(customerName, cardNumber, amount);
                if (paymentIsSuccessful)
                {
                    DeliverySystem deliverySystem = new();
                    bool deliveryIsSuccessful = deliverySystem.ScheduleDelivery(customerName, productName, quantity, address);

                    if (deliveryIsSuccessful)
                    {
                        Console.WriteLine("Order is completed");
                        return true;
                    }
                }
            }
        }

        return false;
    }
}
#endregion