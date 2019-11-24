# ContactForm.Sample

These 2 projects are sample ASP.NET Core microservices build on top of [ContactForm project](https://github.com/ovicrisan/ContactForm). 
First one, [ContactForm.Sample](https://github.com/ovicrisan/ContactForm.Sample/tree/master/ContactForm.Sample) 
(see [readme.md file](https://github.com/ovicrisan/ContactForm.Sample/tree/master/ContactForm.Sample/readme.md)) is only displaying 2 contact forms, one that uses C# and the other using JavaScript (AJAX)
to call second project, [Contactform.Sample.Postgres](https://github.com/ovicrisan/ContactForm.Sample/tree/master/Contactform.Sample.Postgres)
(see [readme.md file](https://github.com/ovicrisan/ContactForm.Sample/tree/master/Contactform.Sample.Postgres/readme.md)), to save data.

The goal of these projects are simply to try using RESTful API over HTTP calls, using Docker Compose and Kubernetes.

![ContactForm.Sample diagram](https://ovicrisan.github.io/ContactForm.Sample/ContactForm.Sample.1.png)

Here's a screenshot of the main page:

![ContactForm.Sample main page](https://ovicrisan.github.io/ContactForm.Sample/ContactForm.Sample.2.png)

## Docker compose

The *docker-compose.yml* file starts 3 services: (1) main web form to collect data, 
(2) RESTful API to save data (in this case in a Postgres database), and (3) Postgres service itself.

All containers are named and then uses environment variables to set API Endpoint and DB connection string.

To build all images from the provided *Dockerfile* use *build.ps1* script, from root folder like this:

`.\build.ps1`

Then just a simple

`docker-compose up -d`

The images are also available on Docker Hub [here](https://hub.docker.com/repository/docker/ovicrisan/contactformsample) 
and [here](https://hub.docker.com/repository/docker/ovicrisan/contactformsamplepg):

```
docker pull ovicrisan/contactformsample
docker pull ovicrisan/contactformsamplepg
```

## Postgres image

To test with Postgres database you have the option to install it locally from 
[postgresql.org/download](https://www.postgresql.org/download/) 
or just use Docker image from [hub.docker.com/_/postgres](https://hub.docker.com/_/postgres):

`docker pull postgres`

Latest is version 12, but to have it working on Windows 10 I used the trick from 
[here](https://elanderson.net/2018/02/setup-postgresql-on-windows-with-docker/):

```
docker create -v /var/lib/postgresql/data --name pgdataalpine alpine
docker run --rm --name postgres -e POSTGRES_PASSWORD=postgres -d -p 5432:5432 --volumes-from pgdataalpine postgres
```

This create a volume in an Alpine image used by Postgres to store the data, persistently. 
Other methods to directly map a local volume to */var/lib/postgresql/data* failed with some permission errors, so in the end I just used Eric's suggestion.

**Read more:**

- [ContactForm.Sample readme.md](https://github.com/ovicrisan/ContactForm.Sample/tree/master/ContactForm.Sample/readme.md)
- [Contactform.Sample.Postgres readme.md](https://github.com/ovicrisan/ContactForm.Sample/tree/master/Contactform.Sample.Postgres/readme.md)
