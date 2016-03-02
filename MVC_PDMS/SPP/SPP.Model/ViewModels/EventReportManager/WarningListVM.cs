﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPP.Model
{
    public class WarningListVM
    {
        public int Warning_UID { get; set; }
        public string Warning_Types { get; set; }
        public string Customer { get; set; }
        public string Project { get; set; }
        public string Part_Types { get; set; }
        public string Product_Phase { get; set; }
        public string Product_Date { get; set; }
        public string Time_Interval { get; set; }
        public string FncPlant_Effect { get; set; }
    }
}