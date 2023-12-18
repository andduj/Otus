namespace Otus.Teaching.PromoCodeFactory.DataAccess.Data
{
    public class DbFactory : IDbFactory
    {
        private readonly DataContext _dataContext;

        public DbFactory(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Create()
        {
            _dataContext.Database.EnsureDeleted();
            _dataContext.Database.EnsureCreated();

            _dataContext.AddRange(FakeDataFactory.Employees);
            _dataContext.SaveChanges();
            _dataContext.AddRange(FakeDataFactory.Preferences);
            _dataContext.SaveChanges();
            _dataContext.AddRange(FakeDataFactory.Customers);
            _dataContext.SaveChanges();
        }
    }
}
