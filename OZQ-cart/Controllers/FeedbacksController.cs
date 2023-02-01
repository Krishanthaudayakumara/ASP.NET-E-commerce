using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
    public class FeedbacksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public FeedbacksController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Feedbacks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FeedbackToReturnDto>>> GetFeedbacks()
        {
            var feedbacks = await _context.Feedbacks.ToListAsync();

            return feedbacks.Select(f => new FeedbackToReturnDto
            {
                Id = f.Id,
                CustomerId = f.CustomerId,
                Comment = f.Comment
            }).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FeedbackToReturnDto>> GetFeedback(int id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);

            if (feedback == null)
            {
                return NotFound();
            }

            return new FeedbackToReturnDto
            {
                Id = feedback.Id,
                CustomerId = feedback.CustomerId,
                Comment = feedback.Comment
            };
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutFeedback(int id, FeedbackToInsertDto feedbackDto)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null)
            {
                return NotFound();
            }

            feedback.Comment = feedbackDto.Comment;
            feedback.CustomerId = feedbackDto.CustomerId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeedbackExists(id))
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

        // POST: api/Feedbacks
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<FeedbackToReturnDto>> PostFeedback(FeedbackToInsertDto feedbackToInsertDto)
        {
            var feedback = new Feedback
            {
                CustomerId = feedbackToInsertDto.CustomerId,
                Comment = feedbackToInsertDto.Comment
            };

            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();

            var feedbackToReturn = new FeedbackToReturnDto
            {
                Id = feedback.Id,
                CustomerId = feedback.CustomerId,
                Comment = feedback.Comment
            };

            return CreatedAtAction("GetFeedback", new { id = feedback.Id }, feedbackToReturn);
        }

        // DELETE: api/Feedbacks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null)
            {
                return NotFound();
            }
            _context.Feedbacks.Remove(feedback);
            await _context.SaveChangesAsync();

            return NoContent();

        }

        private bool FeedbackExists(int id)
        {
            return _context.Feedbacks.Any(e => e.Id == id);
        }
    }
}






