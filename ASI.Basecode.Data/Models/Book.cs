﻿using System;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Models
{
    public partial class Book
    {
        public int Id { get; set; }
        public string Tile { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedTim { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}
