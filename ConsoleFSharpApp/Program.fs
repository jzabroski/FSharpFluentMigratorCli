// Learn more about F# at http://fsharp.org

open System
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Logging;
open FluentMigrator
open FluentMigrator.Runner


type DatabaseConfiguration(processorId : string, connectionString : string) =
     member val ProcessorId = processorId with get, set
     member val ConnectionString = connectionString with get, set

let ConfigureApp (databaseConfiguration : DatabaseConfiguration -> unit) =
    ignore


[<EntryPoint>]
let main argv =
    let dbConfig = DatabaseConfiguration("SqlServer2017", "Data Source=.;Initial Catalog=DATABASE_NAME;Integrated Security=True;")


    let loggingServices = new Action<ILoggingBuilder>(fun lb -> lb.AddDebug().AddFluentMigratorConsole() |> ignore)
    let runnerConfig = new Action<IMigrationRunnerBuilder>(fun builder ->
        builder
            .AddSqlServer()
            .WithGlobalConnectionString(dbConfig.ConnectionString)
            .ScanIn(typeof<DbMigrations.AddTable>.Assembly).For.Migrations() |> ignore)

    let serviceCollection = new ServiceCollection()
    serviceCollection
        .AddLogging(loggingServices)
        .AddFluentMigratorCore()
        .ConfigureRunner(
            runnerConfig
        )
        |> ignore

    let serviceProvider = serviceCollection.BuildServiceProvider()

    let runner = serviceProvider.GetRequiredService<IMigrationRunner>();

    runner.MigrateUp()
    printfn "We did it!!"
    0 // return an integer exit code
