using JamSys.InAppTune.Host.Data;
using LoremNET;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace JamSys.InAppTune.Host.Jobs
{
    public class ItemCreationJob : Job
    {
        private readonly IConfiguration _config;
        private readonly Stats _stats;
        private readonly DbContextOptions<TpccContext> _options;

        public ItemCreationJob(IConfiguration config, Stats stats, DbContextOptions<TpccContext> options)
        {
            Title = "Item and Stock Creation";
            _config = config;
            _stats = stats;
            _options = options;
        }
        protected override bool DoWork()
        {
            long openDb = 0;
            long beforeTransaction = 0;
            long afterTransaction = 0;
            long beforeWarehouseList = 0;
            int numItems = (int)_config.GetValue(typeof(int), "TPCC_Max_Items");

            int itemCount = _stats.ItemCount;

            if (numItems > itemCount)
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();
                using (var repo = new TpccContext(_options))
                {
                    var warehouseList = repo.Warehouses;

                    openDb = watch.ElapsedMilliseconds;
                    try
                    {
                        for (int index = 0; index < 100; index++)
                        {
                            Item item = new Item();

                            item.IId = itemCount + 1 + index;
                            item.IImId = 0;
                            item.IName = RandomText.GenerateText(14, 24, false);

                            Random rand = new Random();
                            int price = rand.Next(100, 10000);
                            item.IPrice = ((decimal)price) / 100;

                            item.IData = RandomText.GenerateText(26, 50, true);
                            //in 10% of cases, the text "ORIGINAL" must be at a random place in the text
                            if (LoremNET.Lorem.Chance(10, 100))
                            {
                                item.IData = RandomText.InsertRandom(item.IData, "ORIGINAL");
                            }
                            repo.Items.Add(item);

                            beforeWarehouseList = watch.ElapsedMilliseconds;
                            foreach (var warehouse in warehouseList)
                            {
                                Stock stock = new Stock();
                                stock.SIId = item.IId;
                                stock.SWId = warehouse.WId;
                                stock.SQuantity = (short)rand.Next(10, 100);
                                stock.SDist01 = RandomText.GenerateText(24, 24, false);
                                stock.SDist02 = RandomText.GenerateText(24, 24, false);
                                stock.SDist03 = RandomText.GenerateText(24, 24, false);
                                stock.SDist04 = RandomText.GenerateText(24, 24, false);
                                stock.SDist05 = RandomText.GenerateText(24, 24, false);
                                stock.SDist06 = RandomText.GenerateText(24, 24, false);
                                stock.SDist07 = RandomText.GenerateText(24, 24, false);
                                stock.SDist08 = RandomText.GenerateText(24, 24, false);
                                stock.SDist09 = RandomText.GenerateText(24, 24, false);
                                stock.SDist10 = RandomText.GenerateText(24, 24, false);
                                stock.SYtd = 0;
                                stock.SOrderCnt = 0;
                                stock.SRemoteCnt = 0;

                                stock.SData = RandomText.GenerateText(26, 50, true);
                                //in 10% of cases, the text "ORIGINAL" must be at a random place in the text
                                if (LoremNET.Lorem.Chance(10, 100))
                                {
                                    stock.SData = RandomText.InsertRandom(stock.SData, "ORIGINAL");
                                }

                                repo.Stocks.Add(stock);
                            }

                            _stats.ItemAdded();
                        }
                        beforeTransaction = watch.ElapsedMilliseconds;
                        repo.SaveChanges();
                        afterTransaction = watch.ElapsedMilliseconds;

                        SetCoreDuration(repo.CommulativeLatency);
                    }
                    catch (Exception ex)
                    {
                        //repo.Database.RollbackTransaction();
                        this.Message = ex.Message;
                    }
                }
                watch.Stop();
                Console.WriteLine($"ItemCreation, OpenDb:{openDb}, Item:{beforeWarehouseList}, BeforTx:{beforeTransaction}, AfterTx:{afterTransaction}");
                return true;
            }
            else
                return false;
        }
    }
}