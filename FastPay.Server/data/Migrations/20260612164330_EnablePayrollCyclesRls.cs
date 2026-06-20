using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastPay.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class EnablePayrollCyclesRls : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE payroll_cycles ENABLE ROW LEVEL SECURITY;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE payroll_cycles DISABLE ROW LEVEL SECURITY;");
        }
    }
}
