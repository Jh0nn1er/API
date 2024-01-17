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
        public ActionResult<IEnumerable<Form>> GetContactForms()
        {
            try
            {
                var forms = _dbContext.Forms.ToList();

                if (!forms.Any())
                {
                    return NotFound(new { msg = "No forms were found in the database."});
                }
                return Ok(_dbContext.Forms.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(new { msg = $"Error trying to get forms: {ex.Message}"});
            }
        }
        [HttpGet("{id}")]
        public IActionResult GetForm(int id)
        {
            if (id == 0)
            {
                return BadRequest(new { msg = "enter a data"});
            }

            var form = _dbContext.Forms.FirstOrDefault(v => v.Id == id);

            if (form == null)
            {
                return NotFound(new { msg = "form is null"});
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
                var errorResponse = new
                {
                    errors = new
                    {
                        mailExists = new[] { "A form with that email already exists" }
                    }
                };

                return BadRequest(errorResponse);
            }
            
            if (!ModelState.IsValid)
            {
                var errorResponse = new
                {
                    message = "The form is not valid and does not meet the conditions",
                    //errors = ModelState
                      //  .Where(e => e.Value.Errors.Count > 0)
                        //.ToDictionary(
                          //  kvp => kvp.Key,
                           // kvp => kvp.Value.Errors.Select(error => error.ErrorMessage).ToArray()
                        //)
                };

                return BadRequest(errorResponse);
            }
            Form newForm = new()
            {
                FullName = formData.FullName,
                Email = formData.Email,
                DocumentType = formData.DocumentType,
                Identifier = formData.Identifier,
                Comment = formData.Comment,
                PaymentDate = DateTime.Now,
            };
            _dbContext.Add(newForm);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(CreateForms), new { id = newForm.Id });
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
            var Form = _dbContext.Forms.FirstOrDefault(v => v.Id == id);
            if (Form == null)
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
        public IActionResult UpdateForms(int id, [FromBody] Form formData)
        {
            if (formData == null)
            {
                return BadRequest(new { msg = "Invalid request data" });
            }

            var existingForm = _dbContext.Forms.FirstOrDefault(f => f.Id == id);
            if (existingForm == null)
            {
                return NotFound(new { msg = "Form not found" });
            }
            try
            {
                Form newForm = new()
                {
                    FullName = formData.FullName,
                    Email = formData.Email,
                    DocumentType = formData.DocumentType,
                    Identifier = formData.Identifier,
                    Comment = formData.Comment,
                    PaymentDate = DateTime.Now,
                };
                _dbContext.Update(newForm);
                return Ok(new { msg = "Form update successfully"});
                return NoContent();
            }
            catch(Exception ex){
                return BadRequest(new { msg = "Error updating form"});
            }
            

        }
    }
}