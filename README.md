# Cosmos Odyssey
Live site: https://delightful-tree-06b279703-preview.westeurope.6.azurestaticapps.net/

## Technologies Used

* **C# (.NET):** The primary programming language and framework.
* **ASP.NET Core:** For building the Web API.
*  **SQLite:** Database for local development.
* **Supabase PostgreSQL:** Database for live app.
* **Blazor WebAssembly:** For the frontend.
*  **Quartz:** For the background job.

## Prerequisites

* **.NET SDK:** The project uses .NET 8.

## Local Development Setup

1.  **Clone the Repository:**
  
2.  **Restore NuGet Packages (API and Blazor):**

    ```bash
    dotnet restore ./API/
    dotnet restore ./Blazor/
    ```

3.  **Alternative if you are using Visual Studio:**

     Create a new startup profile, with multiple projects and select both projects and run.
     Navigate to `http://localhost:5001` to use the web app.

5.  **Run the Web API:**

    ```bash
    dotnet run --project ./API/
    ```

    The API will start running, and you can access it at `http://localhost:5000`.

6.  **Run the Blazor Frontend:**

    * Open a new terminal window.
    * Run the Blazor application:

        ```bash
        dotnet run --project ./Blazor
        ```

    * The Blazor app will start running, and you can access it at `http://localhost:5001`.
7. Navigate to `http://localhost:5001` to use the web app.

