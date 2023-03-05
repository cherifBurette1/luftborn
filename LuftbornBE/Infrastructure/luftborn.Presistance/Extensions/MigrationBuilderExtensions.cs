using Microsoft.EntityFrameworkCore.Migrations;

namespace luftborn.Presistance.Extensions
{
    public static class MigrationBuilderExtensions
    {
        public static MigrationBuilder DropTableIfExists(this MigrationBuilder migrationBuilder, string name)
        {
            migrationBuilder.Sql(string.Format(
                @"IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{0}'))
                   BEGIN
                        EXEC(N'DROP TABLE {0}')
                   END", name));

            return migrationBuilder;
        }
    }
}
