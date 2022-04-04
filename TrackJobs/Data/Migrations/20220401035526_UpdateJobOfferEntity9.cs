using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackJobs.Data.Migrations
{
    public partial class UpdateJobOfferEntity9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Cons",
                table: "JobOffers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "JobOffers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Perks",
                table: "JobOffers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pros",
                table: "JobOffers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cons",
                table: "JobOffers");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "JobOffers");

            migrationBuilder.DropColumn(
                name: "Perks",
                table: "JobOffers");

            migrationBuilder.DropColumn(
                name: "Pros",
                table: "JobOffers");
        }
    }
}
