using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFGetStarted.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Task",
                columns: table => new
                {
                    TaskId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    TeamId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Task", x => x.TaskId);
                });

            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    TeamId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    CurrentTaskId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.TeamId);
                    table.ForeignKey(
                        name: "FK_Team_Task_CurrentTaskId",
                        column: x => x.CurrentTaskId,
                        principalTable: "Task",
                        principalColumn: "TaskId");
                });

            migrationBuilder.CreateTable(
                name: "TeamWorkers",
                columns: table => new
                {
                    TeamsTeamId = table.Column<int>(type: "INTEGER", nullable: false),
                    WorkersWorkerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamWorkers", x => new { x.TeamsTeamId, x.WorkersWorkerId });
                    table.ForeignKey(
                        name: "FK_TeamWorkers_Team_TeamsTeamId",
                        column: x => x.TeamsTeamId,
                        principalTable: "Team",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Todo",
                columns: table => new
                {
                    TodoId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    IsComplete = table.Column<bool>(type: "INTEGER", nullable: true),
                    TaskId = table.Column<int>(type: "INTEGER", nullable: false),
                    WorkerId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Todo", x => x.TodoId);
                    table.ForeignKey(
                        name: "FK_Todo_Task_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Task",
                        principalColumn: "TaskId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Worker",
                columns: table => new
                {
                    WorkerId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    CurrentTodoTodoId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Worker", x => x.WorkerId);
                    table.ForeignKey(
                        name: "FK_Worker_Todo_CurrentTodoTodoId",
                        column: x => x.CurrentTodoTodoId,
                        principalTable: "Todo",
                        principalColumn: "TodoId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Task_TeamId",
                table: "Task",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Team_CurrentTaskId",
                table: "Team",
                column: "CurrentTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamWorkers_WorkersWorkerId",
                table: "TeamWorkers",
                column: "WorkersWorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_Todo_TaskId",
                table: "Todo",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Todo_WorkerId",
                table: "Todo",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_Worker_CurrentTodoTodoId",
                table: "Worker",
                column: "CurrentTodoTodoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Task_Team_TeamId",
                table: "Task",
                column: "TeamId",
                principalTable: "Team",
                principalColumn: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeamWorkers_Worker_WorkersWorkerId",
                table: "TeamWorkers",
                column: "WorkersWorkerId",
                principalTable: "Worker",
                principalColumn: "WorkerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Todo_Worker_WorkerId",
                table: "Todo",
                column: "WorkerId",
                principalTable: "Worker",
                principalColumn: "WorkerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Task_Team_TeamId",
                table: "Task");

            migrationBuilder.DropForeignKey(
                name: "FK_Todo_Task_TaskId",
                table: "Todo");

            migrationBuilder.DropForeignKey(
                name: "FK_Todo_Worker_WorkerId",
                table: "Todo");

            migrationBuilder.DropTable(
                name: "TeamWorkers");

            migrationBuilder.DropTable(
                name: "Team");

            migrationBuilder.DropTable(
                name: "Task");

            migrationBuilder.DropTable(
                name: "Worker");

            migrationBuilder.DropTable(
                name: "Todo");
        }
    }
}
