using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSingleton<DISingletonClass>();

var app = builder.Build();

#region Singleton DI
app.MapGet("/singleton-di", ([FromServices] DISingletonClass dISingletonClass) =>
{
    var res = dISingletonClass.VerifyTCNumber("111");

    return res;
});
#endregion

app.Run();

#region Singleton DI
class DISingletonClass
{
    public bool VerifyTCNumber(string tcNo)
    {
        Console.WriteLine("{0} TC no is {1}", tcNo, true);
        return true;
    }
}
#endregion



//Design Principle
//Yazılım tasarlarken uyman gereken temel kurallar / felsefeler / rehberler
//Daha esnek
//Daha bakımı kolay
//Daha genişletilebilir
//Daha test edilebilir
//SOLID, DRY, KISS, YAGNI, Separation of Concerns

//🧩 Principle vs Pattern farkı
//Şey	Ne?
//Principle	“Nasıl düşünmeliyim?”
//Pattern	“Bu problemi nasıl çözerim?”
//Framework	“Bunu hazır veriyorum, kullan”

//🏗️ Architectural Pattern nedir?

//Architectural Pattern = Uygulamanın genel iskeletini ve katmanlı yapısını tanımlayan büyük ölçekli tasarım şablonudur.

//Yani:

//❌ Bir class’ın içi değil

//❌ Bir metodun nasıl yazıldığı değil

//✅ Sistemin tamamı nasıl organize edilir? sorusunun cevabı