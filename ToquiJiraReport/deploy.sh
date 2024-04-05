#!/bin/bash

IMAGE=$1
REGISTRY_PASSWORD_RO=$2
REGISTRY_USERNAME=$3
ENVIRONMENT=$4
JIRA_API=$5

docker_file_path=/home/ubuntu/docker-compose.yml

cat << EOF > $docker_file_path
version: '3.4'

services:
  jira-api:
    container_name: jira-api
    image: $IMAGE
    ports:
      - 80:80
    environment:
      - ASPNETCORE_ENVIRONMENT=${ENVIRONMENT}
      - ASPNETCORE_URLS=http://+:80
      - ApiUrls__JiraApi=${JIRA_API}
EOF

echo "$REGISTRY_PASSWORD_RO" | docker login -u "$REGISTRY_USERNAME" --password-stdin
docker system prune -af
docker compose up -d