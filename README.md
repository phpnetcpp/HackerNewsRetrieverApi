# Hacker News Retriever API

This is a Minimal API project in ASP.NET that provides an endpoint to retrieve details of the first `n` "best stories" from the Hacker News API, sorted by their score in descending order

## Getting Started

### Prerequisites

- [.NET 7.0](https://dotnet.microsoft.com/download/dotnet/7.0)

### Installation

1. Clone this repository:

```
   git clone https://github.com/phpnetcpp/HackerNewsRetrieverApi.git
   cd HackerNewsRetrieverApi
```
   
2. Build and run the application:

You can run the application using .NET CLI or your preferred IDE with launch profiles

2.1. Using .NET CLI:

```
   dotnet run
```

2.1.1. You can use any profile from launchSettings.json file

```
   dotnet run --launch-profile https
```

2.2. Using Visual Studio Code:

Open the project folder in Visual Studio Code, and press F5 to run the application. Ensure that the .vscode/launch.json file contains a configuration for running the API

2.3. Using Visual Studio:

Open the project in Visual Studio and select the appropriate launch profile (e.g., https) from the dropdown menu in the toolbar. Then, press the "Run" button or hit F5 to start the application

3. The API will be available at http://localhost:5138 or https://localhost:7188:

   > Open a web browser or use a tool like Postman to test the API by making GET requests to http://localhost:5138/api/v1/stories/best?number={number} where {number} is the number of stories you want to retrieve

## API Endpoints

   > GET /api/v1/stories/best

Retrieves the details of the first n "best stories" from Hacker News

### Parameters

* number (required): The number of best stories to retrieve


### Example

To retrieve the details of the top 10 best stories:

   > GET https://localhost:7188/api/v1/stories/best?number=10


## Assumptions

* The Hacker News API URLs are assumed to be stable and accessible during API requests
* The API does not implement caching or rate limiting, which might be added for production use

## Enhancements and Future Improvements

* Implement caching to reduce the load on the Hacker News API
* Add rate limiting to prevent overloading the Hacker News API
* Implement pagination for the results to handle large n values efficiently
* Create a more comprehensive error handling system
* Add unit tests to ensure the reliability of the API
* Update to a distributed cache in a distributed environment to ensure consistency across multiple instances of the application

## License
This project is licensed under the MIT License - see the LICENSE file for details.
