using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Databases.Identity.Migrations
{
    public partial class AddUniversalUserProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UniversalUserProperties",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    AcceptNewsletters = table.Column<bool>(nullable: false),
                    SchoolName = table.Column<string>(nullable: true),
                    SchoolCity = table.Column<string>(nullable: true),
                    ProfileType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UniversalUserProperties", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_UniversalUserProperties_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.Sql(@"EXEC(N'
                INSERT INTO [UniversalUserProperties] 
                (
                    [UserId], 
                    [Name], 
                    [AcceptNewsletters], 
                    [ProfileType], 
                    [SchoolCity],
                    [SchoolName]
                )
                SELECT [Id] as UserId,
                    [Name],
                    [AcceptNewsletters],
                    [ProfileType],
                    [SchoolCity],
                    [SchoolName]
              FROM [AspNetUsers]
              WHERE [Name] IS NOT NULL')");

            migrationBuilder.DropColumn(
                name: "AcceptNewsletters",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProfileType",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SchoolCity",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SchoolName",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<bool>(
                name: "AcceptNewsletters",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileType",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SchoolCity",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SchoolName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.Sql(@"
                UPDATE [dbo].[AspNetUsers]
                SET
                  Name = uup.Name,
                  AcceptNewsletters = uup.AcceptNewsletters,
                  SchoolCity = uup.SchoolCity,
                  SchoolName = uup.SchoolName,
                  ProfileType = uup.ProfileType
                FROM [dbo].[AspNetUsers] u 
                INNER JOIN [dbo].[UniversalUserProperties] uup
                ON u.Id = uup.UserId"
            );

            migrationBuilder.DropTable(
                name: "UniversalUserProperties");
        }
    }
}
