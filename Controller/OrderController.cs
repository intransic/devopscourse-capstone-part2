using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LogiTrack.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly LogiTrackContext _context;

        public OrderController(LogiTrackContext context)
        {
            _context = context;
        }

        // helpers to centralize common async queries
        private Task<Order?> GetOrderWithItemsAsync(int id)
            => _context.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.OrderId == id);

        private Task<bool> OrderExistsAsync(int id)
            => _context.Orders.AnyAsync(o => o.OrderId == id);


        // GET: api/Order
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var list = await _context.Orders.Include(o => o.Items).ToListAsync();
            return list;
        }

        // GET: api/Order/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await GetOrderWithItemsAsync(id);
            return order is null ? NotFound() : order;
        }

        // POST: api/Order
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetOrder), new { id = order.OrderId }, order);
        }

        // PUT: api/Order/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, Order order)
        {
            if (id != order.OrderId)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await OrderExistsAsync(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/Order/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ----------------------------------------------------------------
        // Nested item management (many-to-many)
        // ----------------------------------------------------------------

        // GET: api/Order/5/items
        [HttpGet("{orderId}/items")]
        public async Task<ActionResult<IEnumerable<InventoryItem>>> GetItemsForOrder(int orderId)
        {
            var order = await _context.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.OrderId == orderId);
            if (order == null)
            {
                return NotFound();
            }
            return order.Items;
        }

        // POST: api/Order/5/items/{itemId}
        [HttpPost("{orderId}/items/{itemId}")]
        public async Task<IActionResult> AddItemToOrder(int orderId, int itemId)
        {
            var order = await _context.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.OrderId == orderId);
            var item = await _context.InventoryItems.FindAsync(itemId);
            if (order == null || item == null)
            {
                return NotFound();
            }
            if (!order.Items.Contains(item))
            {
                order.Items.Add(item);
                await _context.SaveChangesAsync();
            }
            return NoContent();
        }

        // DELETE: api/Order/5/items/{itemId}
        [HttpDelete("{orderId}/items/{itemId}")]
        public async Task<IActionResult> RemoveItemFromOrder(int orderId, int itemId)
        {
            var order = await _context.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.OrderId == orderId);
            var item = await _context.InventoryItems.FindAsync(itemId);
            if (order == null || item == null)
            {
                return NotFound();
            }
            if (order.Items.Contains(item))
            {
                order.Items.Remove(item);
                await _context.SaveChangesAsync();
            }
            return NoContent();
        }
    }
}
