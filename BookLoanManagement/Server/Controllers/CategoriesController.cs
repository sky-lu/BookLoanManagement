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
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET:/Categories
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _unitOfWork.Categories.GetAll();
            return Ok(categories);
        }

        // GET:/Categories/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _unitOfWork.Categories.Get(q => q.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        // PUT: /Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }

            _unitOfWork.Categories.Update(category);

            try
            {
                await _unitOfWork.Save(HttpContext);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CategoryExists(id))
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

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            await _unitOfWork.Categories.Insert(category);
            await _unitOfWork.Save(HttpContext);

            return CreatedAtAction("GetCategory", new { id = category.Id }, category);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _unitOfWork.Categories.Get(q => q.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            await _unitOfWork.Categories.Delete(id);
            await _unitOfWork.Save(HttpContext);
            return NoContent();
        }

        private async Task<bool> CategoryExists(int id)
        {
            var category = await _unitOfWork.Categories.Get(q => q.Id == id);
            return category == null;
        }
    }
}
