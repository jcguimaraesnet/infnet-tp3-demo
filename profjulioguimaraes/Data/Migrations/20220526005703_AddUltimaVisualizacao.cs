using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace profjulioguimaraes.Data.Migrations
{
    public partial class AddUltimaVisualizacao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UltimaVisualizacao",
                table: "Amigo",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UltimaVisualizacao",
                table: "Amigo");
        }
    }
}
