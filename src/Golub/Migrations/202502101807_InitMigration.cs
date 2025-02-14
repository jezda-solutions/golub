using FluentMigrator;

namespace Golub.Migrations
{
    /// <summary>
    /// Initial migration for database
    /// </summary>
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
            Create.Table("api_key")
                .WithColumn("id").AsGuid().PrimaryKey().WithDefaultValue(Guid.CreateVersion7())
                .WithColumn("api_key_value").AsGuid().NotNullable().Unique().WithDefaultValue(Guid.CreateVersion7())
                .WithColumn("expiration_date").AsDateTimeOffset().Nullable()
                .WithColumn("application_name").AsString().Nullable()
                .WithColumn("created_on_utc").AsDateTimeOffset().NotNullable()
                .WithColumn("modified_on_utc").AsDateTimeOffset().Nullable();

            Create.Table("email_provider")
                .WithColumn("id").AsGuid().PrimaryKey().WithDefaultValue(Guid.CreateVersion7())
                .WithColumn("name").AsString().NotNullable()
                .WithColumn("is_active").AsBoolean().NotNullable()
                .WithColumn("configuration").AsString().Nullable()
                .WithColumn("last_used_on").AsDateTimeOffset().NotNullable()
                .WithColumn("free_plan_qty").AsInt32().Nullable()
                .WithColumn("remaining_qty").AsInt32().Nullable()
                .WithColumn("period").AsInt32().Nullable()
                .WithColumn("created_on_utc").AsDateTimeOffset().NotNullable()
                .WithColumn("modified_on_utc").AsDateTimeOffset().Nullable();

            Create.Table("sent_email")
                .WithColumn("id").AsGuid().PrimaryKey().WithDefaultValue(Guid.CreateVersion7())
                .WithColumn("email_provider_id").AsGuid().NotNullable()
                .WithColumn("from").AsString().NotNullable()
                .WithColumn("to").AsString().NotNullable()
                .WithColumn("subject").AsString().NotNullable()
                .WithColumn("is_successful").AsBoolean().NotNullable()
                .WithColumn("remark").AsString().NotNullable()
                .WithColumn("created_on_utc").AsDateTimeOffset().NotNullable()
                .WithColumn("modified_on_utc").AsDateTimeOffset().Nullable();
        }

        private void InitForeignKeys()
        {
            Create.ForeignKey("FK_sent_email_email_provider")
                .FromTable("sent_email").ForeignColumn("email_provider_id")
                .ToTable("email_provider").PrimaryColumn("id");
        }

        private void DeleteTables()
        {
            Delete.Table("api_key");
            Delete.Table("email_provider");
            Delete.Table("sent_email");
        }

        private void DeleteForeignKeys()
        {
            Delete.ForeignKey("FK_sent_email_email_provider").OnTable("sent_email");
        }
    }
}
