using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastPay.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPayrollCycles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "payroll_cycles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false, defaultValue: "open"),
                    finalized_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payroll_cycles", x => x.id);
                    table.CheckConstraint("ck_payroll_cycles_date_range", "end_date = start_date + 13");
                    table.CheckConstraint("ck_payroll_cycles_status", "status IN ('open', 'finalized')");
                });

            migrationBuilder.CreateIndex(
                name: "IX_payroll_cycles_start_date_end_date",
                table: "payroll_cycles",
                columns: new[] { "start_date", "end_date" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "payroll_cycles");
        }
    }
}
