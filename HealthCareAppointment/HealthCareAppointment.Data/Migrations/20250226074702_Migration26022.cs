using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HealthCareAppointment.Data.Migrations
{
    /// <inheritdoc />
    public partial class Migration26022 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DateOfBirth", "Email", "Name", "Password", "Role", "Specialization" },
                values: new object[,]
                {
                    { new Guid("09fabf72-c8a7-4776-ad9e-69f99449ae77"), new DateTime(1977, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "quu@gmail.com", "Duong Ba Trinh", "User@123", 0, "None" },
                    { new Guid("362cfb46-a865-48c1-bc03-a01101a0075f"), new DateTime(1992, 11, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "qtc@gmail.com", "Hoang Anh", "User@123", 1, "Dr" },
                    { new Guid("46ab5b8a-8880-46e6-8b39-5f201e2a27e9"), new DateTime(1992, 11, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "quyquy909@gmail.com", "Hoang Quy", "User@123", 1, "Dr" },
                    { new Guid("71d6e368-04cc-4766-b621-9d9f1c9e8f6e"), new DateTime(1992, 11, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "qtc@gmail.com", "Quanh Thanh Cong", "User@123", 1, "Dentist" },
                    { new Guid("c93d1328-8109-48a9-9ecb-77402a64d933"), new DateTime(1999, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "quuq@gmail.com", "Duong Ba Thanh", "User@123", 0, "None" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("09fabf72-c8a7-4776-ad9e-69f99449ae77"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("362cfb46-a865-48c1-bc03-a01101a0075f"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("46ab5b8a-8880-46e6-8b39-5f201e2a27e9"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("71d6e368-04cc-4766-b621-9d9f1c9e8f6e"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("c93d1328-8109-48a9-9ecb-77402a64d933"));
        }
    }
}
