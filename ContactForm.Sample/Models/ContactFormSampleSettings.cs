namespace ContactForm.Sample.Models
{
    public class ContactFormSampleSettings
    {
        public string ContactFormWebEndpoint { get; set; }
        public string APIEndpoint { get; set; }
        public bool RecaptchaEnabled { get; set; }
        public string RecaptchaPublicKey { get; set; }
        public string RecaptchaSecretKey { get; set; }
    }
}
