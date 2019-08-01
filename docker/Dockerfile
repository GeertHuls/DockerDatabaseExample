# escape=`
FROM mcr.microsoft.com/dotnet/core/sdk:2.2 as builder

WORKDIR /home/src
COPY src/. /home/src

RUN dotnet publish -c release `
    --self-contained `
    --runtime linux-x64 `
    --framework netcoreapp2.2 `
    --output /home/publish

FROM mysql:5.7

COPY --from=builder /home/publish /home/migrator
COPY docker/custom-entrypoint.sh .
