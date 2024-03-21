using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Common.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshTokensMod4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                 table: "ApplicationUserRoles",
                 columns: new[] { "Id", "Name" },
                 values: new object[,]
                 {
                     {1, "Cli" },
                     {2, "Admin" }
                 });

            migrationBuilder.InsertData(
                table: "ApplicationUsers",
                columns: new[] { "Id", "Name", "PasswordHash" },
                values: new object[,]
                {
                    {1, "Alient","w1pCBmHfcbNZvFpc8zDClJzU1czUL35dCUandvKK6DWkymDR/q7InfflYqKwsJ9VHHPn1uGLFupXUBw402PZOQ==8E0F9E45A6ABC7732170D65565B212C5D3EBD5F0380A14DE946CCB87E7A51E8BEEDCF582AD2560F8B2C355130975D543AFC0A8A0B89C20B7577E76D7A77DE993" }
                });
            migrationBuilder.InsertData(
                table: "ApplicationUserApplicationRole",
                columns: new[] { "ApplicationUserId", "ApplicationUserRoleId" },
                values: new object[,]
                {
                    {1, 1}
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
