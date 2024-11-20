FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY "/TaskMasterAPI/TaskMasterAPI.csproj" "TaskMasterAPI/"
COPY "TaskMasterAPI.BLL/TaskMasterAPI.BLL.csproj" "TaskMasterAPI.BLL/"
COPY "TaskMasterAPI.DAL/TaskMasterAPI.DAL.csproj" "TaskMasterAPI.DAL/"
COPY "TaskMasterAPI.Models/TaskMasterAPI.Models.csproj" "TaskMasterAPI.Models/"
RUN dotnet restore "TaskMasterAPI/TaskMasterAPI.csproj"
COPY . .
WORKDIR "/src/TaskMasterAPI"
RUN dotnet build "TaskMasterAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TaskMasterAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskMasterAPI.dll"]
