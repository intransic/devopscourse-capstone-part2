using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

public class Order
{
    [Key]
    public int OrderId { get; set; }
    [Required]
    public string CustomerName { get; set; }
    [Required]
    public DateTime DatePlaced { get; set; }

    // Many-to-many: an order can contain many inventory items
    public List<InventoryItem> Items { get; set; } = new();
}