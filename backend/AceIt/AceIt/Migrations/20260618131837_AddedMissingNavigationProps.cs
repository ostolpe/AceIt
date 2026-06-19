using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AceIt.Migrations
{
    /// <inheritdoc />
    public partial class AddedMissingNavigationProps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Sessions_UserId",
                table: "Sessions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AiResponses_QuestionId",
                table: "AiResponses",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AiResponses_Questions_QuestionId",
                table: "AiResponses",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_AspNetUsers_UserId",
                table: "Sessions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AiResponses_Questions_QuestionId",
                table: "AiResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_AspNetUsers_UserId",
                table: "Sessions");

            migrationBuilder.DropIndex(
                name: "IX_Sessions_UserId",
                table: "Sessions");

            migrationBuilder.DropIndex(
                name: "IX_AiResponses_QuestionId",
                table: "AiResponses");
        }
    }
}
