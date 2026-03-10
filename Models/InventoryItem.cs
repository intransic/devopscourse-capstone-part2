using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

public class InventoryItem
{
    [Key]
    public int ItemId { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public int Quantity { get; set; }
    
    [Required]
    public string Location { get; set; }

    // Many-to-many: an inventory item can be in many orders
    public List<Order> Orders { get; set; } = new();

    public void DisplayInfo()
    {
        Console.WriteLine($"ID: {ItemId}, Name: {Name}, Quantity: {Quantity}, Location: {Location}");
    }
}