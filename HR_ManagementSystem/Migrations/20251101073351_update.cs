using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HR_ManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ConractDate",
                table: "Employees",
                newName: "ContractDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ContractDate",
                table: "Employees",
                newName: "ConractDate");
        }
    }
}
