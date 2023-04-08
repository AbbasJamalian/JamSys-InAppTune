using JamSys.InAppTune.Agent;
using JamSys.InAppTune.Host.Data;
using Microsoft.EntityFrameworkCore;

namespace JamSys.InAppTune.Host.Jobs
{
    public class StockLevelTransaction : Job
    {
        private readonly DbContextOptions<TpccContext> _options;

        public StockLevelTransaction(DbContextOptions<TpccContext> options)
        {
            _options = options;
            this.Title = "Stock-Level Transaction";
        }
        protected override bool DoWork()
        {
            int numTasks = NumParallelExecutions;
            //SetCoreDuration(PerformQuery(0));

            TuningAgent.Instance.StartWorkload();
            
            List<Task<long>> taskList = new();

            for (int i = 0; i < numTasks; i++)
            {
                var task = new Task<long>(() => PerformQuery(i));
                taskList.Add(task);
                task.Start();
            }

            Task.WaitAll(taskList.ToArray());
            long latency = 0;

            taskList.ForEach(t => latency += t.Result);
            latency /= numTasks;
            SetCoreDuration(latency);

            float throughput = latency != 0 ? (1000f * numTasks) / latency : 0;

            TuningAgent.Instance.EndWorkload((ulong)latency, throughput);

            return base.DoWork();
        }

        private long PerformQuery(int index)
        {
            long latency = 0;
            try
            {
                using (var repo = new TpccContext(_options))
                {
                    var query = from stock in repo.Stocks
                                from orderLine in repo.OrderLines 
                                where stock.SQuantity < 50
                                && orderLine.OlOId < 20
                                && stock.SWId == orderLine.OlWId
                                select stock;

                    //                            
                    /*
                    var query = from orderline in repo.OrderLines
                                join item in repo.Items
                                on orderline.OlIId equals item.IId
                                orderby item.IName
                                select new { orderline, item };
                    */


                    var count = query.ToList().Distinct().Count();
                    latency = repo.CommulativeLatency;
                }
                
            }
            catch (Exception ex)
            {
                this.Message = ex.Message;
                latency = 0; //to make sure a negative reward                
            }
            return latency;
        }
    }
}
