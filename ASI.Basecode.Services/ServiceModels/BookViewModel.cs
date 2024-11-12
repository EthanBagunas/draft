using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.ServiceModels
{
    public class BookViewModel
    {
        public int Id { get; set; }
        public int? CustomerIdFk { get; set; }
        public int? RoomId { get; set; }
        public DateTime? DateIn { get; set; }
        public DateTime? DateOut { get; set; }
        public DateTime? DateRange { get; set; }

    }
}
