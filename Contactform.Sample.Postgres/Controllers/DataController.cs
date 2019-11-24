using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContactForm;
using ContactForm.Models;
using ContactForm.Sample.Postgres.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Contactform.Sample.Postgres.Controllers
{
    [ApiController]
    [Route("")]
    public class DataController : ControllerBase
    {
        private readonly ILogger<DataController> _logger;
        private readonly AppDbContext context;

        public DataController(ILogger<DataController> logger, AppDbContext context)
        {
            _logger = logger;
            this.context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult<ContactModelData> Save(ContactModel contact)
        {
            _logger.LogInformation("Save new contact form");
            var data = new ContactModelData(contact);
            context.Contacts.Add(data);
            context.SaveChanges();
            return new CreatedResult("", data);
        }
    }
}
