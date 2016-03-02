﻿using Newtonsoft.Json;
using SPP.Common.Helpers;
using System;

namespace SPP.Model
{
    public class EntityDTOBase: BaseModel
    {
        [JsonConverter(typeof(SPPDateTimeConverter))]
        public DateTime Modified_Date { get; set; }

        public int Modified_UID { get; set; }

        public string Modified_UserName { get; set; }

        public string Modified_UserNTID { get; set; }

        public SystemUserDTO ModifiedUser { get; set; }
    }


   
}