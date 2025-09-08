# MVCMovie
Christine Lee
COP 2839: ASP.NET Programming with C#
Professor Castillo

Week 2:
In the Movie App application, the separation concerns with the MvcMovie project were the separation of the logic in the programming, including the interactions of the program with the database. Since the logic for checking and retrieving the data were together, it makes testing and adjusting the program difficult because the logic and infrastructure were in the same program. This query logic was then separated into the IMovieService interface and MovieService class to allow each part to have a single responsibility. These classes separately handles all of the movies operations like getting a list of all the movies and adding, editing, or deleting database information from the code of the controller. To ensure it's integrity, a unit test using xunit was created to demonstrate the benefits of the improved architectural principles applied after the additional classes were incorporated into the program.


Week 1:
This application is caled Movie App and holds a database of movies. User can create, edit, or delete entries.
Available fields for each movie object is Title, Release Date, Genre, Price, and Rating with specified validations during entry.
A search form is created allowing filtering of Genre and title by keyword.
The app is opened through Visual Studio Community using a Movies view, created with a Movies Controller with a logger helper to log actions of Info, Warn, Error, and Debug.
Being an MVC (Model-View-Controller), this application is not dependent on JavaScript support and performs most of the application logic on the server.
