using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using WebApplication1.Models;

namespace WebApplication1.DTOs
{
    public class ShoppingCartDTO
    {
        public Guid Id { get; set; }   
        public string UserId { get; set; }
        public ICollection<LineItemDTO> ShoppingCartItems { get; set; }
    }
}
