# dotnetcore-web-docker

A simple example of Asp.NET Core running in Docker

### What's going on?

The `Dockerfile` starts with the `microsoft/dotnet:onbuild` image (available on [Docker Hub]) and runs `generateCert.sh` to create a self-signed certificate, then exports environment variables that instruct the code to use it when creating a TLS listener.

The application's startup code configures the `/static` folder as its content root so that requests for static content will resolve correctly, then adds MVC support so that Controllers will run when requested correctly. The new standard logging provider is also configured (but not used in this example).

A "Hello World" controller answers requests at `api/hello/[recipient]` to demonstrate MVC routing.

The port to listen on is set via an environment variable (`HTTPS_PORT`). This is a good practice, as it allows provisioning mechanisms like AWS ECS to configure the Docker container to listen on the port that makes sense for each Cluster.

### Build it!

```bash
docker build -t whatever-you-want .
```

### Run it!

```bash
docker run -it -p 443:9999 whatever-you-want
```

### Test it!

```bash
curl -k https://localhost/health-check.html
```

```html
<!DOCTYPE html>
<html>
  <head>
    <meta charset="utf-8">
    <title>Health Check</title>
  </head>
  <body>
    <h1>Hello, World!</h1>
    <p>If you can see this, the Asp.NET Core example is up and running in Docker.</p>
  </body>
</html>
```
=========

```bash
curl -k https://localhost/api/hello/World
```

```
Hello, World!
```

[Docker Hub]: https://hub.docker.com/
