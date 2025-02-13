using FluentMigrator;

namespace Golub.Migrations
{
    [Migration(202502101807)]
    public class InitMigration : Migration
    {
        public override void Up()
        {
            InitTables();
            InitForeignKeys();
        }

        public override void Down()
        {
            DeleteForeignKeys();
            DeleteTables();
        }

        private void InitTables()
        {
            Create.Table("ApiKeys")
                .WithColumn("Id").AsGuid().PrimaryKey().WithDefaultValue(Guid.CreateVersion7())
                .WithColumn("ApiKeyValue").AsGuid().NotNullable().Unique().WithDefaultValue(Guid.CreateVersion7())
                .WithColumn("ExpirationDate").AsDateTimeOffset().Nullable()
                .WithColumn("ApplicationName").AsString().Nullable()
                .WithColumn("CreatedOnUtc").AsDateTimeOffset().NotNullable()
                .WithColumn("ModifiedOnUtc").AsDateTimeOffset().Nullable();

            Create.Table("EmailProviders")
                .WithColumn("Id").AsGuid().PrimaryKey().WithDefaultValue(Guid.CreateVersion7())
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("IsActive").AsBoolean().NotNullable()
                .WithColumn("Configuration").AsString().Nullable()
                .WithColumn("LastUsedOn").AsDateTimeOffset().NotNullable()
                .WithColumn("FreePlanQty").AsInt32().Nullable()
                .WithColumn("RemainingQty").AsInt32().Nullable()
                .WithColumn("Period").AsInt32().Nullable()
                .WithColumn("CreatedOnUtc").AsDateTimeOffset().NotNullable()
                .WithColumn("ModifiedOnUtc").AsDateTimeOffset().Nullable();

            Create.Table("SentEmails")
                .WithColumn("Id").AsGuid().PrimaryKey().WithDefaultValue(Guid.CreateVersion7())
                .WithColumn("EmailProviderId").AsGuid().NotNullable()
                .WithColumn("From").AsString().NotNullable()
                .WithColumn("To").AsString().NotNullable()
                .WithColumn("Subject").AsString().NotNullable()
                .WithColumn("IsSuccesful").AsBoolean().NotNullable()
                .WithColumn("Remark").AsString().NotNullable()
                .WithColumn("CreatedOnUtc").AsDateTimeOffset().NotNullable()
                .WithColumn("ModifiedOnUtc").AsDateTimeOffset().Nullable();
        }

        private void InitForeignKeys()
        {
            Create.ForeignKey("FK_SentEmails_EmailProviders")
                .FromTable("SentEmails").ForeignColumn("EmailProviderId")
                .ToTable("EmailProviders").PrimaryColumn("Id");
        }

        private void DeleteTables()
        {
            Delete.Table("ApiKeys");
            Delete.Table("EmailProviders");
            Delete.Table("SentEmails");
        }

        private void DeleteForeignKeys()
        {
            Delete.ForeignKey("FK_SentEmails_EmailProviders").OnTable("SentEmails");
        }
    }
}
