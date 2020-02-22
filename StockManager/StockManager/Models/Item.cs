using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using Xamarin.Forms;

namespace StockManager.Models
{
    public class Item
    {
        [PrimaryKey, AutoIncrement]
        public int ItemID { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public string StockQty { get; set; }
        public string ImagePath { get; set; }

    }
}
