using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MT.OnlineRestuarant.DataLayer.Migrations
{
    public partial class first : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LoggingInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true, defaultValueSql: "('')"),
                    ControllerName = table.Column<string>(maxLength: 250, nullable: true, defaultValueSql: "('')"),
                    ActionName = table.Column<string>(maxLength: 250, nullable: true, defaultValueSql: "('')"),
                    RecordTimeStamp = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "('')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoggingInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblCustomer",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(maxLength: 225, nullable: false, defaultValueSql: "('')"),
                    LastName = table.Column<string>(maxLength: 225, nullable: false, defaultValueSql: "('')"),
                    Email = table.Column<string>(maxLength: 225, nullable: false, defaultValueSql: "('')"),
                    MobileNumber = table.Column<string>(maxLength: 20, nullable: false, defaultValueSql: "('')"),
                    Password = table.Column<string>(maxLength: 225, nullable: false, defaultValueSql: "('')"),
                    PasswordKey = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: false, defaultValueSql: "('')"),
                    Active = table.Column<bool>(nullable: false),
                    UserCreated = table.Column<int>(nullable: false, defaultValueSql: "((0))"),
                    UserModified = table.Column<int>(nullable: false, defaultValueSql: "((0))"),
                    RecordTimeStamp = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "((0))"),
                    RecordTimeStampCreated = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCustomer", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoggingInfo");

            migrationBuilder.DropTable(
                name: "tblCustomer");
        }
    }
}
