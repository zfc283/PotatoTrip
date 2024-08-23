using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using WebApplication1.Models;

namespace WebApplication1.DTOs
{
    public class OrderDTO
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public ICollection<LineItemDTO> OrderItems { get; set; }
        public string State { get; set; }
        public DateTime CreateTime { get; set; }
        public string TransactionMetadata { get; set; }
    }
}
