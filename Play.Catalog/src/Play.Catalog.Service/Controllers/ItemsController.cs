﻿using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Play.Catalog.Service.Controllers
{
    // https://localhost:5001/items
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private static readonly List<ItemDto> items = new()
        {
            new ItemDto(Guid.NewGuid(), "Potion", "Restores a small amount of HP", 5, DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(), "Antidote", "Cures poison", 7, DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(), "Bronze sword", "Deals a small amount of damage", 20, DateTimeOffset.UtcNow)
        };

        // GET /items/
        [HttpGet]
        public IEnumerable<ItemDto> Get()
        {
            return items;
        }

        // GET /items/{id}
        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetById(Guid id)
        {
            var item = items.Where(item => item.id == id).SingleOrDefault();

            if (item == null) return NotFound();

            return item;
        }

        // POST /items
        [HttpPost]
        public ActionResult<ItemDto> Post([FromBody] CreateItemDto createItemDto)
        {
            var item = new ItemDto(Guid.NewGuid(), createItemDto.Name, createItemDto.Description,
                createItemDto.Price, DateTimeOffset.UtcNow);

            items.Add(item);

            return CreatedAtAction(nameof(GetById), new {id = item.id}, item);
        }

        // PUT /items/{id}
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] UpdateItemDto updateItemDto)
        {
            var existingItem = items.Where(item => item.id == id).SingleOrDefault();

            if (existingItem == null) return NotFound();

            var updatedItem = existingItem with
            {
                Name = updateItemDto.Name,
                Description = updateItemDto.Description,
                Price = updateItemDto.Price
            };

            var index = items.FindIndex(existingItem => existingItem.id == id);
            items[index] = updatedItem;

            return NoContent();
        }

        // DELETE /items/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var existingItem = items.Where(items=> items.id == id).SingleOrDefault();

            if (existingItem == null) return NotFound();

            items.Remove(existingItem);

            return NoContent();
        }
    }
}
