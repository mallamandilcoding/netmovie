using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace netflix_clone.Migrations
{
    /// <inheritdoc />
    public partial class watchlistmovieschanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WatchlistMovies_Watchlists_WatchlistId",
                table: "WatchlistMovies");

            migrationBuilder.RenameColumn(
                name: "WatchlistId",
                table: "WatchlistMovies",
                newName: "WatchListId");

            migrationBuilder.RenameIndex(
                name: "IX_WatchlistMovies_WatchlistId",
                table: "WatchlistMovies",
                newName: "IX_WatchlistMovies_WatchListId");

            migrationBuilder.AlterColumn<int>(
                name: "WatchListId",
                table: "WatchlistMovies",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "WatchlistMovies",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_WatchlistMovies_UserId",
                table: "WatchlistMovies",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WatchlistMovies_Users_UserId",
                table: "WatchlistMovies",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WatchlistMovies_Watchlists_WatchListId",
                table: "WatchlistMovies",
                column: "WatchListId",
                principalTable: "Watchlists",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WatchlistMovies_Users_UserId",
                table: "WatchlistMovies");

            migrationBuilder.DropForeignKey(
                name: "FK_WatchlistMovies_Watchlists_WatchListId",
                table: "WatchlistMovies");

            migrationBuilder.DropIndex(
                name: "IX_WatchlistMovies_UserId",
                table: "WatchlistMovies");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "WatchlistMovies");

            migrationBuilder.RenameColumn(
                name: "WatchListId",
                table: "WatchlistMovies",
                newName: "WatchlistId");

            migrationBuilder.RenameIndex(
                name: "IX_WatchlistMovies_WatchListId",
                table: "WatchlistMovies",
                newName: "IX_WatchlistMovies_WatchlistId");

            migrationBuilder.AlterColumn<int>(
                name: "WatchlistId",
                table: "WatchlistMovies",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WatchlistMovies_Watchlists_WatchlistId",
                table: "WatchlistMovies",
                column: "WatchlistId",
                principalTable: "Watchlists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
