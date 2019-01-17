﻿using Restmium.ERP.BuildingBlocks.Common.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Restmium.ERP.Services.Warehouse.Domain.Entities
{
    public partial class IssueSlip : DatabaseEntity
    {
        public IssueSlip()
        {
            this.Items = new HashSet<Item>();
        }
        public IssueSlip(string name, DateTime utcDispatchDate, DateTime utcDeliveryDate) : this()
        {
            this.Name = name;
            this.UtcDispatchDate = utcDispatchDate;
            this.UtcDeliveryDate = utcDeliveryDate;
        }
        public IssueSlip(string name, DateTime utcDispatchDate, DateTime utcDeliveryDate, ICollection<Item> items) : this(name, utcDispatchDate, utcDeliveryDate)
        {
            this.Items = items;
        }

        [Required]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }
        
        public DateTime UtcDispatchDate { get; set; }

        [Required]
        public DateTime UtcDeliveryDate { get;set; }

        public virtual ICollection<Item> Items { get; protected set; }

        #region Methods
        /// <summary>
        /// Returns the unissued items within the IssueSlip instance
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Item> GetUnissuedItems()
        {
            return this.Items.Where(x => x.IssuedUnits < x.RequestedUnits);
        }
        #endregion
    }
}
