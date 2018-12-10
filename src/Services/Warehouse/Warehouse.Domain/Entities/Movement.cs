﻿using Restmium.ERP.BuildingBlocks.Common.Entities;
using System.ComponentModel.DataAnnotations;

namespace Restmium.ERP.Services.Warehouse.Domain.Entities
{
    public partial class Movement : DatabaseEntity
    {
        public Movement()
        {

        }
        private Movement(Direction direction, EntryContent content, int countChange, int countTotal) : this()
        {
            this.MovementDirection = direction;
            this.Content = content;
            this.CountChange = countChange;
            this.CountTotal = countTotal;
        }
        public Movement(int wareId, int positionId, Direction direction, EntryContent content, int countChange, int countTotal) : this(direction, content, countChange, countTotal)
        {
            this.WareId = wareId;
            this.PositionId = positionId;
        }
        public Movement(Ware ware, Position position, Direction direction, EntryContent content, int countChange, int countTotal) : this(direction, content, countChange, countTotal)
        {
            this.Ware = ware;
            this.Position = position;
        }

        [Required]
        public long Id { get; set; }

        [Required]
        public int WareId { get; set; }
        public virtual Ware Ware { get; set; }

        [Required]
        public long PositionId { get; set; }
        public virtual Position Position { get; set; }

        [Required]
        public Direction MovementDirection { get; set; }

        [Required]
        public EntryContent Content { get; set; }

        [Required]
        public int CountChange { get; set; }

        [Required, Range(0, int.MaxValue)]
        public int CountTotal { get; set; }
    }
}
