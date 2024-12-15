# Carreras Service

Este microservicio maneja la gestión de carreras universitarias, incluyendo materias, requisitos previos y postrequisitos. Utiliza gRPC para comunicación y MongoDB como base de datos.



---

## Requisitos

### Herramientas Necesarias
- [.NET SDK](https://dotnet.microsoft.com/download) (versión compatible con el proyecto).
- [Docker](https://www.docker.com/) para la base de datos SQL Server.
- [Postman](https://www.postman.com/) o similar para pruebas.

### Archivo `.env`
Crea un archivo `.env` en la raíz del proyecto con el siguiente contenido:

```env
MONGODB_URI=mongodb://root:example@localhost:27017
MONGODB_DATABASE=CarrerasDB

RABBITMQ_HOST=localhost
RABBITMQ_PORT=5672
RABBITMQ_USER=guest
RABBITMQ_PASSWORD=guest
RABBITMQ_QUEUE=my_queue
RABBITMQ_EXCHANGE=my_exchange
RABBITMQ_ROUTING_KEY=my_routing_key

```

---

## Configuración

### Base de Datos (Docker Compose)

1. Asegúrate de tener un archivo `docker-compose.yml` con el siguiente contenido:

```yaml
version: "3.8"

services:
  mongodb:
    image: mongo:latest
    container_name: mongodb-container
    ports:
      - "27017:27017"
    environment:
      MONGO_INITDB_ROOT_USERNAME: root
      MONGO_INITDB_ROOT_PASSWORD: example
    volumes:
      - mongodb_data:/data/db

  rabbitmq:
    image: rabbitmq:management
    container_name: rabbitmq-container
    ports:
      - "5672:5672"
      - "15672:15672"
    environment:
      RABBITMQ_DEFAULT_USER: guest
      RABBITMQ_DEFAULT_PASS: guest

volumes:
  mongodb_data:
```

2. Levanta el servicio de la base de datos:

```bash
docker-compose up -d
```

3. Verifica que el contenedor esté corriendo:

```bash
docker ps
```


## Levantar el Servicio

1. Carga las dependencias:

```bash
dotnet restore
```


2. Ejecuta el proyecto:

```bash
dotnet run
```

3. El servicio deberia estar corriedo en:

```
gRPC Server: http://localhost:5190
```

---

## Endpoints Disponibles

Para probar el servicio con gRPC, utiliza un cliente compatible como Postman o BloomRPC.

Configura los siguientes parámetros en tu cliente:

```
Server: localhost
Port: 5190
Proto Root Directory: La carpeta Protos del proyecto.
Full Method: El método que deseas probar.
```
Métodos Disponibles:

- GetAllCarreras: Devuelve todas las carreras.
- GetAllSubjects: Devuelve todas las asignaturas, con prerequisitos y postrequisitos.
- GetAllPrerequisites: Retorna todos los requisitos previos.
- GetAllPostrequisites: Retorna todos los postrequisitos.



## Notas Finales

- Asegúrate de que las configuraciones de conexión coincidan con tu entorno.
- Este microservicio debe integrarse con los demás servicios del sistema para un funcionamiento completo.
- Utiliza Postman probar los endpoints y verificar las respuestas.

---

© 2024 - Arquitectura de Sistemas