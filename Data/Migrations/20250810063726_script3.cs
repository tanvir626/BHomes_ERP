﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bhomes_ERP.Data.Migrations
{
    /// <inheritdoc />
    public partial class script3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BranchCode",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BranchCode",
                table: "AspNetUsers");
        }
    }
}
