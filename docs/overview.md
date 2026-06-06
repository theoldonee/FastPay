# FastPay Project Overview

## What FastPay Is

FastPay is intended to be a simple payroll management system for restaurants.

The first version should let a restaurant owner:

1. Create and manage employee records.
2. Assign an hourly pay rate to each employee.
3. Record the hours each employee worked.
4. Group those hours into payroll cycles, initially every two weeks.
5. Calculate and review the amount owed to each employee.

The first version should remain small. Features such as tax calculation, automated payments, employee self-service, multi-restaurant support, and advanced reporting are future work.

## Current State of the Codebase

The repository is a starter full-stack application created from the Visual Studio ASP.NET Core and React template. It has two main projects:

- `FastPay.Server/`: the ASP.NET Core server, written in C#.
- `fastpay.client/`: the React client, written in TypeScript.

Both projects are listed in `FastPay.sln`, the Visual Studio solution file.

At the moment, the application does **not** contain FastPay payroll features. It only contains a sample weather forecast flow that demonstrates how the React client can request data from the ASP.NET Core server.

There is currently:

- no database;
- no employee, work-hour, payroll-cycle, or payroll-record models;
- no authentication;
- no real payroll API;
- no test project.

This makes the current repository a clean starting point rather than a partially completed payroll system.

## High-Level Architecture

FastPay currently follows a client-server structure:

```text
User's browser
    |
    | Opens the React interface and performs actions
    v
React client (fastpay.client)
    |
    | Sends HTTP requests and receives JSON
    v
ASP.NET Core server (FastPay.Server)
    |
    | Future: reads and writes payroll data
    v
Future database
```

The responsibilities should remain separate:

- The **client** displays pages, forms, buttons, tables, loading states, and errors.
- The **server** validates requests, applies business rules, calculates payroll, and controls database access.
- The **database** stores employees, hours, payroll cycles, and calculated payroll records.

The client should never connect directly to the database or be trusted to perform the final payroll calculation.

## Root Folder Structure

```text
FastPay/
├── FastPay.sln
├── README.md
├── docs/
│   └── overview.md
├── FastPay.Server/
└── fastpay.client/
```

### Important Root Files

- `FastPay.sln`: groups the server and client projects so they can be opened and built together in Visual Studio.
- `README.md`: currently only contains the project title.
- `docs/overview.md`: this beginner-friendly guide to the project.

Generated folders such as `bin/`, `obj/`, `node_modules/`, and the future client `dist/` folder are build output or installed dependencies. They are not the main application source code.

## Server Structure

The server lives in `FastPay.Server/`.

```text
FastPay.Server/
├── Controllers/
│   └── WeatherForecastController.cs
├── Properties/
│   └── launchSettings.json
├── Program.cs
├── WeatherForecast.cs
├── FastPay.Server.csproj
├── FastPay.Server.http
├── appsettings.json
└── appsettings.Development.json
```

### What Each Server File Does

- `Program.cs`: the server entry point. It registers server features and defines the HTTP request pipeline. It currently enables controllers, Swagger, HTTPS redirection, static files, and the React fallback page.
- `Controllers/WeatherForecastController.cs`: the only current API controller. A controller receives HTTP requests and returns responses. Its `GET /weatherforecast` endpoint returns sample JSON data.
- `WeatherForecast.cs`: the C# shape of one weather forecast returned by the sample endpoint.
- `FastPay.Server.csproj`: defines the server project, targets .NET 8, references Swagger and the SPA proxy, and links the client project.
- `Properties/launchSettings.json`: defines local server addresses and development startup behavior. The main HTTPS address is `https://localhost:7239`.
- `appsettings.json`: shared server configuration. It currently only contains logging settings and allowed hosts.
- `appsettings.Development.json`: development-only configuration.
- `FastPay.Server.http`: a small request file that can manually call the sample server endpoint.

### How a Server Request Is Handled

For the existing sample request:

1. The server starts in `Program.cs`.
2. `builder.Services.AddControllers()` registers controller support.
3. `app.MapControllers()` exposes controller routes.
4. A request reaches `WeatherForecastController` because its route is `/weatherforecast`.
5. The controller's `Get()` method creates sample forecast objects.
6. ASP.NET Core converts those C# objects into JSON and returns them to the client.

## Client Structure

The client lives in `fastpay.client/`.

```text
fastpay.client/
├── public/
│   ├── favicon.svg
│   └── icons.svg
├── src/
│   ├── assets/
│   ├── App.tsx
│   ├── App.css
│   ├── main.tsx
│   └── index.css
├── index.html
├── package.json
├── vite.config.ts
├── eslint.config.js
├── tsconfig.json
├── tsconfig.app.json
├── tsconfig.node.json
└── fastpay.client.esproj
```

### What Each Client Area Does

- `index.html`: contains the HTML element where React is loaded.
- `src/main.tsx`: the client entry point. It renders the main `App` component into the page.
- `src/App.tsx`: the only current page/component. It requests weather data and displays it in a table.
- `src/App.css`: styles used by `App.tsx`.
- `src/index.css`: global styles used across the client.
- `src/assets/`: images imported by client code.
- `public/`: files served directly without being imported into TypeScript.
- `package.json`: lists client dependencies and commands such as `npm run dev`, `npm run build`, and `npm run lint`.
- `vite.config.ts`: configures the Vite development server, local HTTPS certificate, and proxy to the ASP.NET Core server.
- `tsconfig*.json`: TypeScript compiler settings.
- `eslint.config.js`: client code-quality rules.
- `fastpay.client.esproj`: allows Visual Studio and the .NET solution to treat the client as a project.

## How the Client Fetches Data From the Server

