This project was developed by Claudio Leone and Vittorio Triolo.

The 'AEEG' project deals with the management of electric and gas energy produced and how the regions manage them in terms of production and consumption.
-   We have created a list of regions that contains a list of structures that can be active or not so as to be able to change the production of a given energy (electricity or gas).
-   We have created a dictionary containing the name of the region as the key and the list of contracts as the value (for both the donor and recipient regions).
-   We have created a client (user interaction menu) where you can print all the data relating to the regions (self-sufficient, deficient and parsimonious).
____________________________________________________________________________________________________________________________________________________________________________

By logging in with the name of a specific region we will be able to:

1) Go and stipulate a contract with another region and it will be verified whether the latter will be able to allow the transfer of the energy requested by the first region
(the data is written on a json files).

2) Print the list of your stipulated contracts (if any).

3) Print your status (quantities produced/consumed, etc...).

4) Add one structure at a time in order to boost production or shut down a specific structure to reduce energy production.
____________________________________________________________________________________________________________________________________________________________________________

The solution is divided into 5 projects:

1) The project 'AEEG', where the 'main' or the starting point of the application is present.

2) The project containing the client, therefore the actual application in which you can navigate to generate and write new data or recover those already saved.

3) The object project where the central classes and methods for our application are present (Region class, Contract class, etc...).

4) The json project within which there are the data files in json format and the class with the methods for managing the serialization and deserialization of the files.

5) The Unit Test project.
