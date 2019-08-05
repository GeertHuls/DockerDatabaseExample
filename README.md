# DockerDatabaseExample

This docker project produces an image to run linux mysql containers.

At startup of the container a schema migration project written in C# will ensure that the proper database is created. In case of an already existing database (eg when using volumes), the migration project will only try to update the database and will not attempt to create a new one.

## Usage

To build an image from the docker file run:
> docker build -f docker/Dockerfile -t <img_name>:\<tag> .

To run a container from the image with the default datbase password:
> docker run -d <img_name>:\<tag>

To run a container from the image with a custom password (recommended):
> docker run -e MYSQL_ROOT_PASSWORD=mynewpassword -d <img_name>:\<tag>
