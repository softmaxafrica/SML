using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace lsclient.Server.Migrations
{
    /// <inheritdoc />
    public partial class InitialFreshMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES 
                       WHERE TABLE_NAME = 'Companies')
        BEGIN
            CREATE TABLE Companies (
                COMPANY_ID NVARCHAR(450) PRIMARY KEY,
                COMPANY_TIN_NUMBER NVARCHAR(MAX) NOT NULL,
                COMPANY_NAME NVARCHAR(MAX) NOT NULL,
                ADMIN_FULL_NAME NVARCHAR(MAX) NOT NULL,
                ADMIN_EMAIL NVARCHAR(MAX) NOT NULL,
                ADMIN_CONTACT NVARCHAR(MAX) NOT NULL,
                COMPANY_ADDRESS NVARCHAR(MAX) NULL,
                COMPANY_DESCRIPTION NVARCHAR(MAX) NULL,
                COMPANY_LATITUDE FLOAT NULL,
                COMPANY_LONGITUDE FLOAT NULL
            );
        END
    ");

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CUSTOMER_ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FULL_NAME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EMAIL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PHONE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ADDRESS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PAYMENT_METHOD = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CARD_NUMBER = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CARD_TYPE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BILLING_ADDRESS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EXPIRY_DATE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BANK_NAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BANK_ACCOUNT_NUMBER = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BANK_ACCOUNT_HOLDER = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MOBILE_NETWORK = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MOBILE_NUMBER = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PROFILE_IMAGE_PATH = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CUSTOMER_ID);
                });

            migrationBuilder.CreateTable(
                name: "GenTableColumns",
                columns: table => new
                {
                    CODE = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TABLE_CODE = table.Column<long>(type: "bigint", nullable: true),
                    FIELD = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HEADER = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WIDTH = table.Column<long>(type: "bigint", nullable: true),
                    TYPE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    COLUMN_DISPLAY = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    COLUMN_INCLUDED = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    COLUMN_INDEX = table.Column<long>(type: "bigint", nullable: true),
                    LAST_ACTION = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CUSER = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EUSER = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EDATE = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenTableColumns", x => x.CODE);
                });

            migrationBuilder.CreateTable(
                name: "GenTables",
                columns: table => new
                {
                    CODE = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LAST_ACTION = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CUSER = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EUSER = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EDATE = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenTables", x => x.CODE);
                });

            migrationBuilder.CreateTable(
                name: "GenTranslations",
                columns: table => new
                {
                    CODE = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LANGUAGE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LAST_ACTION = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CUSER = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EUSER = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EDATE = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenTranslations", x => x.CODE);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    IMAGE_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IMAGE_NAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DIRECTORY = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IMAGE_URL = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.IMAGE_ID);
                });

            migrationBuilder.CreateTable(
                name: "SecUsers",
                columns: table => new
                {
                    USER_ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PASSWORD_HASH = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ROLE = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EMAIL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LAST_LOGIN = table.Column<DateTime>(type: "datetime2", nullable: true),
                    STATUS = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecUsers", x => x.USER_ID);
                });

            migrationBuilder.CreateTable(
                name: "TruckTypes",
                columns: table => new
                {
                    TRUCK_TYPE_ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TYPE_NAME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SAMPLE_IMAGE_URL = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TruckTypes", x => x.TRUCK_TYPE_ID);
                });

            migrationBuilder.CreateTable(
                name: "Drivers",
                columns: table => new
                {
                    DRIVER_ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FULL_NAME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EMAIL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PHONE = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LICENSE_NUMBER = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    STATUS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    REGISTRATION_COMMENT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LICENSE_CLASSES = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LICENSE_EXPIRE_DATE = table.Column<DateOnly>(type: "date", nullable: true),
                    IS_AVILABLE_FOR_BOOKING = table.Column<bool>(type: "bit", nullable: true),
                    IMAGE_URL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.DRIVER_ID);
                    table.ForeignKey(
                        name: "FK_Drivers_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "Companies",
                        principalColumn: "COMPANY_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyCustomers",
                columns: table => new
                {
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CompanyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TXNID = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyCustomers", x => new { x.CustomerId, x.CompanyId });
                    table.ForeignKey(
                        name: "FK_CompanyCustomers_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "COMPANY_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyCustomers_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CUSTOMER_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contracts",
                columns: table => new
                {
                    CONTRACT_ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    REQUEST_ID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    COMPANY_ID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CUSTOMER_ID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CONTRACT_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TERMS_AND_CONDITIONS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AGREED_PRICE = table.Column<double>(type: "float", nullable: true),
                    ADVANCE_PAYMENT = table.Column<double>(type: "float", nullable: true),
                    ADVANCE_PAYMENT_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    STATUS = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.CONTRACT_ID);
                    table.ForeignKey(
                        name: "FK_Contracts_Companies_COMPANY_ID",
                        column: x => x.COMPANY_ID,
                        principalTable: "Companies",
                        principalColumn: "COMPANY_ID");
                    table.ForeignKey(
                        name: "FK_Contracts_Customers_CUSTOMER_ID",
                        column: x => x.CUSTOMER_ID,
                        principalTable: "Customers",
                        principalColumn: "CUSTOMER_ID");
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    INVOICE_NUMBER = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    COMPANY_ID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CUSTOMER_ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    JOB_REQUEST_ID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PAYMENT_ID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TOTAL_AMOUNT = table.Column<double>(type: "float", nullable: true),
                    TOTAL_PAID_AMOUNT = table.Column<double>(type: "float", nullable: true),
                    OWED_AMOUNT = table.Column<double>(type: "float", nullable: true),
                    SERVICE_CHARGE = table.Column<double>(type: "float", nullable: true),
                    OPERATIONAL_CHARGE = table.Column<double>(type: "float", nullable: true),
                    ISSUE_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DUE_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    STATUS = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.INVOICE_NUMBER);
                    table.ForeignKey(
                        name: "FK_Invoices_Customers_CUSTOMER_ID",
                        column: x => x.CUSTOMER_ID,
                        principalTable: "Customers",
                        principalColumn: "CUSTOMER_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DriverTruckType",
                columns: table => new
                {
                    DriverID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TruckTypeID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverTruckType", x => new { x.DriverID, x.TruckTypeID });
                    table.ForeignKey(
                        name: "FK_DriverTruckType_Driver",
                        column: x => x.DriverID,
                        principalTable: "Drivers",
                        principalColumn: "DRIVER_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DriverTruckType_TruckType",
                        column: x => x.TruckTypeID,
                        principalTable: "TruckTypes",
                        principalColumn: "TRUCK_TYPE_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Trucks",
                columns: table => new
                {
                    TRUCK_ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TRUCK_NUMBER = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MODEL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    COMPANY_ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DriverID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "bit", nullable: false),
                    IS_TRUCK_AVILABLE_FOR_BOOKING = table.Column<bool>(type: "bit", nullable: false),
                    TRUCK_TYPE_ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CHASIS_NO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BRAND = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ENGINE_CAPACITY = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FUEL_TYPE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CABIN_TYPE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CATEGORY = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DRIVE = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trucks", x => x.TRUCK_ID);
                    table.ForeignKey(
                        name: "FK_Trucks_Companies_COMPANY_ID",
                        column: x => x.COMPANY_ID,
                        principalTable: "Companies",
                        principalColumn: "COMPANY_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Trucks_Drivers_DriverID",
                        column: x => x.DriverID,
                        principalTable: "Drivers",
                        principalColumn: "DRIVER_ID");
                    table.ForeignKey(
                        name: "FK_Trucks_TruckTypes_TRUCK_TYPE_ID",
                        column: x => x.TRUCK_TYPE_ID,
                        principalTable: "TruckTypes",
                        principalColumn: "TRUCK_TYPE_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PAYMENT_ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    INVOICE_NUMBER = table.Column<int>(type: "int", nullable: false),
                    AMOUNT_PAID = table.Column<double>(type: "float", nullable: false),
                    PAYMENT_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PAYMENT_METHOD = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    REFERENCE_NUMBER = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CURRENCY = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PAYMENT_ID);
                    table.ForeignKey(
                        name: "FK_Payments_Invoices_INVOICE_NUMBER",
                        column: x => x.INVOICE_NUMBER,
                        principalTable: "Invoices",
                        principalColumn: "INVOICE_NUMBER",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    LOCATION_ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TRUCK_ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    REQUEST_ID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LATITUDE = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LONGITUDE = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TIME_STAMP = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.LOCATION_ID);
                    table.ForeignKey(
                        name: "FK_Locations_Trucks_TRUCK_ID",
                        column: x => x.TRUCK_ID,
                        principalTable: "Trucks",
                        principalColumn: "TRUCK_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChargableItems",
                columns: table => new
                {
                    ITEM_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JOB_REQUEST_ID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PRICE_AGREEMENT_ID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    STATUS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    INVOICE_NUMBER = table.Column<int>(type: "int", nullable: true),
                    CUSTOMER_ID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ITEM_DESCRRIPTION = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AMOUNT = table.Column<double>(type: "float", nullable: true),
                    ISSUE_DATE = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChargableItems", x => x.ITEM_ID);
                });

            migrationBuilder.CreateTable(
                name: "JobRequests",
                columns: table => new
                {
                    JOB_REQUEST_ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ASSIGNED_COMPANY = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PICKUP_LOCATION = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DELIVERY_LOCATION = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CARGO_DESCRIPTION = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CONTAINER_NUMBER = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    REQUEST_TYPE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    STATUS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PRICE_AGREEMENT_ID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TRUCK_TYPE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TruckID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LAST_UPDATE_TIME = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FIRST_DEPOSIT_AMOUNT = table.Column<double>(type: "float", nullable: true),
                    COMPANY_ADVANCE_AMOUNT_REQUIRED = table.Column<double>(type: "float", nullable: true),
                    CONTRACT_ID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    INVOICE_NUMBER = table.Column<int>(type: "int", nullable: true),
                    DriverID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CustomerID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CompanyID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobRequests", x => x.JOB_REQUEST_ID);
                    table.ForeignKey(
                        name: "FK_JobRequests_Contracts_CONTRACT_ID",
                        column: x => x.CONTRACT_ID,
                        principalTable: "Contracts",
                        principalColumn: "CONTRACT_ID");
                    table.ForeignKey(
                        name: "FK_JobRequests_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "CUSTOMER_ID");
                    table.ForeignKey(
                        name: "FK_JobRequests_Drivers_DriverID",
                        column: x => x.DriverID,
                        principalTable: "Drivers",
                        principalColumn: "DRIVER_ID");
                    table.ForeignKey(
                        name: "FK_JobRequests_Invoices_INVOICE_NUMBER",
                        column: x => x.INVOICE_NUMBER,
                        principalTable: "Invoices",
                        principalColumn: "INVOICE_NUMBER");
                    table.ForeignKey(
                        name: "FK_JobRequests_Trucks_TruckID",
                        column: x => x.TruckID,
                        principalTable: "Trucks",
                        principalColumn: "TRUCK_ID");
                });

            migrationBuilder.CreateTable(
                name: "PriceAgreements",
                columns: table => new
                {
                    PRICE_AGREEMENT_ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    COMPANY_ID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    REQUEST_ID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CUSTOMER_ID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    COMPANY_PRICE = table.Column<double>(type: "float", nullable: true),
                    CUSTOMER_PRICE = table.Column<double>(type: "float", nullable: true),
                    AGREED_PRICE = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceAgreements", x => x.PRICE_AGREEMENT_ID);
                    table.ForeignKey(
                        name: "FK_PriceAgreements_JobRequests_REQUEST_ID",
                        column: x => x.REQUEST_ID,
                        principalTable: "JobRequests",
                        principalColumn: "JOB_REQUEST_ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChargableItems_JOB_REQUEST_ID",
                table: "ChargableItems",
                column: "JOB_REQUEST_ID");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyCustomers_CompanyId",
                table: "CompanyCustomers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_COMPANY_ID",
                table: "Contracts",
                column: "COMPANY_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_CUSTOMER_ID",
                table: "Contracts",
                column: "CUSTOMER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_CompanyID",
                table: "Drivers",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_DriverTruckType_TruckTypeID",
                table: "DriverTruckType",
                column: "TruckTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_CUSTOMER_ID",
                table: "Invoices",
                column: "CUSTOMER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_JobRequests_CONTRACT_ID",
                table: "JobRequests",
                column: "CONTRACT_ID",
                unique: true,
                filter: "[CONTRACT_ID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_JobRequests_CustomerID",
                table: "JobRequests",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_JobRequests_DriverID",
                table: "JobRequests",
                column: "DriverID");

            migrationBuilder.CreateIndex(
                name: "IX_JobRequests_INVOICE_NUMBER",
                table: "JobRequests",
                column: "INVOICE_NUMBER");

            migrationBuilder.CreateIndex(
                name: "IX_JobRequests_PRICE_AGREEMENT_ID",
                table: "JobRequests",
                column: "PRICE_AGREEMENT_ID",
                unique: true,
                filter: "[PRICE_AGREEMENT_ID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_JobRequests_TruckID",
                table: "JobRequests",
                column: "TruckID");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_TRUCK_ID",
                table: "Locations",
                column: "TRUCK_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_INVOICE_NUMBER",
                table: "Payments",
                column: "INVOICE_NUMBER");

            migrationBuilder.CreateIndex(
                name: "IX_PriceAgreements_REQUEST_ID",
                table: "PriceAgreements",
                column: "REQUEST_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Trucks_COMPANY_ID",
                table: "Trucks",
                column: "COMPANY_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Trucks_DriverID",
                table: "Trucks",
                column: "DriverID");

            migrationBuilder.CreateIndex(
                name: "IX_Trucks_TRUCK_TYPE_ID",
                table: "Trucks",
                column: "TRUCK_TYPE_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ChargableItems_JobRequests_JOB_REQUEST_ID",
                table: "ChargableItems",
                column: "JOB_REQUEST_ID",
                principalTable: "JobRequests",
                principalColumn: "JOB_REQUEST_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_JobRequests_PriceAgreements_PRICE_AGREEMENT_ID",
                table: "JobRequests",
                column: "PRICE_AGREEMENT_ID",
                principalTable: "PriceAgreements",
                principalColumn: "PRICE_AGREEMENT_ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PriceAgreements_JobRequests_REQUEST_ID",
                table: "PriceAgreements");

            migrationBuilder.DropTable(
                name: "ChargableItems");

            migrationBuilder.DropTable(
                name: "CompanyCustomers");

            migrationBuilder.DropTable(
                name: "DriverTruckType");

            migrationBuilder.DropTable(
                name: "GenTableColumns");

            migrationBuilder.DropTable(
                name: "GenTables");

            migrationBuilder.DropTable(
                name: "GenTranslations");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "SecUsers");

            migrationBuilder.DropTable(
                name: "JobRequests");

            migrationBuilder.DropTable(
                name: "Contracts");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "PriceAgreements");

            migrationBuilder.DropTable(
                name: "Trucks");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Drivers");

            migrationBuilder.DropTable(
                name: "TruckTypes");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
