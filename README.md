# JamSys-InAppTune
JamSys InAppTune is a library to tune DBMS parameters using machine learning. Modern DBMS have hundreds of run-time parameters which control run-time behavior of the DBMS and affect its performance. Tuning these knobs manually is a tedious task. There are many tools to perform automatic tuning, and many of them uutilize machine learning. This project is accompanying my master thesis and has not been tested in production environments.


## Setup
InAppTune is tested on PostgreSQL version 12.3 and MySQL version 5.7. The docker files are included in source code. The host application provided here aims to show the demo functionality of InAppTune. It uses TPC-C data model. The database create scripts are included in source code. After setting up the database docker containers, use the scripts to create database structure. The connection string can be configured in `appsettings.json` file. Uncomment the required database provider in the settings file. After starting the software, use the data generator page to generate TPC-C data. This can take a very long time. 

## Tuning
Enter the tuning page, choose the tab `Automatic` and click on start button. The tuning happens automatically.  

# Credits
1. Blazorise: https://github.com/Megabit/Blazorise
2. TorchSharp: https://github.com/dotnet/TorchSharp
3. OtterTune: https://ottertune.com/
