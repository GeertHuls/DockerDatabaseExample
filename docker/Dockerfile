FROM mcr.microsoft.com/dotnet/core/sdk:2.2

WORKDIR /home/src
COPY src/. /home/src

RUN dotnet publish -c release --self-contained --runtime linux-x64 --framework netcoreapp2.2

ENTRYPOINT [ "bash" ]