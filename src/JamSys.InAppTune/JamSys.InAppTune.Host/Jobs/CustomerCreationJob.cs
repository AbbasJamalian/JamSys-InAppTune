
using JamSys.InAppTune.Host.Data;
using LoremNET;
using Microsoft.EntityFrameworkCore;

namespace JamSys.InAppTune.Host.Jobs
{
    public class CustomerCreationJob : Job
    {
        private readonly IConfiguration _config;
        private readonly Stats _stats;
        private readonly DbContextOptions<TpccContext> _options;

        public CustomerCreationJob(IConfiguration config, Stats stats, DbContextOptions<TpccContext> options)
        {
            Title = "Customer and Order Creation";
            _config = config;
            _stats = stats;
            _options = options;
        }
        protected override bool DoWork()
        {
            int numCustomers = (int)_config.GetValue(typeof(int), "TPCC_Customer_Per_District");

            using (var repo = new TpccContext(_options))
            {
                //Find a district which has less customers than exected
                //var district = repo.Districts.Where(d => repo.Customers.Where(c => c.CDId.Equals(d.DId)).Count() < numCustomers).FirstOrDefault();

                District district = null;
                int customerCount = repo.Customers.Count();

                var districtList = repo.Districts.ToList();
                foreach (var tempDistrict in districtList)
                {
                    int currentCustomers = repo.Customers.Where(c => c.CDId == tempDistrict.DId && c.CWId == tempDistrict.DWId).Count();
                    if (currentCustomers < numCustomers)
                    {
                        district = tempDistrict;
                        break;
                    }
                }

                if (district != null)
                {
                    try
                    {
                        //repo.Database.BeginTransaction();

                        Random random = new Random();
                        //int customerId = repo.Customers.Where(c => c.CDId.Equals(district.DId)).Count() + 1;
                        int customerId;
                        for (int index = 0; index < 50; index++)
                        {
                            customerId = customerCount + index + 1;
                            Customer customer = new Customer();
                            customer.CId = customerId;
                            customer.CDId = district.DId;
                            customer.CWId = district.DWId;

                            customer.CLast = GetLastName();

                            customer.CMiddle = "OE";
                            customer.CFirst = RandomText.GenerateText(8, 16, false);
                            customer.CStreet1 = RandomText.GenerateText(10, 20);
                            customer.CStreet2 = RandomText.GenerateText(10, 20);
                            customer.CCity = RandomText.GenerateText(10, 20);
                            customer.CState = RandomText.GenerateText(2, 2);
                            customer.CZip = string.Format("{0:d4}", random.Next(1, 9999)) + "11111";

                            customer.CPhone = GetPhoneNumber();

                            customer.CSince = DateTime.UtcNow;

                            customer.CCredit = "GC";
                            if (LoremNET.Lorem.Chance(10, 100))
                            {
                                customer.CCredit = "BC";
                            }

                            customer.CCreditLim = 50000;
                            customer.CDiscount = ((decimal)random.Next(0, 5000)) / 10000;
                            customer.CBalance = -10;
                            customer.CYtdPayment = 10;
                            customer.CPaymentCnt = 1;
                            customer.CDeliveryCnt = 0;
                            customer.CData = RandomText.GenerateText(300, 500);

                            repo.Customers.Add(customer);

                            //CreateRandomHistory(repo, customer);

                            CreateRandomOrder(repo, customer, customerId);

                            _stats.CustomerAdded();
                        }
                        repo.SaveChanges();
                        //repo.Database.CommitTransaction();

                    }
                    catch (Exception ex)
                    {
                        //repo.Database.RollbackTransaction();
                        this.Message = ex.Message;
                    }
                    return true;
                }
                else
                    return false;
            }
        }

        private void CreateRandomHistory(TpccContext repo, Customer customer)
        {
            History history = new History();

            history.HCId = customer.CId;
            history.HCDId = customer.CDId;
            history.HDId = history.HCDId;
            history.HCWId = customer.CWId;
            history.HWId = history.HCWId;
            history.HDate = DateTime.UtcNow;
            history.HAmount = 10;
            history.HData = RandomText.GenerateText(12, 24);

            repo.Histories.Add(history);


        }
        private string GetPhoneNumber()
        {
            string result = string.Empty;
            Random rand = new Random();

            for (int i = 0; i < 4; i++)
            {
                result += rand.Next(1000, 9999).ToString();
            }
            return result;
        }

        private string GetLastName()
        {
            string name = string.Empty;
            string[] syllabes = { "BAR", "OUGHT", "ABLE", "PRI", "PRES", "ESE", "ANTI", "CALLY", "ATION", "EING" };

            Random rand = new Random();
            int code = rand.Next(0, 999);

            do
            {
                int index = code % 10;
                name += syllabes[index];
                code = code / 10;
            } while (code > 0);

            return name;
        }

        private Order CreateRandomOrder(TpccContext repo, Customer customer, int orderCount)
        {
            Random random = new Random();
            int numOrderLines = random.Next(5, 15);

            Order order = new Order();
            order.OId = customer.CId;
            order.OCId = customer.CId;
            order.ODId = customer.CDId;
            order.OWId = customer.CWId;
            order.OEntryD = DateTime.UtcNow;

            if (orderCount < 2101)
                order.OCarrierId = (short)random.Next(1, 10);
            else
                order.OCarrierId = null;
            order.OOlCnt = (short)numOrderLines;
            order.OAllLocal = 1;
            repo.Orders.Add(order);

            for (int index = 0; index < numOrderLines; index++)
            {
                OrderLine orderLine = new OrderLine();
                orderLine.OlOId = order.OId;
                orderLine.OlDId = order.ODId;
                orderLine.OlWId = order.OWId;
                orderLine.OlNumber = (short)(index + 1);
                orderLine.OlIId = GetRandomItem(repo).IId;
                orderLine.OlSupplyWId = order.OWId;

                if (orderCount < 2101)
                    orderLine.OlDeliveryD = DateTime.UtcNow;
                else
                    orderLine.OlDeliveryD = null;

                orderLine.OlQuantity = 5;

                if (orderCount < 2101)
                    orderLine.OlAmount = 0;
                else
                    orderLine.OlAmount = ((decimal)random.Next(1, 999999)) / 100;
                orderLine.OlDistInfo = RandomText.GenerateText(24, 24);

                repo.OrderLines.Add(orderLine);
            }

            if (orderCount >= 2101)
            {
                NewOrder newOrder = new NewOrder();
                newOrder.NoOId = order.OId;
                newOrder.NoDId = order.ODId;
                newOrder.NoWId = order.OWId;

                repo.NewOrders.Add(newOrder);
            }
            _stats.OrderAdded();
            return order;
        }

        private Item GetRandomItem(TpccContext repo)
        {
            Item item = null;

            Random random = new Random();

            var id = random.Next(1, _stats.ItemCount);

            item = repo.Items.Where(i => i.IId.Equals(id)).FirstOrDefault();

            return item;
        }


    }
}