My primary focus was on ensuring code clarity, cleanliness, and establishing a robust infrastructure.

# Tasks I Didn't Have Time to Address:
   - Integration Testing: Unfortunately, due to time constraints, I had to skip this part entirely. Given more time,
     I would have set up an SQLite database and implemented integration tests to ensure that API endpoints return the expected results.
     For instance, verifying that a GET request with a specific ID returns the corresponding DTO with the same ID.

   - Exception Handling: Similarly, I had to forgo implementing proper exception handling. Given more time,
     I would have created custom exceptions within the Domain layer. Subsequently, I would add an Exception Handling middleware to return
     appropriate messages and status codes based on the encountered exceptions.

   - Application Robustness: While I acknowledge that the application may lack robustness and could potentially have issues with various corner cases,
     my primary objective was to demonstrate my approach to code design. Regrettably, given the scope of the task, robustness had to be sacrificed.

# Areas for Improvement:
   - Auditing Approach: While I believe my approach to auditing is sound, setting the HttpRequestType from EntityState may not be the most optimal solution.
     In a real-world scenario, I would prefer implementing auditing via an ActionFilterAttribute on the API endpoint and perform the audit database call
     in a similar manner. Additionally, with a well-designed audit database schema and some adjustments to the current approach,
     it's feasible to automate auditing without the need for code changes when introducing new entities.

   - Unit Testing: There is room for improvement in unit tests. Utilizing Fluent Validation, employing better naming conventions,
     and avoiding hard-coded percentages would enhance the robustness and maintainability of the tests.

   - Documentation: Admittedly, documentation could be more comprehensive. Clearer documentation is easier to produce when the domain is well understood,
     and the team has established guidelines. Given more time, I would have invested in enhancing documentation quality.

   - Logging: While I recognize the importance of logging, I didn't have sufficient time to implement an extensive logging solution. In future iterations,
     I would aim to automate logging wherever possible and appropriate.

   - Validation: Improvements could be made to validation messages and error handling. Leveraging Fluent Validation to set error codes alongside messages
     would enable more effective handling of validation errors.

   - Premium Calculation: Although the premium calculation solution implemented is extensible and adaptable to changing business logic, there is room
     for enhancement. Storing calculation parameters in a database or configuration file would improve flexibility. Additionally,
     the code could be further refined to accommodate different base rates for various vessel types.

# Areas I Consider Successful:
   - Architecture: I am confident that the application is structured according to Clean Architecture guidelines, facilitating maintainability and scalability.

   - Premium Calculation and Auditing: Despite not being perfect, the premium calculation and auditing functionalities demonstrate a promising initial approach.
     They lay the groundwork for solutions that can adapt to evolving business logic without necessitating code changes.

   - Abstractions and Extensibility: I made effective use of abstractions and ensured code extensibility and granularity, which are crucial for long-term maintainability and scalability.
