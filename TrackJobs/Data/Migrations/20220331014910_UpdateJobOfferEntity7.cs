using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackJobs.Data.Migrations
{
    public partial class UpdateJobOfferEntity7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "JobOffers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "JobOffers");
        }
    }
}
