#!/bin/bash
echo 'starting mysql db!'
# Wait for the mysql database to be available (usually takes 40 seconds),
# before running the database migration.
sleep 40 && /dbmigrator/DockerDatabaseExample &
disown
exec ./usr/local/bin/docker-entrypoint.sh mysqld
