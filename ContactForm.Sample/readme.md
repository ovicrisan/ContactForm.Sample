# ContactForm.Sample

A screenshot of the C# form, with reCAPTCHA demo enabled (JS form is very similar, just that uses AJAX instead of normal HTTP POST):

![ContactForm.Sample CS form](https://ovicrisan.github.io/ContactForm.Sample/ContactForm.Sample.3.png)

This project uses Razor Pages.

## References

The project uses Newtonsoft.Json instead of newer System.Text.Json because that's the package used by ContactForm library to process JSON, so it adds it in Startup.cs (needed to correctly parse 'g-recaptcha-response' value, with dashes).
Antiforgery token is disabled for these demos, but you can simply enable reCAPTHA, doing similar thing.

### ContactForm

This is actaully a sample project on top of [ContactForm project](https://github.com/ovicrisan/ContactForm), it it adds a reference to *OviCrisan.FormContact* package fron NuGet.org.

Main *ContactModel.cs* and reCAPTCHA validation comes from this library.

### FluentValidation

It's used to check form submission data validity, with appropriate error message for required fields, email address format and maximum length of the fields.
Because class being validated and validator class reside in different projects (and namespaces) the code needs to explicitly call validator function.

Error messages are displayed on the page for CS form and in a simple JavaScript alert() for AJAX version.

## Settings

All settings are provided in *appsettings.json* for locally ran version or via environment variables set up in *docker-compose.yml*, for Docker version:

```
"ContactFormSampleSettings": {
    "ContactFormWebEndpoint": "",
    "APIEndpoint": "https://localhost:44324/",
    "RecaptchaEnabled": false,
    "RecaptchaPublicKey": "6LeIxAcTAAAAAJcZVRqyHh71UMIEGNQ_MXjiZKhI",
    "RecaptchaSecretKey": "6LeIxAcTAAAAAGG-vFI1TnRWxMZNFuojJ4WifJWe"
  }
```

*APIEndpoint* - URL to the ContactForm.Sample.Postgres web API to save the data.
*Recaptcha* fields is to enable or disable Google reCAPTCHA, with appropriate keys registered on Google site. Here is using Google reCAPTCHA demo keys, publicly available.

*ContactFormWebEndpoint* - this is an optional setting to to an extra call to 
[ContactForm deplyment](https://github.com/ovicrisan/ContactForm/blob/master/ContactForm.Web/readme.md#azure-deployment)

When using environment variables it neds to be in this form:

```
environment:
    - ContactFormSampleSettings__APIEndpoint=http://contactformsamplepg:8000/
```

Both projects also add endpoints for healthcheck as */healthcheck*.

## PostService

To actually call other APIs over HTTP I'm using *IPostService* interface, implemented in *PostService.cs*. This class is used as strongly typed HttpClient instance, created via DI in Startup.cs.
Then just use *APIEndpoint* settings and/or optional *ContactFormWebEndpoint*, as described above.

## Run

### As multiple projects in VS

You can setup for each project to start with 'IIS Express' or 'Docker' (using included *Dockerfile*),
and then in Solutions properties page set each project with action = 'Start', like below:

![ContactForm.Sample multiple projects](https://ovicrisan.github.io/ContactForm.Sample/ContactForm.Sample.4.png)

Depending on each project start option (IIS Express or Docker) will start both projects, with or without browser launch. 
For Docker the port is 8080, so just open the browser to *http://localhost:8080* (no certificates are available for Docker version, at this time).

### Docker Compose

In the root of the project you have *docker-compose.yml* file already set up, so just try:

```
docker-compose up -d
```

You should see something like:

```
Starting contactformsample   ... done
Starting postgres            ... done
Starting contactformsamplepg ... done
```

Endpoints are setup with their container name, as Docker provide its own name resolution.

Postgres container has the username and password in plain text (don't forget, this is just a demo site, but can be customized upon your needs).
It also needs a volume pre-created (as described in main *readme* file) with:

```
docker create -v /var/lib/postgresql/data --name pgdataalpine alpine
```