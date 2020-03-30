using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MemoryBook.StorageInitializer.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "MemoryBook");

            migrationBuilder.CreateTable(
                name: "DetailType",
                schema: "MemoryBook",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Code = table.Column<string>(maxLength: 50, nullable: false),
                    DetailStartText = table.Column<string>(nullable: false),
                    DetailEndText = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EntityType",
                schema: "MemoryBook",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Code = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MemoryBookUniverses",
                schema: "MemoryBook",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Name = table.Column<string>(maxLength: 1000, nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemoryBookUniverses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RelationshipType",
                schema: "MemoryBook",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Code = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelationshipType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Group",
                schema: "MemoryBook",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    MemoryBookUniverseId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 1000, nullable: false),
                    Code = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Group_MemoryBookUniverses_MemoryBookUniverseId",
                        column: x => x.MemoryBookUniverseId,
                        principalSchema: "MemoryBook",
                        principalTable: "MemoryBookUniverses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Member",
                schema: "MemoryBook",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    MemoryBookUniverseId = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 1000, nullable: false),
                    MiddleName = table.Column<string>(maxLength: 1000, nullable: true),
                    LastName = table.Column<string>(maxLength: 1000, nullable: false),
                    CommonName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Member", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Member_MemoryBookUniverses_MemoryBookUniverseId",
                        column: x => x.MemoryBookUniverseId,
                        principalSchema: "MemoryBook",
                        principalTable: "MemoryBookUniverses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Relationship",
                schema: "MemoryBook",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    MemoryBookUniverseId = table.Column<Guid>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: true),
                    EndTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Relationship", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Relationship_MemoryBookUniverses_MemoryBookUniverseId",
                        column: x => x.MemoryBookUniverseId,
                        principalSchema: "MemoryBook",
                        principalTable: "MemoryBookUniverses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Detail",
                schema: "MemoryBook",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    CustomDetailText = table.Column<string>(nullable: true),
                    StartTime = table.Column<DateTime>(nullable: true),
                    EndTime = table.Column<DateTime>(nullable: true),
                    DetailTypeId = table.Column<Guid>(nullable: false),
                    Story = table.Column<string>(nullable: true),
                    CreatorId = table.Column<Guid>(nullable: false),
                    GroupId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Detail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Detail_Member_CreatorId",
                        column: x => x.CreatorId,
                        principalSchema: "MemoryBook",
                        principalTable: "Member",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Detail_DetailType_DetailTypeId",
                        column: x => x.DetailTypeId,
                        principalSchema: "MemoryBook",
                        principalTable: "DetailType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Detail_Group_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "MemoryBook",
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GroupMembership",
                schema: "MemoryBook",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    MemberId = table.Column<Guid>(nullable: false),
                    GroupId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMembership", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupMembership_Group_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "MemoryBook",
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupMembership_Member_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "MemoryBook",
                        principalTable: "Member",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RelationshipMembership",
                schema: "MemoryBook",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    StartTime = table.Column<DateTime>(nullable: true),
                    EndTime = table.Column<DateTime>(nullable: true),
                    MemberId = table.Column<Guid>(nullable: false),
                    MemberRelationshipTypeId = table.Column<Guid>(nullable: false),
                    RelationshipId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelationshipMembership", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RelationshipMembership_Member_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "MemoryBook",
                        principalTable: "Member",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RelationshipMembership_RelationshipType_MemberRelationshipTypeId",
                        column: x => x.MemberRelationshipTypeId,
                        principalSchema: "MemoryBook",
                        principalTable: "RelationshipType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RelationshipMembership_Relationship_RelationshipId",
                        column: x => x.RelationshipId,
                        principalSchema: "MemoryBook",
                        principalTable: "Relationship",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DetailAssociation",
                schema: "MemoryBook",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    EntityId = table.Column<Guid>(nullable: false),
                    EntityTypeId = table.Column<Guid>(nullable: false),
                    DetailId = table.Column<Guid>(nullable: false),
                    GroupId = table.Column<Guid>(nullable: true),
                    MemberId = table.Column<Guid>(nullable: true),
                    RelationshipId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailAssociation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetailAssociation_Detail_DetailId",
                        column: x => x.DetailId,
                        principalSchema: "MemoryBook",
                        principalTable: "Detail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetailAssociation_EntityType_EntityTypeId",
                        column: x => x.EntityTypeId,
                        principalSchema: "MemoryBook",
                        principalTable: "EntityType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetailAssociation_Group_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "MemoryBook",
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DetailAssociation_Member_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "MemoryBook",
                        principalTable: "Member",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DetailAssociation_Relationship_RelationshipId",
                        column: x => x.RelationshipId,
                        principalSchema: "MemoryBook",
                        principalTable: "Relationship",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DetailPermission",
                schema: "MemoryBook",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    MemberId = table.Column<Guid>(nullable: false),
                    DetailId = table.Column<Guid>(nullable: false),
                    CanEdit = table.Column<bool>(nullable: false),
                    CanDelete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailPermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetailPermission_Detail_DetailId",
                        column: x => x.DetailId,
                        principalSchema: "MemoryBook",
                        principalTable: "Detail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetailPermission_Member_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "MemoryBook",
                        principalTable: "Member",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Detail_CreatorId",
                schema: "MemoryBook",
                table: "Detail",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Detail_DetailTypeId",
                schema: "MemoryBook",
                table: "Detail",
                column: "DetailTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Detail_GroupId",
                schema: "MemoryBook",
                table: "Detail",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_DetailAssociation_DetailId",
                schema: "MemoryBook",
                table: "DetailAssociation",
                column: "DetailId");

            migrationBuilder.CreateIndex(
                name: "IX_DetailAssociation_EntityTypeId",
                schema: "MemoryBook",
                table: "DetailAssociation",
                column: "EntityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DetailAssociation_GroupId",
                schema: "MemoryBook",
                table: "DetailAssociation",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_DetailAssociation_MemberId",
                schema: "MemoryBook",
                table: "DetailAssociation",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_DetailAssociation_RelationshipId",
                schema: "MemoryBook",
                table: "DetailAssociation",
                column: "RelationshipId");

            migrationBuilder.CreateIndex(
                name: "IX_DetailPermission_DetailId",
                schema: "MemoryBook",
                table: "DetailPermission",
                column: "DetailId");

            migrationBuilder.CreateIndex(
                name: "IX_DetailPermission_MemberId",
                schema: "MemoryBook",
                table: "DetailPermission",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_DetailType_Code",
                schema: "MemoryBook",
                table: "DetailType",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EntityType_Code",
                schema: "MemoryBook",
                table: "EntityType",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Group_Code",
                schema: "MemoryBook",
                table: "Group",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Group_MemoryBookUniverseId",
                schema: "MemoryBook",
                table: "Group",
                column: "MemoryBookUniverseId");

            migrationBuilder.CreateIndex(
                name: "IX_Group_Name",
                schema: "MemoryBook",
                table: "Group",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupMembership_GroupId",
                schema: "MemoryBook",
                table: "GroupMembership",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMembership_MemberId",
                schema: "MemoryBook",
                table: "GroupMembership",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Member_CommonName",
                schema: "MemoryBook",
                table: "Member",
                column: "CommonName",
                unique: true,
                filter: "[CommonName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Member_MemoryBookUniverseId",
                schema: "MemoryBook",
                table: "Member",
                column: "MemoryBookUniverseId");

            migrationBuilder.CreateIndex(
                name: "IX_Member_FirstName_MiddleName_LastName",
                schema: "MemoryBook",
                table: "Member",
                columns: new[] { "FirstName", "MiddleName", "LastName" },
                unique: true,
                filter: "[MiddleName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MemoryBookUniverses_Name",
                schema: "MemoryBook",
                table: "MemoryBookUniverses",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Relationship_MemoryBookUniverseId",
                schema: "MemoryBook",
                table: "Relationship",
                column: "MemoryBookUniverseId");

            migrationBuilder.CreateIndex(
                name: "IX_RelationshipMembership_MemberId",
                schema: "MemoryBook",
                table: "RelationshipMembership",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_RelationshipMembership_MemberRelationshipTypeId",
                schema: "MemoryBook",
                table: "RelationshipMembership",
                column: "MemberRelationshipTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RelationshipMembership_RelationshipId",
                schema: "MemoryBook",
                table: "RelationshipMembership",
                column: "RelationshipId");

            migrationBuilder.CreateIndex(
                name: "IX_RelationshipType_Code",
                schema: "MemoryBook",
                table: "RelationshipType",
                column: "Code",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetailAssociation",
                schema: "MemoryBook");

            migrationBuilder.DropTable(
                name: "DetailPermission",
                schema: "MemoryBook");

            migrationBuilder.DropTable(
                name: "GroupMembership",
                schema: "MemoryBook");

            migrationBuilder.DropTable(
                name: "RelationshipMembership",
                schema: "MemoryBook");

            migrationBuilder.DropTable(
                name: "EntityType",
                schema: "MemoryBook");

            migrationBuilder.DropTable(
                name: "Detail",
                schema: "MemoryBook");

            migrationBuilder.DropTable(
                name: "RelationshipType",
                schema: "MemoryBook");

            migrationBuilder.DropTable(
                name: "Relationship",
                schema: "MemoryBook");

            migrationBuilder.DropTable(
                name: "Member",
                schema: "MemoryBook");

            migrationBuilder.DropTable(
                name: "DetailType",
                schema: "MemoryBook");

            migrationBuilder.DropTable(
                name: "Group",
                schema: "MemoryBook");

            migrationBuilder.DropTable(
                name: "MemoryBookUniverses",
                schema: "MemoryBook");
        }
    }
}
