﻿using System;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Models
{
    public partial class RoomInformation
    {
        public int Id { get; set; }
        public string Roomname { get; set; }
        public string CurrOccupant { get; set; }
        public string CurrStatus { get; set; }
    }
}
