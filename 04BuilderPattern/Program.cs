Console.WriteLine("Builder Pattern");

//MailService mailService = new MailService();
//mailService.Send("tanersaydam@gmail.com", "info@destek.com", "Test", "Hello world!");

MailServiceBuilder mailServiceBuilder = new();
mailServiceBuilder
    .To("tanersaydam@gmail.com")
    .From("info@destek.com")
    .Subject("Test")
    .Body("Hello world!")
    .Send();

class MailService
{
    public void Send(string to, string from, string subject, string body)
    {
        Console.WriteLine("Email detail:\nTo: {0}\nFrom: {1}\nSubject: {2}\nBody: {3}\n\nSending email...", to, from, subject, body);
    }
}

class MailServiceBuilder
{
    private string? _to;
    private string? _from;
    private string? _subject;
    private string? _body;

    public MailServiceBuilder To(string to)
    {
        _to = to;
        return this;
    }

    public MailServiceBuilder From(string from)
    {
        _from = from;
        return this;
    }

    public MailServiceBuilder Subject(string subject)
    {
        _subject = subject;
        return this;
    }

    public MailServiceBuilder Body(string body)
    {
        _body = body;
        return this;
    }

    public void Send()
    {
        Console.WriteLine("Email detail:\nTo: {0}\nFrom: {1}\nSubject: {2}\nBody: {3}\n\nSending email...", _to ?? "", _from ?? "", _subject ?? "", _body ?? "");
    }
}