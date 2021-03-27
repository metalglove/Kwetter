using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Kwetter.Services.KweetService.Infrastructure.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kweets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(140)", maxLength: 140, nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kweets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KweetLike",
                columns: table => new
                {
                    KweetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LikedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KweetAggregateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KweetLike", x => new { x.KweetId, x.UserId });
                    table.ForeignKey(
                        name: "FK_KweetLike_Kweets_KweetAggregateId",
                        column: x => x.KweetAggregateId,
                        principalTable: "Kweets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KweetLike_KweetAggregateId",
                table: "KweetLike",
                column: "KweetAggregateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KweetLike");

            migrationBuilder.DropTable(
                name: "Kweets");
        }
    }
}
