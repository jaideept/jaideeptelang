using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;

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
        /// Add a new contact
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create(Contact contact)
        {
            _contactRepo.InsertAsync(contact);
           
            //return StatusCode(201, new { contact });
            //return 201 Created HTTP status code. The response includes the newly created item in the body 
            //and its URL in the Location HTTP header
            var resourceUrl = Path.Combine(Request.Path.ToString(), contact.Id.ToString());
            return Created(resourceUrl, contact);
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != contact.Id)
            {
                return BadRequest();
            }

            var existingContact = await _contactRepo.FindAsync(id);

            if (existingContact == null)
            {
                return BadRequest("Cannot update a non existing contact.");
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
                return NotFound();
            }
            else
            {
                _contactRepo.DeleteAsync(id);
                return NoContent();
            }
        }
    }
}