using FluentMigrator;
using FluentMigrator.Postgres;

namespace Mail.Infrastructure.Migrations;

[Migration(20231107)]
public class Tables : Migration
{
    public override void Up()
    {
        Create
            .Table("UserEntity")
            .WithColumn("Id").AsGuid().NotNullable().PrimaryKey().WithDefaultValue(SystemMethods.NewGuid)
            .WithColumn("MailAddress").AsString(255).NotNullable();
        
        Create
            .Table("LetterEntity")
            .WithColumn("Id").AsGuid().NotNullable().PrimaryKey().WithDefaultValue(SystemMethods.NewGuid)
            .WithColumn("Title").AsString(255).NotNullable()
            .WithColumn("Content").AsString(255).NotNullable()
            .WithColumn("CreatedAt").AsDateTimeOffset().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
            .WithColumn("ReceiverId").AsGuid().NotNullable()
            .WithColumn("OwnerId").AsGuid().NotNullable();
        
        Create
            .ForeignKey("fk_LetterEntity_OwnerId_UserEntity_ID")
            .FromTable("LetterEntity").ForeignColumn("OwnerId")
            .ToTable("UserEntity").PrimaryColumn("Id");
        
        Create
            .ForeignKey("fk_LetterEntity_ReceiverId_UserEntity_ID")
            .FromTable("LetterEntity").ForeignColumn("ReceiverId")
            .ToTable("UserEntity").PrimaryColumn("Id");
    }

    public override void Down()
    {
        Delete.Table("UserEntity");
        Delete.Table("LetterEntity");
    }
}