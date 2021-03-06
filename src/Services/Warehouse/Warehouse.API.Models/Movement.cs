using System;
using System.ComponentModel.DataAnnotations;

namespace Warehouse.API.Models
{
    public class Movement
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public int StoredItemId { get; set; }
        public virtual StoredItem StoredItem { get; set; }

        [Required]
        public long PositionId { get; set; }
        public virtual Position Position { get; set; }

        [Required]
        public Direction Direction { get; set; }

        [Required]
        public EntryContent EntryContent { get; set; }

        [Required]
        public int CountChange { get; set; }

        [Required, Range(0, int.MaxValue)]
        public int CountTotal { get; set; }

        public DateTime MovementDate { get; set; }
    }
}
