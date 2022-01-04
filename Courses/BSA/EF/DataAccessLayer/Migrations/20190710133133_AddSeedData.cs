using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class AddSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TaskStates",
                columns: new[] { "Id", "Value" },
                values: new object[,]
                {
                    { 1, "Created" },
                    { 2, "Started" },
                    { 3, "Finished" },
                    { 4, "Canceled" }
                });

            migrationBuilder.InsertData(
                table: "Teams",
                columns: new[] { "Id", "CreatedAt", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2019, 2, 10, 16, 31, 32, 518, DateTimeKind.Local).AddTicks(179), "Team 1" },
                    { 2, new DateTime(2019, 2, 10, 16, 31, 32, 518, DateTimeKind.Local).AddTicks(1100), "Team 2" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Birthday", "Email", "FirstName", "LastName", "RegisteredAt", "TeamId" },
                values: new object[] { 1, new DateTime(2019, 2, 10, 16, 31, 32, 515, DateTimeKind.Local).AddTicks(5223), "john.doe@mail.com", "John", "Doe", new DateTime(2019, 7, 20, 16, 31, 32, 516, DateTimeKind.Local).AddTicks(9237), 1 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Birthday", "Email", "FirstName", "LastName", "RegisteredAt", "TeamId" },
                values: new object[] { 2, new DateTime(2019, 1, 10, 16, 31, 32, 517, DateTimeKind.Local).AddTicks(216), "jane.doe@mail.com", "Jane", "Doe", new DateTime(2019, 7, 25, 16, 31, 32, 517, DateTimeKind.Local).AddTicks(229), 2 });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "AuthorId", "CreatedAt", "Deadline", "Description", "Name", "TeamId" },
                values: new object[] { 1, 1, new DateTime(2019, 2, 10, 16, 31, 32, 518, DateTimeKind.Local).AddTicks(4102), new DateTime(2019, 12, 10, 16, 31, 32, 518, DateTimeKind.Local).AddTicks(4540), "Project 1", "Project 1", 1 });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "CreatedAt", "Description", "FinishedAt", "Name", "PerformerId", "ProjectId", "TaskStateId" },
                values: new object[] { 1, new DateTime(2019, 2, 10, 16, 31, 32, 517, DateTimeKind.Local).AddTicks(5194), "Task 1", new DateTime(2019, 12, 10, 16, 31, 32, 517, DateTimeKind.Local).AddTicks(5634), "Task 1", 1, 1, 1 });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "CreatedAt", "Description", "FinishedAt", "Name", "PerformerId", "ProjectId", "TaskStateId" },
                values: new object[] { 2, new DateTime(2019, 2, 10, 16, 31, 32, 517, DateTimeKind.Local).AddTicks(7147), "Task 2", new DateTime(2019, 12, 10, 16, 31, 32, 517, DateTimeKind.Local).AddTicks(7159), "Task 2", 2, 1, 2 });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "CreatedAt", "Description", "FinishedAt", "Name", "PerformerId", "ProjectId", "TaskStateId" },
                values: new object[] { 3, new DateTime(2019, 2, 10, 16, 31, 32, 517, DateTimeKind.Local).AddTicks(7176), "Task 3", new DateTime(2019, 12, 10, 16, 31, 32, 517, DateTimeKind.Local).AddTicks(7179), "Task 3", 1, 1, 3 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TaskStates",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TaskStates",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TaskStates",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TaskStates",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Teams",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Teams",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
