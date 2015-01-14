# Loan Monitor Plugin
The e360 Tray Application is a companion tool for the Encompass360 software. 
This is an 'alpha' release and, as such, the features will change over time. 
See below for some high-level information about the initial release.

## Dynamic Event Handling
At the core of the plugin is support for handling events. 
The events being handled provide, nearly, limitless possibilities for extensibility. 
All Design Patterns previously identified in the 'Ditech Prototype Design Patterns' form have now been replicated externally. 
This means we have a single code-base for deploying between environments.
* Interface Triggers
* Extending Button Clicks

## Permission Based Menu
The main UI of the application is built with the idea of creating menus dynamically based on the users persona. 
This allows the user experience to be customized for each role within the application. 
Below are some of the scenarios which are permission based:
* Support Related Items
* Administration Tools
* Role-related Forms

## Non-Loan Level Forms
One of the biggest limitations within the Encompass360 application has been the lack of support for forms outside of a loan. 
This limitation has now been lifted, as we can provide simple access to the forms. 
Below are some scenarios for non-loan level forms:
* Servicing Information Screen
* Bulk Funding
* Commission Statements

## Administrative Functions
The tools provided under the Admin menu will allow for a consolidated location for miscellaneous maintenance tools. 
These tools will provide additional functionality outside the realm of the Enconpass360 configuration.
* CLI Monitored Fields
* User Administration
* Property Configuration Manager
