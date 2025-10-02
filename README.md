# TaskManager


<img width="1915" height="1031" alt="image" src="https://github.com/user-attachments/assets/b04f6bbd-5689-4ae0-a756-d8506d5dbbd3" />



🗂️ TaskManager – ASP.NET Core MVC + API + xUnit

TaskManager is an ASP.NET Core MVC + Web API application for task management, built as a personal practice project. The goal was to strengthen backend concepts, REST API, authentication, testing, and observability.

📌 Implemented Features

✅ Add, edit, delete, and list tasks  
✅ JWT authentication + roles (User, Manager, Admin)  
✅ REST API for TaskItems (TaskItemsApiController)  
✅ [Authorize] protection for certain routes  
✅ Database: EF Core + SQL Server  
✅ Integrated logger (Serilog + Console)  
✅ Unit and integration tests (xUnit)  

🧪 Testing

✅ Unit tests (for Business Logic / Controllers)  
✅ Integration tests (for APIs)  
✅ Tests written with xUnit  
⚠️ Currently, integration tests write to the real database because the CustomWebApplicationFactory implementation with InMemoryDb was temporarily suspended (incompatibility with current infrastructure).  

❗ The main goal was to cover end-to-end integration logic. Full database cleanup can be added later (e.g. EnsureDeleted() per test).  

📊 Observability

✅ Logging with Serilog  
- Logging in TaskItemsApiController (GET, POST, PUT, DELETE)  
- Events logged with proper levels (Info, Warning, Error)  

Planned future integration with:  
- Seq (for log visualization)  
- Application Insights (for monitoring)  

🏗️ Technologies Used

- ASP.NET Core MVC 7  
- ASP.NET Core Web API  
- Entity Framework Core  
- SQL Server  
- Identity + JWT  
- xUnit  
- Serilog  
- Swashbuckle (Swagger)  
- InMemory EF Core (partially configured for testing)  

🧠 What I Learned

- Organizing MVC vs API controllers  
- Configuring WebApplicationFactory for testing  
- Working with ILogger<T> and logging events  
- Creating an authorization policy (RequireManagerOrAdmin)  
- Database seeding and reset  
- Diagnosing and debugging EF Core service/test-related errors  

---

## Additional Features:

<img width="1908" height="1027" alt="image" src="https://github.com/user-attachments/assets/e0bdaa27-4d81-4b15-b940-4d9edb18544c" />  

✅ Blazor functionality successfully integrated into the ASP.NET Core MVC app.  
✅ The Blazor counter now displays the number of active tasks retrieved from the API.  
✅ Successfully overcame obstacles such as:  

- Routing to _Host  
- Proper HttpClient injection  
- Binding errors (@DefaultLayout)  
- MainLayout configuration  
- Consuming the API  

---

## Additional Features:

(State Management) added on top of TaskManager.  
For Blazor, the healthiest way to learn Flux/Redux-style state management is Fluxor (Redux port for Blazor). Here’s a small, clean setup that:  

- keeps the task list in a global store  
- fetches data from the API via effects  
- synchronizes create/edit/delete/Toggle IsDone** in a single place  
- Blazor components only observe the store and dispatch actions (no direct HttpClient calls)  

---

## Additional Features:

Bootstrap integrated into the project for a clean UI + reusable small components (no extra tooling):  

- Unified Navbar & layout, modified Shared/MainLayout.razor.  
- Mini component library with TMCard, TMButton, ConfirmDialog, TaskRow.  
- Task list with a “card” layout + components in TaskList and “Add” page as a card.  

---

⚙️ Run Locally

Clone the project:  

