using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RiskAnalyzer.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNewPropsOnDecisions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Decisions_AspNetUsers_DecidedByUserId",
                table: "Decisions");

            migrationBuilder.RenameColumn(
                name: "TotalRiskScore",
                table: "Decisions",
                newName: "CalculatedValue");

            migrationBuilder.AlterColumn<string>(
                name: "RecommendedAction",
                table: "Decisions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Decisions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DecidedByUserId",
                table: "Decisions",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "CriterionId",
                table: "Decisions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "Decisions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Decisions_CriterionId",
                table: "Decisions",
                column: "CriterionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Decisions_AspNetUsers_DecidedByUserId",
                table: "Decisions",
                column: "DecidedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Decisions_Criteria_CriterionId",
                table: "Decisions",
                column: "CriterionId",
                principalTable: "Criteria",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Decisions_AspNetUsers_DecidedByUserId",
                table: "Decisions");

            migrationBuilder.DropForeignKey(
                name: "FK_Decisions_Criteria_CriterionId",
                table: "Decisions");

            migrationBuilder.DropIndex(
                name: "IX_Decisions_CriterionId",
                table: "Decisions");

            migrationBuilder.DropColumn(
                name: "CriterionId",
                table: "Decisions");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "Decisions");

            migrationBuilder.RenameColumn(
                name: "CalculatedValue",
                table: "Decisions",
                newName: "TotalRiskScore");

            migrationBuilder.AlterColumn<string>(
                name: "RecommendedAction",
                table: "Decisions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Decisions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DecidedByUserId",
                table: "Decisions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Decisions_AspNetUsers_DecidedByUserId",
                table: "Decisions",
                column: "DecidedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
