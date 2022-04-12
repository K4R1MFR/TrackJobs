using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackJobs.Migrations
{
    public partial class AddWFHtoJobOffer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsWFHAvailable",
                table: "JobOffers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsWFHAvailable",
                table: "JobOffers");
        }
    }
}
