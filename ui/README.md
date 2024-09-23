<div align="center" text-align="center" width="100%">
    <img src="/ui/.artifacts/icon.png" alt="League Leaders" align="center" width="20%">
</div>
<div align="center">
  <h1>League Leaders</h1>
  <h3>UI</h3>
</div>
<div align="center">
  <p>
  Think of it as your go-to spot for everything in Champions League. From tracking your favorite team's every move to discovering hidden gems, League Leaders has got you covered. So, grab your favorite jersey, kick back, and let the football fiesta begin! 
  </p>

<a href="">![Angular](https://img.shields.io/badge/Angular-18-red?style=flat)</a>

</div>

---

## How to work

This project was generated with [Angular CLI](https://github.com/angular/angular-cli) version 18.1.4.

---

## Running with Docker

### Steps to Build and Run the UI with Docker

1. **Build the Docker image for the UI:**

   Run the following command to build the Docker image for the UI:

   ```bash
   docker build -t league-leaders-ui:latest .

   ```

2. **Run the Docker container:**

   Run the container, mapping port 80 inside the container to port 80 on the host machine:

   ```bash
   docker run --name league-leaders-ui-container -d -p 80:80 league-leaders-ui:latest

   ```

3. **Access the UI:**

   After starting the container, navigate to `http://localhost:80/` or `http://localhost` in your browser to view the application.

---

## Using Docker Compose (With API and Database)

0. **Move to the root of the application - to folder `league-leaders`**

   ```bash
   cd ..
   ```

To run the UI alongside the API and database, use Docker Compose.

1. **Build and run the services using Docker Compose:**

   ```bash
   docker-compose up --build
   ```

2. **Access the UI:**

The UI will be available at `http://localhost:80/` or `http://localhost`

### Development server

Run `ng serve` for a dev server. Navigate to `http://localhost:4200/`. The application will automatically reload if you change any of the source files.

### Code scaffolding

Run `ng generate component component-name` to generate a new component. You can also use `ng generate directive|pipe|service|class|guard|interface|enum|module`.

### Build

Run `ng build` to build the project. The build artifacts will be stored in the `dist/` directory.

### Running unit tests

Run `ng test` to execute the unit tests via [Karma](https://karma-runner.github.io).

### Running end-to-end tests

Run `ng e2e` to execute the end-to-end tests via a platform of your choice. To use this command, you need to first add a package that implements end-to-end testing capabilities.

### Further help

To get more help on the Angular CLI use `ng help` or go check out the [Angular CLI Overview and Command Reference](https://angular.dev/tools/cli) page.
