using ContactForm.Models;
using ContactForm.Sample.Models;
using ContactForm.Sample.Services;
using ContactForm.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace ContactForm.Sample.Pages
{
    public class ContactJSModel: PageModel
    {
        private ILogger<ContactJSModel> logger;
        private readonly IPostService post;

        public ContactFormSampleSettings settings { get; private set; }

        public ContactJSModel(ILogger<ContactJSModel> logger, IOptions<ContactFormSampleSettings> settings, IPostService post)
        {
            this.logger = logger;
            this.post = post;
            this.settings = settings.Value;
        }

        public void OnGet()
        {

        }

        public IActionResult OnPost([FromBody]ContactModel ContactModel)
        {
            // Call custom validator
            var validator = new ContactModelValidator();
            var results = validator.Validate(ContactModel);
            results.AddToModelState(ModelState, null);

            // Check Recaptcha
            if (settings.RecaptchaEnabled)
            {
                var recaptchaService = new RecaptchaService();
                var recaptchaResponse = recaptchaService.Validate(
                    new RecaptchaSettings
                    {
                        Enabled = true,
                        RecaptchaKey = settings.RecaptchaSecretKey,
                        RecaptchaResponse = ContactModel.RecaptchaResponse
                    }, logger);

                if (recaptchaResponse.ServiceResultType != ServiceResultType.Success)
                    ModelState.AddModelError("Recaptcha", recaptchaResponse.Message);
            }

            if (!ModelState.IsValid)
            {
                return new JsonResult(ModelState.Values);
            }

            HttpContent content = new StringContent(JsonConvert.SerializeObject(ContactModel), Encoding.UTF8, "application/json");
            var resultOk = post.Post(settings.APIEndpoint, content) && post.Post(settings.ContactFormWebEndpoint, content);

            return new JsonResult(new { Status = resultOk ? "OK" : "Error calling API." });
        }
    }
}