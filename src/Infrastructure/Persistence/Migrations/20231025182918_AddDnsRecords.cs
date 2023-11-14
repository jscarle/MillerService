using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MillerDemo.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDnsRecords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DnsRecords",
                columns: table => new
                {
                    Name = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: false),
                    IpAddress = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DnsRecords", x => x.Name);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DnsRecords");
        }
    }
}
