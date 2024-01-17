using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAPIProduco.Model;
using WebAPIProduco.Data; 


namespace WebAPIProduco.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormsController : ControllerBase
    {
        private readonly DataContext _dbContext;
        public FormsController(DataContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<Form>> GetContactForms()
        {
            return Ok(_dbContext.Forms.ToList());
        }
        [HttpGet("{id}")]
        public IActionResult GetForm(int id)
        {
            if(id==0) 
            {
                return BadRequest();
            }

            var form = _dbContext.Forms.FirstOrDefault(v => v.Id == id);

            if (form == null)
            {
                return NotFound(); 
            }

            return Ok(form);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateForms([FromBody] Form formData)
        {
            if (_dbContext.Forms.FirstOrDefault(v => v.Email.ToLower() == formData.Email.ToLower()) != null)
            {
                ModelState.AddModelError("mailExists", "a form with that email already exists");
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

             Form newForm = new()
                {
                    FullName = formData.FullName,
                    Email = formData.Email,
                    DocumentType = formData.DocumentType,
                    Identifier = formData.Identifier,
                    Comment = formData.Comment,
                    PaymentDate=DateTime.Now,

                };
                _dbContext.Add(newForm);
                await _dbContext.SaveChangesAsync();

                return CreatedAtAction(nameof(CreateForms), new { id = newForm.Id }, new { id = newForm.Id });
        }
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteForms(int id) 
        { 
        if (id == 0)
            {
                return BadRequest();
            }
            var Form =_dbContext.Forms.FirstOrDefault(v=> v.Id == id);
            if(Form == null)
            {
                return NotFound(); 
            }
            _dbContext.Forms.Remove(Form);
            _dbContext.SaveChanges();

            return NoContent();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
       public  IActionResult UpdateForms(int id, [FromBody] Form formData) 
       {
           if(formData == null || id!=formData.Id)
           {
               return BadRequest();    
           }
           Form newForm = new()
           {
               Id = formData.Id,
               FullName = formData.FullName,
               Email = formData.Email,
               DocumentType = formData.DocumentType,
               Identifier = formData.Identifier,
               Comment = formData.Comment,
               PaymentDate=DateTime.Now,
           };
           _dbContext.Update(newForm);
           return NoContent();

       }
    }
}