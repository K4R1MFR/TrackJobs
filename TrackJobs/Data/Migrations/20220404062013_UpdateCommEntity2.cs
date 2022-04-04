using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackJobs.Data.Migrations
{
    public partial class UpdateCommEntity2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSoftDeleted",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "IsSoftDeleted",
                table: "Communications");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSoftDeleted",
                table: "Contacts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSoftDeleted",
                table: "Communications",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