The current request begins in `fastpay.client/src/App.tsx`:

```ts
const response = await fetch('weatherforecast');
const data = await response.json();
setForecasts(data);
```

The complete development flow is:

1. React loads `App.tsx`.
2. React's `useEffect` calls `populateWeatherData()` after the component first appears.
3. `fetch('weatherforecast')` sends a browser request to the client development server.
4. The proxy rule in `vite.config.ts` matches `/weatherforecast`.
5. Vite forwards the request to the ASP.NET Core server, normally at `https://localhost:7239`.
6. `WeatherForecastController.Get()` handles the request and returns C# objects.
7. ASP.NET Core serializes the objects into JSON using camel-case property names.
8. The client parses the JSON and stores it in React state.
9. React renders the returned data in a table.

The Vite proxy is important during development. It lets client code use a short relative URL such as `/weatherforecast` instead of hard-coding a server address. It also avoids common browser cross-origin problems during local development.

The same pattern can be used for FastPay endpoints. For example, the future client may call:

```ts
const response = await fetch('/api/employees');
```

and a future server controller may expose:

```csharp
[ApiController]
[Route("api/employees")]
public class EmployeesController : ControllerBase
{
    [HttpGet]
    public IActionResult GetEmployees()
    {
        // Read employees from the database and return them as JSON.
        return Ok();
    }
}
```

When real API routes are introduced, the proxy in `vite.config.ts` should be changed from matching only `/weatherforecast` to matching `/api`.

## Running the Current Application

### From Visual Studio

Open `FastPay.sln` and run the `FastPay.Server` HTTPS profile. The ASP.NET Core SPA proxy is configured to start the React development server as well.

### From Terminals

Start the server:

```bash
dotnet run --project FastPay.Server --launch-profile https
```

Start the client in a second terminal if it was not started automatically:

```bash
cd fastpay.client
npm install
npm run dev
```

Useful checks:

```bash
dotnet build FastPay.sln
cd fastpay.client && npm run lint
cd fastpay.client && npm run build
```

In development:

- the server normally runs at `https://localhost:7239`;
- the Vite client normally runs at `https://localhost:64348`;
- Swagger is available from the server at `/swagger`.

## Suggested Structure as FastPay Grows

The current structure is enough for the starter example, but payroll features should not all be placed into `App.tsx` and controller files. A practical next structure is:

```text
FastPay.Server/
├── Controllers/
│   ├── EmployeesController.cs
│   ├── WorkHoursController.cs
│   └── PayrollCyclesController.cs
├── Models/
│   ├── Employee.cs
│   ├── WorkEntry.cs
│   └── PayrollCycle.cs
├── Contracts/
│   ├── Employees/
│   ├── WorkHours/
│   └── Payroll/
├── Services/
│   └── PayrollService.cs
├── Data/
│   └── FastPayDbContext.cs
└── Program.cs

fastpay.client/src/
├── api/
│   ├── employees.ts
│   ├── workHours.ts
│   └── payroll.ts
├── components/
├── pages/
│   ├── EmployeesPage.tsx
│   ├── WorkHoursPage.tsx
│   └── PayrollPage.tsx
├── types/
└── App.tsx
```

These folders are a recommendation; they do not exist yet.

### Recommended Responsibilities

- **Controllers** receive HTTP requests, validate the basic request shape, call the appropriate service, and return an HTTP response.
- **Models** represent data stored by the server.
- **Contracts** define the request and response shapes sent between the client and server. Keeping these separate from database models prevents the API from accidentally exposing internal fields.
- **Services** contain business rules, especially payroll calculations. Controllers should not contain the main calculation logic.
- **Data** contains database access and configuration.
- Client **api** files contain `fetch` calls in one place instead of spreading them throughout page components.
- Client **pages** represent full screens.
- Client **components** contain reusable interface pieces such as forms, tables, and buttons.
- Client **types** describe the JSON data the client expects from the server.

## High-Level Build Plan

The simplest way to build Version 1 with the current structure is to complete one working feature flow at a time.

### 1. Establish the Data Model

Start with the smallest necessary records:

- `Employee`: name, hourly rate, active status, and timestamps.
- `PayrollCycle`: start date, end date, and status.
- `WorkEntry`: employee, payroll cycle, and hours worked.
- A payroll result or calculation response containing hours, rate, and gross pay.

Add a database and migrations only after agreeing on these fields and their rules.

### 2. Build Employee Management End to End

Create server endpoints to add, list, update, and deactivate employees. Then create the employee list and employee form in React. This proves that the database, server, client fetch layer, validation, and error handling work together.

### 3. Build Work-Hour Entry

Create payroll cycles and allow the owner to record or update each employee's hours for a cycle. The server should reject invalid values such as negative hours or entries for inactive employees.

### 4. Build Payroll Calculation

Place the calculation on the server:

```text
gross pay = hours worked x hourly rate
```

The client should send or select the payroll cycle, then display the server's calculated results for review.

### 5. Add Reliability Before Expanding Scope

Before adding future features, add:

- server unit tests for payroll calculations;
- server API tests for employee, hours, and payroll endpoints;
- client tests for important forms and pages;
- clear validation and user-friendly error messages;
- authentication before storing real restaurant or employee data;
- secrets and connection strings through environment-specific configuration, never committed source files.

## A Good First Vertical Feature

The first implementation milestone should be:

> A restaurant owner can add one employee with an hourly rate, record that employee's hours for a two-week period, and see the correctly calculated gross pay.

This is a useful end-to-end slice because it touches every necessary layer without introducing out-of-scope features. Once it works reliably, the same patterns can be extended to employee editing, multiple employees, payroll history, and future functionality.
