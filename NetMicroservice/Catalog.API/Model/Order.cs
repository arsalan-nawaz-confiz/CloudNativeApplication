using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Model
{
    public class Order
    {
        public DateTime OrderDate { get; set; }

        public long OrderNumber { get; set; }

        //[JsonConstructor]
        //public Order(DateTime orderDate, long orderNumber)
        //{
        //    OrderDate = orderDate;
        //    OrderNumber = orderNumber;
        //}
    }
}
