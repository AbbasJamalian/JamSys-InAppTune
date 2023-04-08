using JamSys.InAppTune.Agent;
using JamSys.InAppTune.Host.Data;
using Microsoft.EntityFrameworkCore;

namespace JamSys.InAppTune.Host.Jobs
{
    public class ReadWriteWorkloadJob : Job
    {
        private readonly DbContextOptions<TpccContext> _options;
        private bool _hasError;

        public ReadWriteWorkloadJob(DbContextOptions<TpccContext> options)
        {
            _options = options;
            this.Title = "Read-Write Workload";
        }
        protected override bool DoWork()
        {
            int numTasks = NumParallelExecutions;
            //SetCoreDuration(PerformQuery(0));

            _hasError = false;
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

            if(_hasError)
            {
                latency = 0;
                throughput = 0;
            }

            TuningAgent.Instance.EndWorkload((ulong)latency, throughput);

            return base.DoWork();
        }

        private long PerformQuery(int index)
        {
            long latency = 0;

            try
            {
                this.Message = string.Empty;
                using (var repo = new TpccContext(_options))
                {
                    var query = from order in repo.Orders
                                join orderline in repo.OrderLines
                                on order.OId equals orderline.OlOId
                                join item in repo.Items
                                on orderline.OlIId equals item.IId
                                where order.OOlCnt < JobParameter
                                orderby item.IName
                                select new { order, orderline, item };


                    var list = query.Take(JobLimit).ToList();
                    Console.WriteLine($"Query returned {list.Count} lines with limit {JobLimit}");

                    for (int x = 0; x < list.Count && x < 1000; x++)
                    {
                        var listItem = list[x];
                        listItem.orderline.OlDistInfo = RandomText.GenerateText(24, 24);
                        repo.OrderLines.Update(listItem.orderline);
                    }
                    repo.SaveChanges();
                    latency = repo.CommulativeLatency;
                }

            }
            catch (Exception ex)
            {
                this.Message = ex.Message;
                latency = 0; //to make sure a negative reward                
                _hasError = true;
            }
            return latency;
        }
    }
}
