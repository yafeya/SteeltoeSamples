@cloud
Feature: PostgreEFCore Connector
    In order to show you how to use Steeltoe for connecting to PostgreSql using EntityFramework Core
    You can run some PostgreSql using EntityFramework Core connection samples

    @netcoreapp2.1
    @win10-x64
    Scenario: PostgreEFCore Connector for .Net Core 2.1 (win10-x64)
        Given you have at least .NET Core SDK 2.1.300 installed
        And you have CloudFoundry service postgresql-10-odb installed
        When you run: cf create-service postgresql-10-odb standalone myPostgres -c '{"db_name":"postgresample", "db_username":"steeltoe", "owner_name":"Steeltoe Demo", "owner_email":"demo@steeltoe.io"}'
        And you wait until CloudFoundry service myPostgres is created
        And you run: dotnet restore
        And you run: dotnet publish -f netcoreapp2.1 -r win10-x64
        And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/netcoreapp2.1/win10-x64/publish
        And you wait until CloudFoundry app postgresefcore-connector is started
        When you get https://postgresefcore-connector.x.y.z/Home/PostgresData
        Then you should see "1: Test Data 1 - EF Core TestContext"
        And you should see "2: Test Data 2 - EF Core TestContext"

    @netcoreapp2.1
    @ubuntu.14.04-x64
    Scenario: PostgreEFCore Connector for .Net Core 2.1 (ubuntu.14.04-x64)
        Given you have at least .NET Core SDK 2.1.300 installed
        And you have CloudFoundry service postgresql-10-odb installed
        When you run: cf create-service postgresql-10-odb standalone myPostgres -c '{"db_name":"postgresample", "db_username":"steeltoe", "owner_name":"Steeltoe Demo", "owner_email":"demo@steeltoe.io"}'
        And you wait until CloudFoundry service myPostgres is created
        And you run: dotnet restore
        And you run: dotnet publish -f netcoreapp2.1 -r ubuntu.14.04-x64
        And you run in the background: cf push -f manifest.yml -p bin/Debug/netcoreapp2.1/ubuntu.14.04-x64/publish
        And you wait until CloudFoundry app postgresefcore-connector is started
        When you get https://postgresefcore-connector.x.y.z/Home/PostgresData
        Then you should see "1: Test Data 1 - EF Core TestContext"
        And you should see "2: Test Data 2 - EF Core TestContext"
