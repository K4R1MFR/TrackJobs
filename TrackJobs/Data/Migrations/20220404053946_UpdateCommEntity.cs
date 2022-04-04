using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackJobs.Data.Migrations
{
    public partial class UpdateCommEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSoftDeleted",
                table: "Communications",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSoftDeleted",
                table: "Communications");
        }
    }
}
