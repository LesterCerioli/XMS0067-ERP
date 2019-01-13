﻿using Restmium.ERP.Services.Warehouse.Domain.Entities.Abstract;
using System.ComponentModel.DataAnnotations;

namespace Restmium.ERP.Services.Warehouse.Domain.Entities
{
    public partial class IssueSlip
    {
        public class Item : WarePosition
        {
            public Item()
            {

            }
            private Item(int requestedUnits, int issuedUnits) : this()
            {
                this.RequestedUnits = requestedUnits;
                this.IssuedUnits = issuedUnits;
            }
            public Item(long issueSlipId, int wareId, long positionId, int requestedUnits, int issuedUnits) : this(requestedUnits, issuedUnits)
            {
                this.IssueSlipId = issueSlipId;
                this.WareId = wareId;
                this.PositionId = positionId;
            }
            public Item(IssueSlip issueSlip, Ware ware, Position position, int requestedUnits, int issuedUnits) : this(requestedUnits, issuedUnits)
            {
                this.IssueSlip = issueSlip;
                this.Ware = ware;
                this.Position = position;
            }

            [Required]
            public long IssueSlipId { get; set; }
            public virtual IssueSlip IssueSlip { get; set; }

            [Required]
            public int RequestedUnits { get; set; } //Počet vyžádaných jednotek
            [Required]
            public int IssuedUnits { get; set; } //Počet vydaných jednotek
        }
    }    
}
