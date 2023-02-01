using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OZQ_cart.Data;
using OZQ_cart.DTOs;
using OZQ_cart.Models;

namespace OZQ_cart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderToReturnDto>>> GetOrders()
        {
            var orders = await _context.Orders
                                .Include(o => o.Customer)
                                .Include(o => o.OrderItems)
                                    .ThenInclude(oi => oi.Item)
                                .ToListAsync();

            return orders.Select(o => new OrderToReturnDto
            {
                Id = o.Id,
                CustomerId = o.CustomerId,
                OrderItems = o.OrderItems.Select(oi => new OrderItemToReturnDto
                {
                    Id = oi.Id,
                    ItemId = oi.ItemId,
                    Quantity = oi.Quantity
                }).ToList(),
                TotalPrice = o.TotalPrice
            }).ToList();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrder(int id)
        {
            var order = await _context.Orders
                                .Include(o => o.Customer)
                                .Include(o => o.OrderItems)
                                    .ThenInclude(oi => oi.Item)
                                .SingleOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return new OrderToReturnDto
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                OrderItems = order.OrderItems.Select(oi => new OrderItemToReturnDto
                {
                    Id = oi.Id,
                    ItemId = oi.ItemId,
                    Quantity = oi.Quantity
                }).ToList(),
                TotalPrice = order.TotalPrice
            };
        }

        [HttpGet("bycustomer/{customerId}")]
        public async Task<IActionResult> GetOrdersByCustomer(int customerId)
        {
            var orders = await _context.Orders
                .Where(o => o.CustomerId == customerId)
                .Include(o => o.OrderItems)
                .Select(o => new OrderToReturnDto
                {
                    Id = o.Id,
                    CustomerId = o.CustomerId,
                    OrderItems = o.OrderItems.Select(oi => new OrderItemToReturnDto
                    {
                        ItemId = oi.ItemId,
                        Quantity = oi.Quantity,
                    }).ToList(),
                    TotalPrice = o.TotalPrice
                })
                .ToListAsync();

            return Ok(orders);
        }


        // PUT: api/Orders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, OrderToInsertDto orderDto)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            if (orderDto.CustomerId != order.CustomerId)
            {
                return BadRequest();
            }

            _context.Entry(order).CurrentValues.SetValues(orderDto);

            var orderItemsToRemove = order.OrderItems.Where(o => !orderDto.OrderItems.Any(oi => oi.ItemId == o.ItemId)).ToList();

            foreach (var orderItem in orderItemsToRemove)
            {
                _context.OrderItems.Remove(orderItem);
            }

            var orderItemsToAdd = orderDto.OrderItems.Where(oi => !order.OrderItems.Any(o => o.ItemId == oi.ItemId)).ToList();

            foreach (var orderItemDto in orderItemsToAdd)
            {
                var newOrderItem = new OrderItem
                {
                    ItemId = orderItemDto.ItemId,
                    Quantity = orderItemDto.Quantity
                };
                order.OrderItems.Add(newOrderItem);
            }

            var orderItemsToUpdate = order.OrderItems.Where(o => orderDto.OrderItems.Any(oi => oi.ItemId == o.ItemId)).ToList();

            foreach (var orderItem in orderItemsToUpdate)
            {
                var orderItemDto = orderDto.OrderItems.First(oi => oi.ItemId == orderItem.ItemId);
                _context.Entry(orderItem).CurrentValues.SetValues(orderItemDto);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(OrderToInsertDto orderToInsert)
        {
            var order = new Order
            {
                CustomerId = orderToInsert.CustomerId,
                TotalPrice = 0
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var orderItems = new List<OrderItem>();
            foreach (var item in orderToInsert.OrderItems)
            {
                var orderItem = new OrderItem
                {
                    ItemId = item.ItemId,
                    Quantity = item.Quantity,
                    OrderId = order.Id
                };
                var dbItem = _context.Items.Find(item.ItemId);
                orderItem.Item = dbItem;
                orderItems.Add(orderItem);
                order.TotalPrice += (double)dbItem.Price * item.Quantity;
            }

            _context.OrderItems.AddRange(orderItems);
            await _context.SaveChangesAsync();

            order.OrderItems = orderItems;
            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            var orderItems = _context.OrderItems.Where(x => x.OrderId == order.Id);
            _context.OrderItems.RemoveRange(orderItems);
            await _context.SaveChangesAsync();

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}

