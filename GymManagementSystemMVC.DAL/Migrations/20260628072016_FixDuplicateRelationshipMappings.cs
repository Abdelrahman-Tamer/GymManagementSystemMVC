using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymManagementSystemMVC.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixDuplicateRelationshipMappings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Members_MemberId1",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Session_SessionId1",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_MemberShips_Members_MemberId1",
                table: "MemberShips");

            migrationBuilder.DropForeignKey(
                name: "FK_Session_Trainers_TrainerId1",
                table: "Session");

            migrationBuilder.DropIndex(
                name: "IX_Session_TrainerId1",
                table: "Session");

            migrationBuilder.DropIndex(
                name: "IX_MemberShips_MemberId1",
                table: "MemberShips");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_MemberId1",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_SessionId1",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "TrainerId1",
                table: "Session");

            migrationBuilder.DropColumn(
                name: "MemberId1",
                table: "MemberShips");

            migrationBuilder.DropColumn(
                name: "MemberId1",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "SessionId1",
                table: "Bookings");

            migrationBuilder.AlterColumn<decimal>(
                name: "Weight",
                table: "HealthRecords",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Height",
                table: "HealthRecords",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TrainerId1",
                table: "Session",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MemberId1",
                table: "MemberShips",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Weight",
                table: "HealthRecords",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldPrecision: 5,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "Height",
                table: "HealthRecords",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldPrecision: 5,
                oldScale: 2);

            migrationBuilder.AddColumn<int>(
                name: "MemberId1",
                table: "Bookings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SessionId1",
                table: "Bookings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Session_TrainerId1",
                table: "Session",
                column: "TrainerId1");

            migrationBuilder.CreateIndex(
                name: "IX_MemberShips_MemberId1",
                table: "MemberShips",
                column: "MemberId1");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_MemberId1",
                table: "Bookings",
                column: "MemberId1");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_SessionId1",
                table: "Bookings",
                column: "SessionId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Members_MemberId1",
                table: "Bookings",
                column: "MemberId1",
                principalTable: "Members",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Session_SessionId1",
                table: "Bookings",
                column: "SessionId1",
                principalTable: "Session",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MemberShips_Members_MemberId1",
                table: "MemberShips",
                column: "MemberId1",
                principalTable: "Members",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Session_Trainers_TrainerId1",
                table: "Session",
                column: "TrainerId1",
                principalTable: "Trainers",
                principalColumn: "Id");
        }
    }
}
