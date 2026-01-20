Console.WriteLine("Hello, World!");

//SingletonClass singletonClass1 = SingletonClass.Instance;
//singletonClass1.VerifyTCNumber("111");

//IServiceCollection services = new ServiceCollection();
//services.AddSingleton<DISingletonClass>();

//var srv = services.BuildServiceProvider();

//DISingletonClass dISingletonClass = srv.GetRequiredService<DISingletonClass>();
//dISingletonClass.VerifyTCNumber("222");

//StaticSingletonClass.VerifyTCNumber("333");

//ExtensionsSingletonClass.VerifyTCNumber("444");
//string tcNo = "444";
//tcNo.VerifyTCNumber();

#region Old Version
class SingletonClass
{
    //private static SingletonClass? _instance;
    private static readonly object _lock = new();
    public static SingletonClass Instance
    {
        get
        {
            //if (_instance is null)
            //{
            //    _instance = new();
            //}
            //return _instance;

            //.NET 10 çözümü
            //if (field is null) 
            //{
            //    field = new SingletonClass();
            //}
            //return field;

            //with lock
            lock (_lock)
            {
                if (field is null)
                {
                    field = new SingletonClass();
                }
                return field;
            }


        }
    }
    private SingletonClass()
    {

    }
    public bool VerifyTCNumber(string tcNo)
    {
        Console.WriteLine("{0} TC no is {1}", tcNo, true);
        return true;
    }
}

#endregion

#region Güncel Version
class DISingletonClass
{
    public bool VerifyTCNumber(string tcNo)
    {
        Console.WriteLine("{0} TC no is {1}", tcNo, true);
        return true;
    }
}

#endregion

#region Static Class version
static class StaticSingletonClass
{
    public static bool VerifyTCNumber(string tcNo)
    {
        Console.WriteLine("{0} TC no is {1}", tcNo, true);
        return true;
    }
}
#endregion

#region Extensions Version

static class ExtensionsSingletonClass
{
    public static bool VerifyTCNumber(this string tcNo)
    {
        Console.WriteLine("{0} TC no is {1}", tcNo, true);
        return true;
    }
}

#endregion

#region Extensions .NET 10 Version

static class ExtensionsNET10SingletonClass
{
    extension(string tcNo)
    {
        public bool VerifyTCNumber()
        {
            Console.WriteLine("{0} TC no is {1}", tcNo, true);
            return true;
        }
    }
}

#endregion