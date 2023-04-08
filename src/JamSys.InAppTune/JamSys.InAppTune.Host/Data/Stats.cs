using Microsoft.EntityFrameworkCore;

namespace JamSys.InAppTune.Host.Data
{
    public class Stats
    {
        private object lockObject = new object();
        private int _warehouseCount;
        private int _itemCount;
        private int _orderCount;

        private int _customerCount;

        public int WarehouseCount => _warehouseCount;

        public int ItemCount => _itemCount;

        public int OrderCount => _orderCount;

        public int CustomerCount => _customerCount;

        public Stats(DbContextOptions<TpccContext> options)
        {
            try
            {
                using (var repo = new TpccContext(options))
                {
                    repo.Database.EnsureCreated();
                    _warehouseCount = repo.Warehouses.Count();
                    _itemCount = repo.Items.Count();
                    _orderCount = repo.Orders.Count();
                    _customerCount = repo.Customers.Count();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void WarehouseAdded()
        {
            Interlocked.Increment(ref _warehouseCount);
        }

        public void ItemAdded()
        {
            Interlocked.Increment(ref _itemCount);
        }

        public void OrderAdded()
        {
            Interlocked.Increment(ref _orderCount);
        }

        public void CustomerAdded()
        {
            Interlocked.Increment(ref _customerCount);
        }

    }
}