```bash
git clone https://github.com/mariusStefan30/TaskManager.git

dotnet run

dotnet test








































Exempliu detaliat de **definire a obiectivelor SMART** pentru planul de upskilling:

---

## 🎯 Obiective SMART

### 1. Specifice (Specific)

* **Întrebare:** Ce anume vrei să știi sau să poți face concret la final?
* **Exemplu:**

  * „Voi fi capabil să construiesc o **aplicație web full-stack** folosind **ASP.NET Core MVC**, care să includă:

    1. **Autentificare** (login/logout cu cookie sau JWT),
    2. **Funcționalitate CRUD** (create/read/update/delete pentru entități),
    3. **Un API REST** separat (JSON),
    4. **Testare** unitară și de integrare,
    5. **Pipeline CI/CD** care să compileze, testeze și să deploy-eze automat.”

### 2. Măsurabile (Measurable)

* **Întrebare:** Cum știi că ai atins obiectivul? Ce dovezi concrete vei avea?
* **Exemplu:**

  * „Voi publica **6 proiecte separate** pe GitHub, unul pentru fiecare etapă:

    1. Proiect console C# (fundamente),
    2. Proiect MVC CRUD,
    3. Proiect API REST cu Swagger,
    4. Proiect cu autentificare JWT,
    5. Proiect cu suite de teste (xUnit + in-memory DB),
    6. Proiect CI/CD pe GitHub Actions/Azure DevOps.
  * Fiecare proiect va avea **README** detaliat.

### 3. Atingibile (Achievable)

* **Întrebare:** Ai resursele, timpul și nivelul de bază necesar pentru a realiza obiectivul?
* **Analiză:**

  * Timp disponibil: **5–7 ore/săptămână**
  * Experiență existentă: **3 ani VB.NET/WinForms + SQL**
  * Resurse: **Codecademy, documentație Microsoft, GitHub**
* **Concluzie:**

  * Obiectivul este realist pe **12–18 luni**, cu parcurgerea etapizată (vezi planul lunar).

### 4. Relevante (Relevant)

* **Întrebare:** În ce măsură acest obiectiv se aliniază cu ambițiile și nevoile tale profesionale?
* **Argumente:**

  * Te ajută să treci de la **VB.NET/WinForms** la **.NET modern** și **Web Dev**.
  * Crește-ți atractivitatea pentru roluri **Mid/Senior .NET Developer** în **UK**, unde cerința e **ASP.NET Core**, **API**, **DevOps**.
  * Îți dezvoltă și **gândirea critică** și **abilitatea de auto-learning**, competențe foarte căutate.

### 5. Încadrate în timp (Time-bound)

* **Întrebare:** Care sunt termenele limită pentru fiecare etapă și pentru obiectivul final?
* **Termene propuse:**

  * **Luna 1–3:** Fundamente C# & MVC CRUD
  * **Luna 4–6:** Web APIs & front-end light
  * **Luna 7–9:** Autentificare & Securitate
  * **Luna 10–12:** Testare & Observabilitate
  * **Luna 13–15:** Front-end modern (Blazor/React)
  * **Luna 16–18:** CI/CD & DevOps
* **Obiectiv final:** **Până la sfârșitul lunii 18**, să ai un portofoliu complet, gata de prezentat în interviuri.

---

Prin această formulare:

* **Specific:** știi exact ce vei învăța și construi.
* **Măsurabil:** ai livrabile concrete și indicatori (număr de proiecte, feedback, coverage teste).
* **Atingibil:** se potrivește cu timpul și experiența ta.
* **Relevant:** susține direcția spre joburile dorite.
* **Time-bound:** fiecare etapă are deadline, iar obiectivul final are dată clară.

Acum poți copia această structură în README-ul repo-ului tău și pe board-ul de management, ca să ai mereu clar obiectivele SMART la vedere.


#Task Manager features

-- CRUD Task Manager funtionality

Swagger documentation

MVC architecture

Entity Framework

Styling cu Bootstrap

Data validation

Filtrare / Cautare

Marcare rapida a task-urilor

Pagini de detaliu / imbunatatire UI Details si Edit / Adaugat navigatie la /TaskItems ai /TaskItemsCreate

-- Autentificare, Autorizare si Securitate features

Adaugare Identity si JWT

Roluri si Politici / Delete endpoint protejat , doar admin poate sterge un task

Securitate UI / Am implementat login/logout si tasks nu poate fi accesat fara sa fi autentificat

--Testare is Observabilitate

Unit Tests si Xunit pe TAskItemsControllerService, pe TaskItemsApiController si pe AuthController

Am adaugat Api integrations tests, stilll two of them are not working.

Am adaugat Serilog în TaskManager

TO FIX: ApiIntegrationsTest scrie in baza de date principala cand ruleaza testele

