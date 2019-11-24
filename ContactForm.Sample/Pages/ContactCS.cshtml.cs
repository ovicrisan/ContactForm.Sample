using ContactForm.Models;
using ContactForm.Sample.Models;
using ContactForm.Sample.Services;
using ContactForm.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

namespace ContactForm.Sample.Pages
{
    [IgnoreAntiforgeryToken]
    public class ContactCSModel: PageModel
    {
        private ILogger<ContactCSModel> logger;
        private readonly IPostService post;

        public ContactFormSampleSettings settings { get; private set; }
        public string Message { get; set; }

        [BindProperty]
        public ContactModel ContactModel { get; set; }

        public ContactCSModel(ILogger<ContactCSModel> logger, IOptions<ContactFormSampleSettings> settings, IPostService post)
        {
            this.logger = logger;
            this.post = post;
            this.settings = settings.Value;
        }

        public void OnGet(int? s)
        {
            if(s.HasValue && s.Value == 1)
                Message = "Contact form processed successfully. Thank you.";
        }

        public IActionResult OnPost()
        {
            // Call custom validator
            var validator = new ContactModelValidator();
            var results = validator.Validate(ContactModel);
            results.AddToModelState(ModelState, "ContactModel.");

            // Check Recaptcha
            if (settings.RecaptchaEnabled)
            {
                var recaptchaService = new RecaptchaService();
                var recaptchaResponse = recaptchaService.Validate(
                    new RecaptchaSettings
                    {
                        Enabled = true,
                        RecaptchaKey = settings.RecaptchaSecretKey,
                        RecaptchaResponse = Request.Form["g-recaptcha-response"]
                    }, logger);

                if (recaptchaResponse.ServiceResultType != ServiceResultType.Success)
                    ModelState.AddModelError("Recaptcha", recaptchaResponse.Message);
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            HttpContent content = new StringContent(JsonConvert.SerializeObject(ContactModel), Encoding.UTF8, "application/json");
            var resultOk = post.Post(settings.APIEndpoint, content) && post.Post(settings.ContactFormWebEndpoint, content);

            if (resultOk)
                return new RedirectToPageResult("ContactCS", new { s = 1 });
            else
            {
                Message = "Error calling API.";
                return Page();
            }
        }
    }
}