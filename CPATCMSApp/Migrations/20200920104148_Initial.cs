using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CPATCMSApp.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssignmentSlots",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AssignmentName = table.Column<string>(nullable: true),
                    StartTime = table.Column<int>(nullable: false),
                    EndTime = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentSlots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bays",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bays", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BkashMessages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Message = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BkashMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CnFRegistrations",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CnFName = table.Column<string>(nullable: false),
                    CnFCode = table.Column<string>(nullable: false),
                    CnFPhoneNumber = table.Column<string>(nullable: false),
                    CnFAddress = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CnFRegistrations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Gates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethod",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethod", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrialAssignmentItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    YardId = table.Column<int>(nullable: false),
                    BlockName = table.Column<string>(nullable: true),
                    CnFName = table.Column<string>(nullable: true),
                    CnFCode = table.Column<string>(nullable: true),
                    Vessel = table.Column<string>(nullable: true),
                    ImpReg = table.Column<string>(nullable: true),
                    MLO = table.Column<string>(nullable: true),
                    ContainerNumber = table.Column<string>(nullable: true),
                    Size = table.Column<double>(nullable: false),
                    Height = table.Column<double>(nullable: false),
                    LineNumber = table.Column<string>(nullable: true),
                    Dst = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    VerifyNumber = table.Column<string>(nullable: true),
                    ExitNumber = table.Column<string>(nullable: true),
                    AssignmentType = table.Column<string>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrialAssignmentItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Assignments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    AssignmentSlotId = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    DeliveryDate = table.Column<DateTime>(nullable: false),
                    DeliveryStart = table.Column<int>(nullable: false),
                    DeliveryEnd = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assignments_AssignmentSlots_AssignmentSlotId",
                        column: x => x.AssignmentSlotId,
                        principalTable: "AssignmentSlots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Yards",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Number = table.Column<string>(nullable: true),
                    BayId = table.Column<int>(nullable: false),
                    GateId = table.Column<int>(nullable: true),
                    OutGateId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Yards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Yards_Bays_BayId",
                        column: x => x.BayId,
                        principalTable: "Bays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Yards_Gates_GateId",
                        column: x => x.GateId,
                        principalTable: "Gates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Yards_Gates_OutGateId",
                        column: x => x.OutGateId,
                        principalTable: "Gates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    GateId = table.Column<int>(nullable: true),
                    YardId = table.Column<int>(nullable: true),
                    UserTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Gates_GateId",
                        column: x => x.GateId,
                        principalTable: "Gates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_UserTypes_UserTypeId",
                        column: x => x.UserTypeId,
                        principalTable: "UserTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Yards_YardId",
                        column: x => x.YardId,
                        principalTable: "Yards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CnFProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ApplicationUserId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    VerificationNumber = table.Column<string>(nullable: true),
                    Balance = table.Column<double>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CnFProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CnFProfiles_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssignmentItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    YardId = table.Column<int>(nullable: false),
                    CnFProfileId = table.Column<int>(nullable: false),
                    AssignmentId = table.Column<int>(nullable: false),
                    Vessel = table.Column<string>(nullable: true),
                    ImpReg = table.Column<string>(nullable: true),
                    MLO = table.Column<string>(nullable: true),
                    ContainerNumber = table.Column<string>(nullable: true),
                    Size = table.Column<double>(nullable: false),
                    Height = table.Column<double>(nullable: false),
                    LineNumber = table.Column<string>(nullable: true),
                    Dst = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    VerifyNumber = table.Column<string>(nullable: true),
                    ExitNumber = table.Column<string>(nullable: true),
                    AssignmentType = table.Column<string>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    KeepDownTime = table.Column<DateTime>(nullable: false),
                    EstimatedTruckQty = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    ReleaseOrder = table.Column<string>(nullable: true),
                    CartTicket = table.Column<string>(nullable: true),
                    BillOfEntity = table.Column<string>(nullable: true),
                    AssesmentNotice = table.Column<string>(nullable: true),
                    OneStopBill = table.Column<string>(nullable: true),
                    ContainerScanCopy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssignmentItems_Assignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssignmentItems_CnFProfiles_CnFProfileId",
                        column: x => x.CnFProfileId,
                        principalTable: "CnFProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssignmentItems_Yards_YardId",
                        column: x => x.YardId,
                        principalTable: "Yards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    PaymentMethodId = table.Column<int>(nullable: false),
                    PaymentTypeId = table.Column<int>(nullable: false),
                    CnFProfileId = table.Column<int>(nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    VerificationCode = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    ReferenceCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_CnFProfiles_CnFProfileId",
                        column: x => x.CnFProfileId,
                        principalTable: "CnFProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Payments_PaymentMethod_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalTable: "PaymentMethod",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Payments_PaymentTypes_PaymentTypeId",
                        column: x => x.PaymentTypeId,
                        principalTable: "PaymentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TruckEntities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AssignmentItemId = table.Column<int>(nullable: false),
                    TruckNumer = table.Column<string>(nullable: true),
                    EntryDate = table.Column<DateTime>(nullable: false),
                    EntryTime = table.Column<int>(nullable: false),
                    ExitTime = table.Column<int>(nullable: false),
                    LoadingTime = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TruckEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TruckEntities_AssignmentItems_AssignmentItemId",
                        column: x => x.AssignmentItemId,
                        principalTable: "AssignmentItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "CreatedOn", "Description", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "a682b56a-6135-4111-a0k0-bdec547e3waz", "da9a3b0e-8b6f-8529-71d0-4fd255e23f15", new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Has All Permissions", "HarbourAndMarine", "HARBOURANDMARINE" },
                    { "b793b57a-6135-4221-b0l0-bdec547e3wax", "ga9a3b0e-8b6f-9130-67d0-4fd255e23f16", new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Has All Permissions", "SuperAdmin", "SUPERADMIN" },
                    { "c814b58b-7456-9332-c0m0-bdec765e3awc", "ea9a3b0f-9b5f-7153-81e0-4fd799e23f16", new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Has Minimum Permissions", "Cnf", "CNF" },
                    { "d925b59b-7456-1442-d0n0-bdec765e3awv", "ea9a3b0f-9b5f-7153-81e0-4fd799e23f17", new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Has Minimum Permissions", "GateSergent", "GATESERGENT" },
                    { "e136b60b-7456-2552-e0o0-bdec765e3awb", "ea9a3b0f-9b5f-7153-81e0-4fd799e23f18", new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Has Minimum Permissions", "GateAdmin", "GATEADMIN" },
                    { "f247b61b-7456-3662-f0p0-bdec765e3awn", "ea9a3b0f-9b5f-7153-81e0-4fd799e23f19", new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Has Minimum Permissions", "Yard", "YARD" },
                    { "g358b62b-7456-6772-g0q0-bdec765e3awm", "ea9a3b0f-9b5f-7153-81e0-4fd799e23f20", new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Has Minimum Permissions", "OneStop", "ONESTOP" },
                    { "h469b63b-7456-7882-h0r0-bdec765e3awl", "ea9a3b0f-9b5f-7153-81e0-4fd799e23f21", new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Has Minimum Permissions", "Mechanical", "MECHANICAL" },
                    { "i571b64b-7456-5992-i0s0-bdec765e3awk", "ea9a3b0f-9b5f-7153-81e0-4fd799e23f22", new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Has Viewer Permissions", "TMOffice", "TMOFFICE" },
                    { "j693b65b-7456-8112-j0t0-bdec765e3awj", "ea9a3b0f-9b5f-7153-81e0-4fd799e23f23", new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Has Viewer Permissions", "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AssignmentSlots",
                columns: new[] { "Id", "AssignmentName", "EndTime", "StartTime" },
                values: new object[,]
                {
                    { 1, "Assignment 1 (5 PM)", 1700, 1001 },
                    { 2, "Assignment 2 (9 PM)", 2100, 1701 },
                    { 3, "Assignment 3 (10 AM)", 1000, 2101 },
                    { 4, "Assignment 4 (Special)", 1600, 1400 }
                });

            migrationBuilder.InsertData(
                table: "Bays",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "GCB" });

            migrationBuilder.InsertData(
                table: "Gates",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 2, "Gate 2" },
                    { 3, "Gate 3" },
                    { 1, "Gate 1" }
                });

            migrationBuilder.InsertData(
                table: "PaymentMethod",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "bKash" },
                    { 4, "System" },
                    { 3, "Nagad" },
                    { 2, "Rocket" }
                });

            migrationBuilder.InsertData(
                table: "PaymentTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 3, "Fine" },
                    { 2, "Revenue" },
                    { 1, "Recharge" }
                });

            migrationBuilder.InsertData(
                table: "UserTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 2, "Gate" },
                    { 10, "SuperAdmin" },
                    { 9, "PaymentAdmin" },
                    { 8, "Admin" },
                    { 7, "TM Office" },
                    { 6, "Mechanical" },
                    { 5, "One Stop" },
                    { 3, "Gate Admin" },
                    { 1, "CNF" },
                    { 4, "Yard" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "GateId", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName", "UserTypeId", "YardId" },
                values: new object[,]
                {
                    { "8ab6ee64-f39c-44b4-ae27-dbb33cbfb510", 0, "26d21881-0a3a-44ab-aa4d-10664ace1e2g", "test@mail.com", false, null, false, null, "TEST@MAIL.COM", "GATEADMIN", "AQAAAAEAACcQAAAAEOJVsHUa611Khzkcg/zXgZ8EeegKhZAyW2eVPMzWJiToPuR45aBwuID99TNJ91JPxg==", null, false, "5TDMS5CNA2GYJK2URB4GDOCQI2NI7EPM", false, "GateAdmin", 3, null },
                    { "8ab6ee66-f41c-46b6-ae27-dbb55cbfb512", 0, "26d21881-0a3a-44ab-aa4d-10664ace1e2j", "test@mail.com", false, null, false, null, "TEST@MAIL.COM", "ONESTOP", "AQAAAAEAACcQAAAAEOJVsHUa611Khzkcg/zXgZ8EeegKhZAyW2eVPMzWJiToPuR45aBwuID99TNJ91JPxg==", null, false, "5TDMS5CNA2GYJK2URB4GDOCQI2NI7EPP", false, "OneStop", 5, null },
                    { "8ab6ee67-f42c-47b7-ae27-dbb66cbfb513", 0, "26d21881-0a3a-44ab-aa4d-10664ace1e2k", "test@mail.com", false, null, false, null, "TEST@MAIL.COM", "MECHANICAL", "AQAAAAEAACcQAAAAEOJVsHUa611Khzkcg/zXgZ8EeegKhZAyW2eVPMzWJiToPuR45aBwuID99TNJ91JPxg==", null, false, "5TDMS5CNA2GYJK2URB4GDOCQI2NI7EPQ", false, "Mechanical", 6, null },
                    { "8ab6ee68-f43c-48b8-ae27-dbb77cbfb514", 0, "26d21881-0a3a-44ab-aa4d-10664ace1e2l", "test@mail.com", false, null, false, null, "TEST@MAIL.COM", "TMOFFICE", "AQAAAAEAACcQAAAAEOJVsHUa611Khzkcg/zXgZ8EeegKhZAyW2eVPMzWJiToPuR45aBwuID99TNJ91JPxg==", null, false, "5TDMS5CNA2GYJK2URB4GDOCQI2NI7EPR", false, "TMOffice", 7, null },
                    { "8ab6ee61-f36c-41b1-ae27-dbb23cbfb507", 0, "26d21881-0a3a-44ab-aa4d-10664ace1e2d", "test@mail.com", false, null, false, null, "TEST@MAIL.COM", "HARBOURANDMARINE", "AQAAAAEAACcQAAAAEOJVsHUa611Khzkcg/zXgZ8EeegKhZAyW2eVPMzWJiToPuR45aBwuID99TNJ91JPxg==", null, false, "5TDMS5CNA2GYJK2URB4GDOCQI2NI7EHJ", false, "HarbourAndMArine", 8, null },
                    { "8ab6ee69-f44c-49b9-ae27-dbb88cbfb515", 0, "26d21881-0a3a-44ab-aa4d-10664ace1e2m", "test@mail.com", false, null, false, null, "TEST@MAIL.COM", "ADMIN", "AQAAAAEAACcQAAAAEOJVsHUa611Khzkcg/zXgZ8EeegKhZAyW2eVPMzWJiToPuR45aBwuID99TNJ91JPxg==", null, false, "5TDMS5CNA2GYJK2URB4GDOCQI2NI7EP3", false, "Admin", 9, null },
                    { "8ac6ee70-f45c-50b0-ae27-dbb99cbfb516", 0, "26d21779-1a4a-55ab-aa4d-34567ace1e2n", "neurostorm.soft@gmail.com", false, null, false, null, "NEUROSTORM.SOFT@GMAIL.COM", "NEUROSTORM", "AQAAAAEAACcQAAAAEOJVsHUa611Khzkcg/zXgZ8EeegKhZAyW2eVPMzWJiToPuR45aBwuID99TNJ91JPxg==", null, false, "6VCMS5CNA2GYJI4NET6YGDOCQI2NI7EP4", false, "NeuroStorm", 10, null },
                    { "8ab6ee62-f37c-42b2-ae27-dbb11cbfb508", 0, "26d21881-0a3a-44ab-aa4d-10664ace1e2e", "test@mail.com", false, 1, false, null, "TEST@MAIL.COM", "NCYGATEIN", "AQAAAAEAACcQAAAAEOJVsHUa611Khzkcg/zXgZ8EeegKhZAyW2eVPMzWJiToPuR45aBwuID99TNJ91JPxg==", null, false, "5TDMS5CNA2GYJK2URB4GDOCQI2NI7EHK", false, "NCYGateIn", 2, null },
                    { "8ab6ee63-f38c-43b3-ae27-dbb22cbfb509", 0, "26d21881-0a3a-44ab-aa4d-10664ace1e2f", "test@mail.com", false, 2, false, null, "TEST@MAIL.COM", "NCYGATEOUT", "AQAAAAEAACcQAAAAEOJVsHUa611Khzkcg/zXgZ8EeegKhZAyW2eVPMzWJiToPuR45aBwuID99TNJ91JPxg==", null, false, "5TDMS5CNA2GYJK2URB4GDOCQI2NI7EPL", false, "NCYGateOut", 2, null }
                });

            migrationBuilder.InsertData(
                table: "Yards",
                columns: new[] { "Id", "BayId", "GateId", "Name", "Number", "OutGateId" },
                values: new object[,]
                {
                    { 1, 1, 1, "Yard 1", "1", null },
                    { 2, 1, 2, "Yard 2", "2", null }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[,]
                {
                    { "8ab6ee64-f39c-44b4-ae27-dbb33cbfb510", "e136b60b-7456-2552-e0o0-bdec765e3awb" },
                    { "8ab6ee66-f41c-46b6-ae27-dbb55cbfb512", "g358b62b-7456-6772-g0q0-bdec765e3awm" },
                    { "8ab6ee67-f42c-47b7-ae27-dbb66cbfb513", "h469b63b-7456-7882-h0r0-bdec765e3awl" },
                    { "8ab6ee68-f43c-48b8-ae27-dbb77cbfb514", "i571b64b-7456-5992-i0s0-bdec765e3awk" },
                    { "8ab6ee61-f36c-41b1-ae27-dbb23cbfb507", "a682b56a-6135-4111-a0k0-bdec547e3waz" },
                    { "8ab6ee69-f44c-49b9-ae27-dbb88cbfb515", "j693b65b-7456-8112-j0t0-bdec765e3awj" },
                    { "8ac6ee70-f45c-50b0-ae27-dbb99cbfb516", "b793b57a-6135-4221-b0l0-bdec547e3wax" },
                    { "8ab6ee62-f37c-42b2-ae27-dbb11cbfb508", "d925b59b-7456-1442-d0n0-bdec765e3awv" },
                    { "8ab6ee63-f38c-43b3-ae27-dbb22cbfb509", "d925b59b-7456-1442-d0n0-bdec765e3awv" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "GateId", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName", "UserTypeId", "YardId" },
                values: new object[] { "8ab6ee65-f40c-45b5-ae27-dbb44cbfb511", 0, "26d21881-0a3a-44ab-aa4d-10664ace1e2h", "test@mail.com", false, null, false, null, "TEST@MAIL.COM", "YARDADMIN", "AQAAAAEAACcQAAAAEOJVsHUa611Khzkcg/zXgZ8EeegKhZAyW2eVPMzWJiToPuR45aBwuID99TNJ91JPxg==", null, false, "5TDMS5CNA2GYJK2URB4GDOCQI2NI7EPN", false, "YardAdmin", 4, 1 });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { "8ab6ee65-f40c-45b5-ae27-dbb44cbfb511", "f247b61b-7456-3662-f0p0-bdec765e3awn" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_GateId",
                table: "AspNetUsers",
                column: "GateId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserTypeId",
                table: "AspNetUsers",
                column: "UserTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_YardId",
                table: "AspNetUsers",
                column: "YardId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentItems_AssignmentId",
                table: "AssignmentItems",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentItems_CnFProfileId",
                table: "AssignmentItems",
                column: "CnFProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentItems_YardId",
                table: "AssignmentItems",
                column: "YardId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_AssignmentSlotId",
                table: "Assignments",
                column: "AssignmentSlotId");

            migrationBuilder.CreateIndex(
                name: "IX_CnFProfiles_ApplicationUserId",
                table: "CnFProfiles",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CnFProfileId",
                table: "Payments",
                column: "CnFProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentMethodId",
                table: "Payments",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentTypeId",
                table: "Payments",
                column: "PaymentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TruckEntities_AssignmentItemId",
                table: "TruckEntities",
                column: "AssignmentItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Yards_BayId",
                table: "Yards",
                column: "BayId");

            migrationBuilder.CreateIndex(
                name: "IX_Yards_GateId",
                table: "Yards",
                column: "GateId");

            migrationBuilder.CreateIndex(
                name: "IX_Yards_OutGateId",
                table: "Yards",
                column: "OutGateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BkashMessages");

            migrationBuilder.DropTable(
                name: "CnFRegistrations");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "TrialAssignmentItems");

            migrationBuilder.DropTable(
                name: "TruckEntities");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "PaymentMethod");

            migrationBuilder.DropTable(
                name: "PaymentTypes");

            migrationBuilder.DropTable(
                name: "AssignmentItems");

            migrationBuilder.DropTable(
                name: "Assignments");

            migrationBuilder.DropTable(
                name: "CnFProfiles");

            migrationBuilder.DropTable(
                name: "AssignmentSlots");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "UserTypes");

            migrationBuilder.DropTable(
                name: "Yards");

            migrationBuilder.DropTable(
                name: "Bays");

            migrationBuilder.DropTable(
                name: "Gates");
        }
    }
}
