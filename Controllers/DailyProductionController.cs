using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DailyProduction.Model;
using Azure.Data.Tables;
using Azure;

namespace IbasAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DailyProductionController : ControllerBase
    {

        //private List<DailyProductionDTO> _productionRepo;
        private readonly ILogger<DailyProductionController> _logger;
        TableClient client;


        TableServiceClient serviceClient = new TableServiceClient("DefaultEndpointsProtocol=https;AccountName=ibasstoragejacob;AccountKey=894lj5M/t2RBs8xo26wSzZZfklUr1O4u2pqGsuG3E5fqCS5p+JrxnO5il6YWRhjNvJtS0Un96JEG+AStp3d9yQ==;EndpointSuffix=core.windows.net");


        string MyPK = "PartitionKey";
        string MyRK = "RowKey";

        public DailyProductionController(ILogger<DailyProductionController> logger)
        {
            //client = new TableClient("DefaultEndpointsProtocol=https;AccountName=ibasstoragejacob;AccountKey=894lj5M/t2RBs8xo26wSzZZfklUr1O4u2pqGsuG3E5fqCS5p+JrxnO5il6YWRhjNvJtS0Un96JEG+AStp3d9yQ==;EndpointSuffix=core.windows.net", "IBASProduktion2020");

            client = new TableClient(
                new Uri("https://ibasstoragejacob.table.core.windows.net/IBASProduktion2020"),
                "IBASProduktion2020",
                new TableSharedKeyCredential("ibasstoragejacob", "894lj5M/t2RBs8xo26wSzZZfklUr1O4u2pqGsuG3E5fqCS5p+JrxnO5il6YWRhjNvJtS0Un96JEG+AStp3d9yQ==\n"));

           

            _logger = logger;
        //    _productionRepo = new List<DailyProductionDTO>
        //    {
        //        new DailyProductionDTO {Date = new DateTime(2020, 1, 31), Model = BikeModel.IBv1, ItemsProduced = 12},
        //        new DailyProductionDTO {Date = new DateTime(2020, 2, 28), Model = BikeModel.IBv1, ItemsProduced = 32},
        //        new DailyProductionDTO {Date = new DateTime(2020, 3, 31), Model = BikeModel.IBv1, ItemsProduced = 32},
        //        new DailyProductionDTO {Date = new DateTime(2020, 4, 30), Model = BikeModel.IBv1, ItemsProduced = 141},
        //        new DailyProductionDTO {Date = new DateTime(2020, 5, 31), Model = BikeModel.IBv1, ItemsProduced = 146},
        //        new DailyProductionDTO {Date = new DateTime(2020, 6, 30), Model = BikeModel.IBv1, ItemsProduced = 162},
        //        new DailyProductionDTO {Date = new DateTime(2020, 7, 31), Model = BikeModel.IBv1, ItemsProduced = 102},
        //        new DailyProductionDTO {Date = new DateTime(2020, 8, 31), Model = BikeModel.IBv1, ItemsProduced = 210},
        //        new DailyProductionDTO {Date = new DateTime(2020, 9, 30), Model = BikeModel.IBv1, ItemsProduced = 144},
        //        new DailyProductionDTO {Date = new DateTime(2020, 10, 31), Model = BikeModel.IBv1, ItemsProduced = 151},
        //        new DailyProductionDTO {Date = new DateTime(2020, 11, 30), Model = BikeModel.IBv1, ItemsProduced = 61},
        //        new DailyProductionDTO {Date = new DateTime(2020, 12, 31), Model = BikeModel.IBv1, ItemsProduced = 86},

        //        new DailyProductionDTO {Date = new DateTime(2020, 1, 31), Model = BikeModel.evIB100, ItemsProduced = 1},
        //        new DailyProductionDTO {Date = new DateTime(2020, 2, 28), Model = BikeModel.evIB100, ItemsProduced = 2},
        //        new DailyProductionDTO {Date = new DateTime(2020, 3, 31), Model = BikeModel.evIB100, ItemsProduced = 3},
        //        new DailyProductionDTO {Date = new DateTime(2020, 4, 30), Model = BikeModel.evIB100, ItemsProduced = 4},
        //        new DailyProductionDTO {Date = new DateTime(2020, 5, 31), Model = BikeModel.evIB100, ItemsProduced = 4},
        //        new DailyProductionDTO {Date = new DateTime(2020, 6, 30), Model = BikeModel.evIB100, ItemsProduced = 6},
        //        new DailyProductionDTO {Date = new DateTime(2020, 7, 31), Model = BikeModel.evIB100, ItemsProduced = 10},
        //        new DailyProductionDTO {Date = new DateTime(2020, 8, 31), Model = BikeModel.evIB100, ItemsProduced = 21},
        //        new DailyProductionDTO {Date = new DateTime(2020, 9, 30), Model = BikeModel.evIB100, ItemsProduced = 17},
        //        new DailyProductionDTO {Date = new DateTime(2020, 10, 31), Model = BikeModel.evIB100, ItemsProduced = 15},
        //        new DailyProductionDTO {Date = new DateTime(2020, 11, 30), Model = BikeModel.evIB100, ItemsProduced = 25},
        //        new DailyProductionDTO {Date = new DateTime(2020, 12, 31), Model = BikeModel.evIB100, ItemsProduced = 30},

        //        new DailyProductionDTO {Date = new DateTime(2020, 1, 31), Model = BikeModel.evIB200, ItemsProduced = 10},
        //        new DailyProductionDTO {Date = new DateTime(2020, 2, 28), Model = BikeModel.evIB200, ItemsProduced = 2},
        //        new DailyProductionDTO {Date = new DateTime(2020, 3, 31), Model = BikeModel.evIB200, ItemsProduced = 32},
        //        new DailyProductionDTO {Date = new DateTime(2020, 4, 30), Model = BikeModel.evIB200, ItemsProduced = 41},
        //        new DailyProductionDTO {Date = new DateTime(2020, 5, 31), Model = BikeModel.evIB200, ItemsProduced = 46},
        //        new DailyProductionDTO {Date = new DateTime(2020, 6, 30), Model = BikeModel.evIB200, ItemsProduced = 62},
        //        new DailyProductionDTO {Date = new DateTime(2020, 7, 31), Model = BikeModel.evIB200, ItemsProduced = 102},
        //        new DailyProductionDTO {Date = new DateTime(2020, 8, 31), Model = BikeModel.evIB200, ItemsProduced = 21},
        //        new DailyProductionDTO {Date = new DateTime(2020, 9, 30), Model = BikeModel.evIB200, ItemsProduced = 44},
        //        new DailyProductionDTO {Date = new DateTime(2020, 10, 31), Model = BikeModel.evIB200, ItemsProduced = 51},
        //        new DailyProductionDTO {Date = new DateTime(2020, 11, 30), Model = BikeModel.evIB200, ItemsProduced = 61},
        //        new DailyProductionDTO {Date = new DateTime(2020, 12, 31), Model = BikeModel.evIB200, ItemsProduced = 88}
        //    };
        }

        [HttpGet]
        public IEnumerable<DailyProductionDTO> Get()
        {
            List<DailyProductionDTO> nyListe = new List<DailyProductionDTO>();

            var entities = client.Query<TableEntity>();

            foreach (var item in entities)
            {
                DailyProductionDTO cykel = new DailyProductionDTO();

                cykel.Date = DateTime.Parse(item.RowKey);


                if (int.Parse(item.PartitionKey) == 1)
                {
                   cykel.Model = BikeModel.IBv1;
                }
                else if (int.Parse(item.PartitionKey) == 2) {
                    cykel.Model = BikeModel.evIB100;
                }
                 else if (int.Parse(item.PartitionKey) == 3) {
                    cykel.Model = BikeModel.evIB200;
                } else
               {
                    cykel.Model = BikeModel.undefined;
                }

                cykel.ItemsProduced = item.GetInt32("itemsProduced");

                nyListe.Add(cykel);
            }


            return nyListe;
        }
    }
}
