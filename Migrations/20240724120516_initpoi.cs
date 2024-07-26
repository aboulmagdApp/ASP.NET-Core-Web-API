using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace cityInfo.Api.Migrations
{
    /// <inheritdoc />
    public partial class initpoi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("5a89ba5d-76c9-45f2-84bb-a8a2dad0d484"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("ca203af8-7127-433e-ae28-9c255a04ed84"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("ffe7e114-c1d3-41e3-b294-f49e2ad00f0b"));

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("06450b09-a0e9-4732-b056-b36a148891dc"), "The one with the cathedral that was never really finished.", "Antwerp" },
                    { new Guid("ad15bd1a-d7c9-48b2-ae2b-027464482030"), "The one with that big park.", "New York City" },
                    { new Guid("b5a53d85-5c02-43a3-aa04-9ca7414db146"), "The one with that big tower.", "Paris" }
                });

            migrationBuilder.InsertData(
                table: "PointsOfInterest",
                columns: new[] { "Id", "CityId", "Description", "Name" },
                values: new object[,]
                {
                    { 1, new Guid("5a89ba5d-76c9-45f2-84bb-a8a2dad0d484"), "The most visited urban park in the United States.", "Central Park" },
                    { 2, new Guid("5a89ba5d-76c9-45f2-84bb-a8a2dad0d484"), "A 102-story skyscraper located in Midtown Manhattan.", "Empire State Building" },
                    { 3, new Guid("ca203af8-7127-433e-ae28-9c255a04ed84"), "A Gothic style cathedral, conceived by architects Jan and Pieter Appelmans.", "Cathedral" },
                    { 4, new Guid("ca203af8-7127-433e-ae28-9c255a04ed84"), "The the finest example of railway architecture in Belgium.", "Antwerp Central Station" },
                    { 5, new Guid("ffe7e114-c1d3-41e3-b294-f49e2ad00f0b"), "A wrought iron lattice tower on the Champ de Mars, named after engineer Gustave Eiffel.", "Eiffel Tower" },
                    { 6, new Guid("ffe7e114-c1d3-41e3-b294-f49e2ad00f0b"), "The world's largest museum.", "The Louvre" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("06450b09-a0e9-4732-b056-b36a148891dc"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("ad15bd1a-d7c9-48b2-ae2b-027464482030"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("b5a53d85-5c02-43a3-aa04-9ca7414db146"));

            migrationBuilder.DeleteData(
                table: "PointsOfInterest",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PointsOfInterest",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PointsOfInterest",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PointsOfInterest",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "PointsOfInterest",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "PointsOfInterest",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("5a89ba5d-76c9-45f2-84bb-a8a2dad0d484"), "The one with that big park.", "New York City" },
                    { new Guid("ca203af8-7127-433e-ae28-9c255a04ed84"), "The one with the cathedral that was never really finished.", "Antwerp" },
                    { new Guid("ffe7e114-c1d3-41e3-b294-f49e2ad00f0b"), "The one with that big tower.", "Paris" }
                });
        }
    }
}
