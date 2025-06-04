using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CertificateManager.Server.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCertificateModelPublicKeyOnly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PfxFilePath",
                table: "Certificates");

            migrationBuilder.DropColumn(
                name: "PfxPasswordHash",
                table: "Certificates");

            migrationBuilder.AddColumn<string>(
                name: "CertificateData",
                table: "Certificates",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CertificateData",
                table: "Certificates");

            migrationBuilder.AddColumn<string>(
                name: "PfxFilePath",
                table: "Certificates",
                type: "TEXT",
                maxLength: 1024,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PfxPasswordHash",
                table: "Certificates",
                type: "TEXT",
                nullable: true);
        }
    }
}
