# MongoDBDemo

## Setup MongoDB

### MongoDB on MAC med HomeBrew

* Guide fra: https://www.mongodb.com/docs/manual/tutorial/install-mongodb-on-os-x/

```
# Hent Tap til MongoDB relatede installationer 
brew tap mongodb/brew

# Opdater homebrew
brew update

# Installer mongoDb serveren
brew install mongodb-community@8.0

# Start server
brew services start mongodb-community@8.0

# Kontroller den kører
brew services list

# Stop server
brew services stop mongodb-community@8.0
```

Se db med Azure Data Studio:
* Installer exstension til MongoDB
* Connect med default: mongodb://localhost:27017

### MongoDB on Windows ikke som windows service

* Guide fra: https://www.mongodb.com/docs/manual/tutorial/install-mongodb-on-windows/

```
# Hent Mongosh 

# Hent MongoDB vælg ikke at opsætte som service

# Åben Powershell 

# Lav database dir 
cd C:\
md "\data\db"

# kør mongod.exe

```

## Add MongoDb driver to .Net project

Guide fra: https://www.mongodb.com/docs/drivers/csharp/current/

```
# Tilføj package
dotnet add package MongoDB.Driver
```
