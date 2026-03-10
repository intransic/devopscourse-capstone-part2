using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LogiTrack.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly LogiTrackContext _context;

        public InventoryController(LogiTrackContext context)
        {
            _context = context;
        }

        // helper queries
        private Task<InventoryItem?> GetItemAsync(int id)
            => _context.InventoryItems.FindAsync(id).AsTask();

        private Task<bool> ItemExistsAsync(int id)
            => _context.InventoryItems.AnyAsync(e => e.ItemId == id);

        // GET: api/Inventory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryItem>>> GetInventory()
        {
            var list = await _context.InventoryItems.ToListAsync();
            return list;
        }

        // GET: api/Inventory/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<InventoryItem>> GetInventoryItem(int id)
        {
            var item = await GetItemAsync(id);
            return item is null ? NotFound() : item;
        }

        // POST: api/Inventory
        [HttpPost]
        public async Task<ActionResult<InventoryItem>> CreateInventoryItem(InventoryItem item)
        {
            _context.InventoryItems.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetInventoryItem), new { id = item.ItemId }, item);
        }

        // PUT: api/Inventory/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInventoryItem(int id, InventoryItem item)
        {
            if (id != item.ItemId)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ItemExistsAsync(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/Inventory/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteInventoryItem(int id)
        {
            var item = await GetItemAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.InventoryItems.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
