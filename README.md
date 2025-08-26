# TaskManager


<img width="1915" height="1031" alt="image" src="https://github.com/user-attachments/assets/b04f6bbd-5689-4ae0-a756-d8506d5dbbd3" />



# 🗂️ TaskManager – ASP.NET Core MVC + API + xUnit

**TaskManager** este o aplicație ASP.NET Core MVC + Web API pentru gestionarea task-urilor, construită ca proiect de practică personală. Scopul a fost să consolidez concepte de backend, API REST, autentificare, testare și observabilitate.

---

## 📌 Funcționalități implementate

- ✅ Adăugare, editare, ștergere și listare taskuri
- ✅ Autentificare JWT + roluri (User, Manager, Admin)
- ✅ API REST pentru `TaskItems` (`TaskItemsApiController`)
- ✅ Protecție cu `[Authorize]` pentru anumite rute
- ✅ Bază de date: EF Core + SQL Server
- ✅ Logger integrat (Serilog + Console)
- ✅ Teste unitare și de integrare (xUnit)

---

## 🧪 Testare

- ✅ Teste **unitare** (pentru Business Logic / Controllers)
- ✅ Teste **de integrare** (pentru API-uri)
- ✅ Teste scrise cu **xUnit**
- ⚠️ În acest moment, testele de integrare scriu în baza de date reală deoarece implementarea `CustomWebApplicationFactory` cu InMemoryDb a fost temporar suspendată (incompatibilitate cu infrastructura actuală).

> ❗ Scopul principal a fost să acopăr logica de integrare end-to-end. Cleanup-ul complet al bazei de date poate fi implementat ulterior (ex: `EnsureDeleted()` per test).

---

## 📊 Observabilitate

- ✅ Logging cu **Serilog**
  - Logging în `TaskItemsApiController` (GET, POST, PUT, DELETE)
  - Evenimente logate cu nivele corecte (Info, Warning, Error)
-  Integrare ulterioară planificată cu:
  - **Seq** (pentru vizualizare loguri)
  - **Application Insights** (pentru monitorizare)

---

## 🏗️ Tehnologii folosite

- ASP.NET Core MVC 7
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- Identity + JWT
- xUnit
- Serilog
- Swashbuckle (Swagger)
- InMemory EF Core (parțial configurat pentru testare)

---

## 🧠 Ce am învățat

- Organizarea controllerelor MVC vs API
- Configurarea `WebApplicationFactory` pentru testare
- Lucrul cu `ILogger<T>` și înregistrarea evenimentelor
- Crearea unei politici de autorizare (`RequireManagerOrAdmin`)
- Seed și reset pentru baza de date
- Diagnosticul și depanarea erorilor legate de servicii EF Core și testare

---

## Am mai adaugat:

<img width="1908" height="1027" alt="image" src="https://github.com/user-attachments/assets/e0bdaa27-4d81-4b15-b940-4d9edb18544c" />


✅Funcționalitatea Blazor este integrată cu succes în aplicația ASP.NET Core MVC.
✅ Counter-ul Blazor funcționează și acum afișează numărul de taskuri active preluate din API.
✅ Depășit cu succes obstacole precum:

Rutarea către _Host

Injectarea corectă a HttpClient

Erori de binding (@DefaultLayout)

Configurarea MainLayout

Consumarea API-ului


## Am mai adaugat:

(State Management) și să o adăugăm peste ce ai deja în TaskManager. Pentru Blazor, cea mai sănătoasă cale de a învăța Flux/Redux-style este Fluxor (port Redux pentru Blazor). Îți dau un setup mic, curat, care:

ține lista de task-uri într-un store global

face fetch din API prin effects

sincronizează crea/edita/șterge/Toggle IsDone** dintr-un singur loc

componentele Blazor doar observă store-ul și trimit actions (fără HttpClient direct)

---
## Am mai adaugat:


Bootstrap în proiect, facem un UI curat + componente mici reutilizabile (fără tooling nou):

Navbar & layout unificat , modificat Shared/MainLayout.razor.

Mini-bibliotecă de componente cu TMCard, TMButton, ConfirmDialog, TaskRow.

Listă cu aspect “card” + componente in TaskList si pagina de “Add” în card.

---

## ⚙️ Rulare locală

1. Clonează proiectul:
   ```bash
   git clone https://github.com/mariusStefan30/TaskManager.git



   dotnet run




   dotnet test







































Iată un exempliu detaliat de **definire a obiectivelor SMART** pentru planul tău de upskilling:

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

