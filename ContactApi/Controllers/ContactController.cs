using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;
using ContactApi.Repository;
using ContactApi.Models;
using System.Collections.Generic;

namespace ContactApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactController : ControllerBase
    {
        private IContactRepository _contactRepo;

        public ContactController(IContactRepository contactRepo)
        {
            _contactRepo = contactRepo;
        }


        /// <summary>
        /// Get all contacts
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> Get()
        {
            var contacts = await _contactRepo.GetAllAsync();
            return Newtonsoft.Json.JsonConvert.SerializeObject(contacts);
        }

        /// <summary>
        /// Get contacts with paging
        /// </summary>
        /// <returns></returns>
        [HttpGet("{page}/{pageSize}")]
        public async Task<string> Get(int page = 1, int pageSize = 25)
        {
            var contacts = await _contactRepo.GetByPageIndex<Contact>(page, pageSize);

            return Newtonsoft.Json.JsonConvert.SerializeObject(contacts);
        }

        /// <summary>
        /// Get Contact By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var contact = await _contactRepo.FindAsync(id);

            if (contact == null)
            {
                return NotFound(new { message = "Contact not found" });
            }

            return Ok(contact);
        }

        /// <summary>
        /// Add a new contact
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(Contact contact)
        {
            var contactInserted = await _contactRepo.InsertAsync(contact);

            //in a RESTful API a POST should return a 201 Created response, 
            //with a Location header pointing to the url for the newly created response
            return CreatedAtAction(nameof(Get), new { id = contactInserted.Id }, contactInserted);
        }

        /// <summary>
        /// Update an existing contact item
        /// </summary>
        /// <param name="id"></param>
        /// <param name="contact"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Contact contact)
        {
            if (id != contact.Id)
            {
                return BadRequest();
            }

            var existingContact = await _contactRepo.FindAsync(id);

            if (existingContact == null)
            {
                return BadRequest(new { message = "Cannot update a non existing contact." });
            }
            else
                _contactRepo.UpdateAsync(contact);

            return NoContent();
        }

        /// <summary>
        /// Delete an contact item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingContact = await _contactRepo.FindAsync(id);

            //If contact doesn't exist, a 404 Not Found HTTP status code is returned
            if (existingContact == null)
            {
                return NotFound(new { message = "Contact not found" });
            }
            else
            {
                _contactRepo.DeleteAsync(id);
                return NoContent();
            }
        }
    }
}
