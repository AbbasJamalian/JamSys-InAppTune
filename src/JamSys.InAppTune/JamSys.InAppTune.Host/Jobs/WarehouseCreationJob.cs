using JamSys.InAppTune.Host;
using JamSys.InAppTune.Host.Data;
using JamSys.InAppTune.Host.Jobs;
using LoremNET;
using Microsoft.EntityFrameworkCore;

namespace JamSys.InAppTune.Host.Jobs
{
    public class WarehouseCreationJob : Job
    {
        private readonly IConfiguration _config;
        private readonly Stats _stats;
        private readonly DbContextOptions<TpccContext> _options;

        public WarehouseCreationJob(IConfiguration config, Stats stats, DbContextOptions<TpccContext> options)
        {
            Title = "Warehouse and District Creation";
            _config = config;
            _stats = stats;
            _options = options;
        }
        protected override bool DoWork()
        {
            int numWarehouse = (int)_config.GetValue(typeof(int), "TPCC_Max_Warehouse");

            int warehouseCount = _stats.WarehouseCount;

            if (numWarehouse > warehouseCount)
            {
                using (var repo = new TpccContext(_options))
                {
                    try
                    {
                        Random random = new Random();
                        //create a new warehouse
                        Warehouse warehouse = new Warehouse();
                        warehouse.WId = warehouseCount + 1;

                        warehouse.WName = RandomText.GenerateText(6, 10, false);

                        warehouse.WStreet1 = RandomText.GenerateText(10, 20, true);
                        warehouse.WStreet2 = RandomText.GenerateText(10, 20, true);
                        warehouse.WCity = RandomText.GenerateText(10, 20, true);
                        warehouse.WState = RandomText.GenerateText(2, 2, false);
                        warehouse.WZip = string.Format("{0:d4}", random.Next(1, 9999)) + "11111";
                        warehouse.WTax = ((decimal)random.Next(0, 9999)) / 10000;
                        warehouse.WYtd = 300000;
                        repo.Warehouses.Add(warehouse);

                        //Each warehouse has 10 Districts
                        for (int i = 0; i < 10; i++)
                        {
                            District district = new District();
                            district.DId = (short) (i + 1);

                            district.DWId = warehouse.WId;
                            district.DName = RandomText.GenerateText(6, 10, false);
                            district.DStreet1 = RandomText.GenerateText(10, 20);
                            district.DStreet2 = RandomText.GenerateText(10, 20);
                            district.DCity = RandomText.GenerateText(10, 20);
                            district.DState = RandomText.GenerateText(2, 2);

                            district.DZip = string.Format("{0:d4}", random.Next(1, 9999)) + "11111";

                            district.DTax = ((decimal)random.Next(0, 2000)) / 10000;

                            district.DYtd = 30000;

                            repo.Districts.Add(district);

                        }


                        repo.Database.BeginTransaction();
                        repo.SaveChanges();
                        repo.Database.CommitTransaction();

                        _stats.WarehouseAdded();

                    }
                    catch (Exception ex)
                    {
                        this.Message = ex.Message;
                        repo.Database.RollbackTransaction();
                    }

                    SetCoreDuration(repo.CommulativeLatency);
                }


                return true;
            }
            else
                return false;
        }
    }
}