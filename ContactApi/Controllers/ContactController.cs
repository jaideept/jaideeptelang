using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;
using ContactApi.Repository;
using ContactApi.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.AspNetCore.JsonPatch;
using StackExchange.Profiling;

namespace ContactApi.Controllers
{
    [ApiController]
    //[Route("[controller]")]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private IContactRepository _contactRepo;
        readonly ILogger<ContactController> _logger;

        public ContactController(IContactRepository contactRepo, ILogger<ContactController> logger)
        {
            _contactRepo = contactRepo;
            _logger = logger;
        }


        /// <summary>
        /// Get all contacts
        /// </summary>
        /// <returns></returns>
        //[HttpGet]
        //public async Task<string> Get()
        //{
        //    var contacts = await _contactRepo.GetAllAsync();
        //    return Newtonsoft.Json.JsonConvert.SerializeObject(contacts);
        //}

        /// <summary>
        /// Get all contacts in paged result
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<PagedCollectionResponse<Contact>>> Get([FromQuery] FilterModel filter)
        {
            var contacts = await _contactRepo.GetByPageIndex<Contact>(filter.Page, filter.Limit);
            //Filtering logic  
            IEnumerable<Contact> filterData(FilterModel filterModel) => contacts;

            //Get the data for the current page  
            var result = new PagedCollectionResponse<Contact>();
            result.Items = filterData(filter);

            //Get next page URL string  
           FilterModel nextFilter = filter.Clone() as FilterModel;
            nextFilter.Page += 1;
            String nextUrl = filterData(nextFilter).Count() <= 0 ? null : this.Url.Action("Get", null, nextFilter, Request.Scheme);

            //Get previous page URL string  
            FilterModel previousFilter = filter.Clone() as FilterModel;
            previousFilter.Page -= 1;
            String previousUrl = previousFilter.Page <= 0 ? null : this.Url.Action("Get", null, previousFilter, Request.Scheme);

            result.NextPage = !String.IsNullOrWhiteSpace(nextUrl) ? new Uri(nextUrl) : null;
            result.PreviousPage = !String.IsNullOrWhiteSpace(previousUrl) ? new Uri(previousUrl) : null;

            return result;

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
        /// with return type as ActionResult<T>,
        /// if all goes well you will be getting a Contact object in the end without the need to wrap the result in an OK.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[ResponseCache(Duration = 100)]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Contact>> Get([FromRoute] int id)
        {
            //profiling using MiniProfiler
            using (MiniProfiler.Current.Step("GetById method"))
            {
                using (MiniProfiler.Current.CustomTiming("HTTP", "GET "))
                {
                    var contact = await _contactRepo.FindAsync(id);

                    if (contact == null)
                    {
                        return NotFound(new { message = "Contact not found" });
                    }

                    return contact;
                }
            }
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
        [HttpPut("{id:int}")]
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

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<Contact> patchedEntity)
        {
            if (patchedEntity == null)
            {
                return BadRequest();
            }

            // Get our original Contact object from the database
            var existingContactFromDB = await _contactRepo.FindAsync(id);

            if (existingContactFromDB == null)
            {
                return NotFound();
            }

            //Apply the patch
            patchedEntity.ApplyTo(existingContactFromDB, ModelState);

            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }

            //var isValid = TryValidateModel(existingContactFromDB);

            //if (!isValid)
            //{
            //    return BadRequest(ModelState);
            //}

            _contactRepo.UpdateAsync(existingContactFromDB);

            return NoContent();
        }

        /// <summary>
        /// Delete an contact item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
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