# Step 1
FROM mcr.microsoft.com/dotnet/aspnet:8.0.0-jammy AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Step 2
FROM mcr.microsoft.com/dotnet/sdk:8.0.100-jammy AS build
RUN echo "Starting copy..."
WORKDIR /
COPY [".", "devprime/"]
RUN dotnet restore "devprime/ms-payment.sln"
#COPY . .
WORKDIR "/devprime"
RUN echo "Build..."
RUN dotnet build "ms-payment.sln" -c Release -o /app/build --no-restore

# Step 3
RUN echo "Publish..."
FROM build AS publish
RUN dotnet publish "ms-payment.sln" -c Release -o /app/publish --no-restore

# Step 4
FROM base AS final
WORKDIR /app
RUN echo "Copy..."
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "App.dll"]