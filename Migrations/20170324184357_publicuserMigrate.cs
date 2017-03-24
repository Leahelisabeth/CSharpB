using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BeltS.Migrations
{
    public partial class publicuserMigrate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Connection_User_UserId",
                table: "Connection");

            migrationBuilder.DropIndex(
                name: "IX_Connection_UserId",
                table: "Connection");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Connection");

            migrationBuilder.CreateIndex(
                name: "IX_Connection_CurUserId",
                table: "Connection",
                column: "CurUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Connection_NetUserId",
                table: "Connection",
                column: "NetUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Connection_User_CurUserId",
                table: "Connection",
                column: "CurUserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Connection_User_NetUserId",
                table: "Connection",
                column: "NetUserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Connection_User_CurUserId",
                table: "Connection");

            migrationBuilder.DropForeignKey(
                name: "FK_Connection_User_NetUserId",
                table: "Connection");

            migrationBuilder.DropIndex(
                name: "IX_Connection_CurUserId",
                table: "Connection");

            migrationBuilder.DropIndex(
                name: "IX_Connection_NetUserId",
                table: "Connection");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Connection",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Connection_UserId",
                table: "Connection",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Connection_User_UserId",
                table: "Connection",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
