namespace DbMigrations

open FluentMigrator
open System
    
[<Migration(1L)>]
type AddTable() = 
    inherit Migration()

    override this.Up() = 
        Console.WriteLine(System.AppContext.GetData("APP_CONTEXT_DEPS_FILES"))
        this.Create.Table("Table")
            .WithColumn("Column").AsString(10, "Latin1_General_CI_AS").NotNullable().Indexed().PrimaryKey() |> ignore
        ()

    override this.Down() =
        this.Delete.Table("Table") |> ignore
        ()
