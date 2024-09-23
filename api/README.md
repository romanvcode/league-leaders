<div align="center" text-align="center" width="100%">
    <img src="/api/.artifacts/icon.png" alt="League Leaders" align="center" width="20%">
</div>
<div align="center">
  <h1>League Leaders</h1>
  <h3>API</h3>
</div>
<div align="center">
  <p>
  Think of it as your go-to spot for everything in Champions League. From tracking your favorite team's every move to discovering hidden gems, League Leaders has got you covered. So, grab your favorite jersey, kick back, and let the football fiesta begin! 
  </p>

<a href="">![ .NET](https://img.shields.io/badge/.NET-8-blue?style=flat)</a>

</div>

## How to work

This Web API provides a platform to manage Champions League results and next matches.

### Prerequisites

- [.NET SDK (version 8)](https://dotnet.microsoft.com/download)

## Running the API

### Development

To run the API in a development environment:

```bash
dotnet run
```

The API will be available at `https://localhost:7250/`

---

## Running with Docker

### Steps to Build and Run the UI with Docker

1. **Build the Docker image for the API:**

   Run the following command to build the Docker image for the API:

   ```bash
   docker build -t league-leaders-api:latest .

   ```

2. **Run the Docker container:**

   Run the container, mapping port 8080 inside the container to port 8080 on the host machine:

   ```bash
   docker run --name league-leaders-api-container -d -p 8080:8080 league-leaders-api:latest

   ```

3. **Access the API:**

   After starting the container, navigate to `http://localhost:8080/` in your browser to view the application.

---

## Using Docker Compose (With UI and Database)

0. **Move to the root of the application - to folder `league-leaders`**

   ```bash
   cd ..
   ```

To run the API alongside the UI and database, use Docker Compose.

1. **Build and run the services using Docker Compose:**

   ```bash
   docker-compose up --build
   ```

2. **Access the API:**

The API will be available at `http://localhost:8080/`

### Current implementation and limitations

- TBD
