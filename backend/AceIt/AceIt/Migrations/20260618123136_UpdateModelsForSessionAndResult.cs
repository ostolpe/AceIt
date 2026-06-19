using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AceIt.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModelsForSessionAndResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Sessions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "QuestionId",
                table: "AiResponses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AiResponses_SessionId",
                table: "AiResponses",
                column: "SessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AiResponses_Sessions_SessionId",
                table: "AiResponses",
                column: "SessionId",
                principalTable: "Sessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AiResponses_Sessions_SessionId",
                table: "AiResponses");

            migrationBuilder.DropIndex(
                name: "IX_AiResponses_SessionId",
                table: "AiResponses");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "AiResponses");
        }
    }
}
