using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Abstract Factory Pattern");

#region Manuel
//var dbContext = new MSSqlDBContext();
//var repository = new Repository(dbContext);
//var unitOfWork = new UnitOfWork(dbContext);
//ProductService productService = new(repository, unitOfWork);
//productService.Create();
#endregion

#region DI Çözümü
var services = new ServiceCollection();
services.AddScoped<IDbContext, MSSqlDBContext>();
//services.AddKeyedScoped<IDbContext, PostgreSqlDBContext>("postgresql");
services.AddTransient<Repository>();
services.AddTransient<UnitOfWork>();
services.AddTransient<ProductService>();
services.AddKeyedScoped<ProductService>(DatabaseTypeEnum.MSSql, (factory, key) =>
{
    IDbContext _dbContext;
    Repository _repository;
    UnitOfWork _unitOfWork;
    switch (key)
    {
        case DatabaseTypeEnum.MSSql:
            _dbContext = new MSSqlDBContext();
            break;
        case DatabaseTypeEnum.PostgreSql:
            _dbContext = new PostgreSqlDBContext();
            break;
        default:
            throw new ArgumentNullException(nameof(key));
    }

    _repository = new(_dbContext);
    _unitOfWork = new(_dbContext);
    return new(_repository, _unitOfWork);
});

var srv = services.BuildServiceProvider();
//var productService = srv.GetRequiredService<ProductService>();
var productService = srv.GetRequiredKeyedService<ProductService>(DatabaseTypeEnum.MSSql);
productService.Create();
#endregion

#region Abstract Factory
//ProductAbstractFactory productAbstractFactory = new(DatabaseTypeEnum.MSSql);
//productAbstractFactory.productService.Create();
#endregion

#region Initialize
interface IDbContext
{
    void Connect();
}
class MSSqlDBContext : IDbContext
{
    public MSSqlDBContext()
    {
        Console.WriteLine("MSSQL Db Context constructor");
    }
    public void Connect()
    {
        Console.WriteLine("Connected the MSSql Db");
    }
}

class PostgreSqlDBContext : IDbContext
{
    public PostgreSqlDBContext()
    {
        Console.WriteLine("PostgreSql Db Context constructor");
    }
    public void Connect()
    {
        Console.WriteLine("Connected the PostgreSql Db");
    }
}

class Repository
{
    private readonly IDbContext _dBContext;
    public Repository(IDbContext dbContext)
    {
        _dBContext = dbContext;
    }

    public void Add()
    {
        _dBContext.Connect();
        Console.WriteLine("I added new data");
    }
}

class UnitOfWork
{
    private readonly IDbContext _dBContext;
    public UnitOfWork(IDbContext dbContext)
    {
        _dBContext = dbContext;
    }

    public void SaveChanges()
    {
        _dBContext.Connect();
        Console.WriteLine("I saved the data");
    }
}

class ProductService
{
    private readonly Repository _repository;
    private readonly UnitOfWork _unitOfWork;

    public ProductService(Repository repository, UnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    public void Create()
    {
        Console.WriteLine("Business logic...");

        _repository.Add();
        _unitOfWork.SaveChanges();

        Console.WriteLine("I completed create operation");
    }
}

enum DatabaseTypeEnum
{
    MSSql,
    PostgreSql
}

#endregion

#region Old Version
class ProductAbstractFactory
{
    public readonly ProductService productService;
    private readonly Repository _repository;
    private readonly UnitOfWork _unitOfWork;
    private readonly IDbContext _dbContext;
    public ProductAbstractFactory(DatabaseTypeEnum dbType)
    {
        switch (dbType)
        {
            case DatabaseTypeEnum.MSSql:
                _dbContext = new MSSqlDBContext();
                break;
            case DatabaseTypeEnum.PostgreSql:
                _dbContext = new PostgreSqlDBContext();
                break;
            default:
                throw new ArgumentNullException(nameof(dbType));
        }

        _repository = new(_dbContext);
        _unitOfWork = new(_dbContext);
        productService = new(_repository, _unitOfWork);
    }
}
#endregion