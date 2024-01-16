# Utiliza la imagen oficial de ASP.NET Core como base
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

EXPOSE 80
EXPOSE 5024
COPY ./*.csproj ./

# Copia los archivos del proyecto al contenedor
COPY . .

# Restaura las dependencias y compila la aplicación
RUN ls -la
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Utiliza una imagen más ligera para la aplicación final
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Define el comando de inicio de la aplicación
CMD ["dotnet", "WebAPIPdroduct.csproj"]

