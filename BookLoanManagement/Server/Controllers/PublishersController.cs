﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookLoanManagement.Server.Data;
using BookLoanManagement.Shared.Domain;
using BookLoanManagement.Server.IRepository;

namespace BookLoanManagement.Server.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public PublishersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET:/Publishers
        [HttpGet]
        public async Task<IActionResult> GetPublishers()
        {
            var publishers = await _unitOfWork.Publishers.GetAll();
            return Ok(publishers);
        }

        // GET:/Publishers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPublisher(int id)
        {
            var publisher = await _unitOfWork.Publishers.Get(q => q.Id == id);

            if (publisher == null)
            {
                return NotFound();
            }

            return Ok(publisher);
        }

        // PUT: /Publishers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPublisher(int id, Publisher publisher)
        {
            if (id != publisher.Id)
            {
                return BadRequest();
            }

            _unitOfWork.Publishers.Update(publisher);

            try
            {
                await _unitOfWork.Save(HttpContext);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await PublisherExists(id))
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

        // POST: api/Publishers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Publisher>> PostPublisher(Publisher publisher)
        {
            await _unitOfWork.Publishers.Insert(publisher);
            await _unitOfWork.Save(HttpContext);

            return CreatedAtAction("GetPublisher", new { id = publisher.Id }, publisher);
        }

        // DELETE: api/Publishers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePublisher(int id)
        {
            var publisher = await _unitOfWork.Publishers.Get(q => q.Id == id);
            if (publisher == null)
            {
                return NotFound();
            }
            await _unitOfWork.Publishers.Delete(id);
            await _unitOfWork.Save(HttpContext);
            return NoContent();
        }

        private async Task<bool> PublisherExists(int id)
        {
            var publisher = await _unitOfWork.Publishers.Get(q => q.Id == id);
            return publisher == null;
        }
    }
}
