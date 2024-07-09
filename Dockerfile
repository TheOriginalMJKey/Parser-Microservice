# Use the bullseye-slim image with .NET 8.0 SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Install required packages for native AOT compilation
RUN apt-get update && \
    apt-get install -y --no-install-recommends \
        clang \
        libc6-dev \
        libclang-14-dev \
        libllvm14 \
        zlib1g-dev && \
    rm -rf /var/lib/apt/lists/*

WORKDIR /app

RUN dotnet restore

COPY . .


RUN dotnet publish -c Release -r linux-x64 -o out

# Use the aspnet image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

COPY --from=build /app/out .

EXPOSE 80

ENTRYPOINT ["dotnet", "ProJect.dll"]
