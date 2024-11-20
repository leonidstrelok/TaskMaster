using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskMasterAPI.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Add_Table_Company_And_New_Relations_For_Task : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NameCompany = table.Column<string>(type: "text", nullable: false),
                    DescriptionCompany = table.Column<string>(type: "text", nullable: true),
                    CompanyIndustry = table.Column<int>(type: "integer", nullable: false),
                    QuantityEmployees = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                });
            
            migrationBuilder.DropForeignKey(
                name: "FK_Task_AspNetUsers_ClientId",
                table: "Task");

            migrationBuilder.AlterColumn<string>(
                name: "ClientId",
                table: "Task",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "Task",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                table: "Task",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "TesterId",
                table: "Task",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                table: "AspNetUsers",
                type: "uuid",
                nullable: true,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Task_AuthorId",
                table: "Task",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Task_CompanyId",
                table: "Task",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Task_TesterId",
                table: "Task",
                column: "TesterId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CompanyId",
                table: "AspNetUsers",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Company_CompanyId",
                table: "AspNetUsers",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Task_AspNetUsers_AuthorId",
                table: "Task",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Task_AspNetUsers_ClientId",
                table: "Task",
                column: "ClientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Task_AspNetUsers_TesterId",
                table: "Task",
                column: "TesterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Task_Company_CompanyId",
                table: "Task",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Company_CompanyId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Task_AspNetUsers_AuthorId",
                table: "Task");

            migrationBuilder.DropForeignKey(
                name: "FK_Task_AspNetUsers_ClientId",
                table: "Task");

            migrationBuilder.DropForeignKey(
                name: "FK_Task_AspNetUsers_TesterId",
                table: "Task");

            migrationBuilder.DropForeignKey(
                name: "FK_Task_Company_CompanyId",
                table: "Task");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropIndex(
                name: "IX_Task_AuthorId",
                table: "Task");

            migrationBuilder.DropIndex(
                name: "IX_Task_CompanyId",
                table: "Task");

            migrationBuilder.DropIndex(
                name: "IX_Task_TesterId",
                table: "Task");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CompanyId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Task");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Task");

            migrationBuilder.DropColumn(
                name: "TesterId",
                table: "Task");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "ClientId",
                table: "Task",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Task_AspNetUsers_ClientId",
                table: "Task",
                column: "ClientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
