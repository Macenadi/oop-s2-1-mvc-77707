namespace Library.mvc.Models
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.ComponentModel.DataAnnotations;

    public class CreateInvoiceViewModel
    {
        [Required]
        [DataType(DataType.Date)]
        public DateTime InvoiceDate { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        public required IEnumerable<SelectListItem> Customers { get; set; }
        public required IEnumerable<SelectListItem> Products { get; set; }
    }
}