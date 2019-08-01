#!/bin/bash
echo 'starting mysql db!'
exec ./usr/local/bin/custom-docker-entrypoint.sh mysqld
