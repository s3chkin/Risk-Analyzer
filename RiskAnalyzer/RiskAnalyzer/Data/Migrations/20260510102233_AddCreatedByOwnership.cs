using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RiskAnalyzer.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedByOwnership : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "Scenarios",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "RiskTypes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "Criteria",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Scenarios_CreatedByUserId",
                table: "Scenarios",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskTypes_CreatedByUserId",
                table: "RiskTypes",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Criteria_CreatedByUserId",
                table: "Criteria",
                column: "CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Criteria_AspNetUsers_CreatedByUserId",
                table: "Criteria",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskTypes_AspNetUsers_CreatedByUserId",
                table: "RiskTypes",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Scenarios_AspNetUsers_CreatedByUserId",
                table: "Scenarios",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Criteria_AspNetUsers_CreatedByUserId",
                table: "Criteria");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskTypes_AspNetUsers_CreatedByUserId",
                table: "RiskTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_Scenarios_AspNetUsers_CreatedByUserId",
                table: "Scenarios");

            migrationBuilder.DropIndex(
                name: "IX_Scenarios_CreatedByUserId",
                table: "Scenarios");

            migrationBuilder.DropIndex(
                name: "IX_RiskTypes_CreatedByUserId",
                table: "RiskTypes");

            migrationBuilder.DropIndex(
                name: "IX_Criteria_CreatedByUserId",
                table: "Criteria");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Scenarios");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "RiskTypes");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Criteria");
        }
    }
}
