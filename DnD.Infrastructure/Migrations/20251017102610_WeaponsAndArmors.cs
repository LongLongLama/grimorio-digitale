using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DnD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class WeaponsAndArmors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ArmorClass",
                table: "Items",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Damage",
                table: "Items",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DamageType",
                table: "Items",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EquippedArmorId",
                table: "Characters",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PrimaryWeaponId",
                table: "Characters",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SecondaryWeaponId",
                table: "Characters",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Characters_EquippedArmorId",
                table: "Characters",
                column: "EquippedArmorId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_PrimaryWeaponId",
                table: "Characters",
                column: "PrimaryWeaponId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_SecondaryWeaponId",
                table: "Characters",
                column: "SecondaryWeaponId");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Items_EquippedArmorId",
                table: "Characters",
                column: "EquippedArmorId",
                principalTable: "Items",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Items_PrimaryWeaponId",
                table: "Characters",
                column: "PrimaryWeaponId",
                principalTable: "Items",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Items_SecondaryWeaponId",
                table: "Characters",
                column: "SecondaryWeaponId",
                principalTable: "Items",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Items_EquippedArmorId",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Items_PrimaryWeaponId",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Items_SecondaryWeaponId",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Characters_EquippedArmorId",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Characters_PrimaryWeaponId",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Characters_SecondaryWeaponId",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "ArmorClass",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Damage",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "DamageType",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "EquippedArmorId",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "PrimaryWeaponId",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "SecondaryWeaponId",
                table: "Characters");
        }
    }
}
