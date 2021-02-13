# AccessPointMap
The main goal of the AccessPointMap project is to collect, analyze and process data on the presence of WiFi access points, their parameters and geolocation. In contrast to similar projects, the project does not focus on statistics on a global, but on a local scale. A particular access point is not just a number, any device that acts as an access point has many features. One of the important features is its security! The aim of the project is also to pay attention to how many people do not use their WiFi encryption or use old standards such as WEP. A more detailed description of the entire project can be found in the PDF file.

## Segments
The project consists of several parts, microservices and applications:

- SQL Server database as a microservice
- ASP .NET Core REST API as a microservice
- Angular application for clients and administration
- Xamarin mobile application for collecting data
- A special device based on the ESP8266 microcontroller used to collect data slightly more efficiently than the mobile application

## Technologies
In addition to fundamental technologies such as .NET Core, Angular and Xamarin, the following technologies and libraries have been used:

- [.NET Core](https://dotnet.microsoft.com) - a open-source, cross-platform, managed computer software framework for Windows, Linux, and macOS operating systems.
- [Angular](https://angular.io) - a open-source web application framework that allows you to create "Single-Page" client interfaces.
- [Xamarin](https://dotnet.microsoft.com/apps/xamarin) - technology based on .NET that provides tools for creating native Android and iOS applications.
- [Docker](https://www.docker.com) - Virtualization at the operating system level. The division into microservices provides high scalability of the entire project.
- [Hangfire](https://www.hangfire.io) - An easy way to perform background processing in .NET and .NET Core applications.
- [Entity Framework](https://docs.microsoft.com/en-us/ef/) - Modern object-database mapper for .NET
- [OpenLayers](https://openlayers.org) - This technology makes it easy to put a dynamic map in any web page.
- [ChartJS](https://www.chartjs.org) - Simple yet flexible JavaScript charting for designers & developers
- [Bootstrap](https://getbootstrap.com) - A framework for creating responsive user interfaces
- [TypeScript](https://www.typescriptlang.org) - an open-source language which builds on JavaScript, one of the world’s most used tools, by adding static type definitions.
- [JsonWebToken](https://jwt.io) - an open, industry standard RFC 7519 method for representing claims securely between two parties.