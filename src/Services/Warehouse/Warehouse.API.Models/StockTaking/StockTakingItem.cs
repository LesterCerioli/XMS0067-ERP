using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Warehouse.API.Models.StockTaking
{
    public class StockTakingItem
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public int StockTakingId { get; set; }
        public virtual StockTaking StockTaking { get; set; }

        [Required]
        public int StoredItemId { get; set; }
        public virtual StoredItem StoredItem { get; set; }

        [Required]
        public long PositionId { get; set; }
        public virtual Position Position { get; set; }

        [Required]
        public int CurrentStock { get; set; }

        [Required]
        public int CountedStock { get; set; }

        [NotMapped]
        public int Variance
        {
            get => this.CountedStock - this.CurrentStock;
        }
    }
}
