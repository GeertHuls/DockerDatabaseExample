# escape=`
FROM mcr.microsoft.com/dotnet/core/sdk:2.2 as builder

WORKDIR /home/src
COPY src/. /home/src
COPY docker/custom-entrypoint.sh /home

RUN dotnet publish -c release `
    --self-contained `
    --runtime linux-x64 `
    --framework netcoreapp2.2 `
    --output /home/publish && `
    apt update && `
    apt-get install dos2unix && `
    dos2unix /home/custom-entrypoint.sh

FROM mysql:5.7

ENV MYSQL_ROOT_PASSWORD="defaultpwd"

COPY --from=builder /home/publish /dbmigrator
COPY --from=builder /home/custom-entrypoint.sh .

ENTRYPOINT [ "/custom-entrypoint.sh" ]
