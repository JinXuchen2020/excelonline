#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ExcelOnline.Api/ExcelOnline.Api.csproj", "ExcelOnline.Api/"]
COPY ["ExcelOnline.Data/ExcelOnline.Data.csproj", "ExcelOnline.Data/"]
RUN dotnet restore "ExcelOnline.Api/ExcelOnline.Api.csproj"
COPY . .
WORKDIR "/src/ExcelOnline.Api"
RUN dotnet build "ExcelOnline.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ExcelOnline.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS="http://*:5000"
EXPOSE 5000
ENTRYPOINT ["dotnet", "ExcelOnline.Api.dll"]