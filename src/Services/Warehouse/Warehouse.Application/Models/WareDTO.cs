﻿using System.ComponentModel.DataAnnotations;

namespace Restmium.ERP.Services.Warehouse.Application.Models
{
    /// <summary>
    /// DTO for <see cref="Domain.Entities.Ware"/>
    /// </summary>
    public class WareDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public double Width { get; set; }
        [Required]
        public double Height { get; set; }
        [Required]
        public double Depth { get; set; }
        [Required]
        public double Weight { get; set; }
    }
}