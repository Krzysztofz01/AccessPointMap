![apm logo](https://user-images.githubusercontent.com/46250989/123469319-2b96bb80-d5f3-11eb-9b53-d8be8fa73c0b.png)
#  AccessPointMap

The AccessPointMap project can be described as "White hat war-driving". AccessPointMap consists of the collection and analysis of access points data, their security and geolocation (without connecting to the network or interacting with it in any way). In contrast to similar projects like (WiGLE), the project does not focus on statistics on a global, but on a local scale. The main assumption of the project is to pay attention to the lack of education and knowledge of people about their own networks, which often have a default, unsafe configuration. At the project site, you can spot your network on a map or use a search engine. From the technical side, the project consists of a mobile application for gathering information. The information is then sent to a server where the analysis is performed, the data is stored in a database and can be viewed through the web application. Anyone can join the project by collecting data or writing their own analysis algorithms using the AccessPointMap Web API.A more detailed description of the entire project can be found in the PDF file.

##  Segments

The project consists of several parts, microservices and applications:

- SQL Server database as a microservice

- ASP .NET Core REST API as a microservice

- Angular application for clients and administration

- Xamarin mobile application for collecting data

- A special device based on the ESP8266 microcontroller used to collect data slightly more efficiently than the mobile application

  

##  Technologies

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