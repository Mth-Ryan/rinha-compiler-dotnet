FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /App

COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out
RUN chmod +x run.sh
COPY ./run.sh ./out
WORKDIR ./out

ENTRYPOINT [ "./run.sh"]
