# app-awesome-contactz

Awesome Contactz is a cross-platform mobile application sample for iOS, Android, and Windows built with Xamarin.Forms and leverages several service sinside of Azure including Azure AD B2C, Functions, CosmosDB, and cognitive services.


### Data
Sample data for this application was imported from the [Microsoft Cloud Developer Advocates](https://developer.microsoft.com/en-us/advocates/) (CDAs). When a CDA logs in (who is part of Azure Active Directory) they are able to update their profile, photos, and location. All users have the ability to skip login and browse our beautiful contact list of CDAs.

## Smarts
Since CDAs travel often to help developers around the world build amazing things we built into the application a way for CDAs to update their current city/state/country and a way for other CDAs to see who is nearby. This leverages built in capabilities of the devices such as geolocation and also leverages advanced geolocation capabilities of CosmosDB for fast queries.

To make getting into the application faster we have integrated facial recognition that CDAs can use after their initial login to get into the app faster.

## Services

### [Azure Active Directory B2C](https://azure.microsoft.com/en-us/services/active-directory-b2c/)


### [CosmosDB](https://azure.microsoft.com/en-us/services/cosmos-db/)
Our CDAs are located all through the world. We leverage CosmosDB for it's geo-replication to ensure optimal performance in the mobile applications. We also leverage CosmosDB advanced queries to get specific data based on the user's geolocation.


### [Azure Functions](https://azure.microsoft.com/en-us/services/functions/)
We use serverless architecture for several features of the application including:

* Updating geolocation data. The CDA will update their location with an Azure Function that will detect their city/state/country and get an annonymous Latitude and Longitude for that location. This ensure privacy so exact location is not stored.

### [Cognitive Services](https://azure.microsoft.com/en-us/services/cognitive-services/)
When adding smarts the application Cognitive Services enables the application to build in automatic login based on facial recognition.


### [Visual Studio Team Services](https://www.visualstudio.com/team-services/)

### [Visual Studio App Center](https://appcenter.ms)

## Libraries Used
* Xamarin.Forms
* Settings Plugin
* MVVM Helpers
* Messaging Plugin
* Geolocator Plugin

## License
Under MIT (see license file)

