﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace SimpleAdsSessionHmwk.Data
{
    public class SimpleAd
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
    }
}
