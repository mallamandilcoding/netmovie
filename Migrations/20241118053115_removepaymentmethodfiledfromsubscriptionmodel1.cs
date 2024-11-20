using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace netflix_clone.Migrations
{
    /// <inheritdoc />
    public partial class removepaymentmethodfiledfromsubscriptionmodel1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "Subscriptions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "Subscriptions",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
