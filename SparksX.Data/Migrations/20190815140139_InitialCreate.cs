using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SparksX.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RecordStatus",
                columns: table => new
                {
                    RecordStatusId = table.Column<byte>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RecordStatusName = table.Column<string>(maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecordStatus", x => x.RecordStatusId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    UserName = table.Column<string>(maxLength: 50, nullable: false),
                    Password = table.Column<string>(maxLength: 20, nullable: false),
                    EmailAddress = table.Column<string>(maxLength: 100, nullable: true),
                    RecordStatusId = table.Column<byte>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<int>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users_1", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Users",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Users2",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Users1",
                        column: x => x.ModifiedBy,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_RecordStatus",
                        column: x => x.RecordStatusId,
                        principalTable: "RecordStatus",
                        principalColumn: "RecordStatusId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    CompanyId = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    CompanyName = table.Column<string>(maxLength: 100, nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false),
                    RecordStatusId = table.Column<byte>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<int>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.CompanyId);
                    table.ForeignKey(
                        name: "FK_Company_Users",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Company_Users2",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Company_Users1",
                        column: x => x.ModifiedBy,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Company_RecordStatuses",
                        column: x => x.RecordStatusId,
                        principalTable: "RecordStatus",
                        principalColumn: "RecordStatusId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    CustomerId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    OtherId = table.Column<string>(maxLength: 20, nullable: true),
                    UserNameWs = table.Column<string>(maxLength: 20, nullable: true),
                    PasswordWs = table.Column<string>(maxLength: 20, nullable: true),
                    RecordStatusId = table.Column<byte>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<int>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.CustomerId);
                    table.ForeignKey(
                        name: "FK_Customer_Users",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Customer_Users2",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Customer_Users1",
                        column: x => x.ModifiedBy,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Customer_RecordStatuses",
                        column: x => x.RecordStatusId,
                        principalTable: "RecordStatus",
                        principalColumn: "RecordStatusId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(nullable: false),
                    CustomerId = table.Column<Guid>(nullable: false),
                    ProductNo = table.Column<string>(maxLength: 50, nullable: false),
                    ProductNameTr = table.Column<string>(maxLength: 100, nullable: false),
                    ProductNameEng = table.Column<string>(maxLength: 100, nullable: true),
                    ProductNameOrg = table.Column<string>(maxLength: 100, nullable: true),
                    HsCode = table.Column<string>(maxLength: 16, nullable: false),
                    Uom = table.Column<string>(maxLength: 3, nullable: true),
                    GrossWeight = table.Column<double>(nullable: true),
                    NetWeight = table.Column<double>(nullable: true),
                    RecordStatusId = table.Column<byte>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<int>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Product_Users",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Product_Customer",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Product_Users2",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Product_Users1",
                        column: x => x.ModifiedBy,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Product_RecordStatuses",
                        column: x => x.RecordStatusId,
                        principalTable: "RecordStatus",
                        principalColumn: "RecordStatusId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkOrderMaster",
                columns: table => new
                {
                    WorkOrderMasterId = table.Column<Guid>(nullable: false),
                    WorkOrderNo = table.Column<string>(maxLength: 30, nullable: false),
                    DeclarationType = table.Column<string>(maxLength: 2, nullable: false),
                    Status = table.Column<int>(nullable: false),
                    MasterId = table.Column<Guid>(nullable: true),
                    CustomerId = table.Column<Guid>(nullable: false),
                    Message = table.Column<string>(type: "ntext", nullable: true),
                    RecordStatusId = table.Column<byte>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<int>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkOrderMaster", x => x.WorkOrderMasterId);
                    table.ForeignKey(
                        name: "FK_WorkOrderMaster_Users",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkOrderMaster_Customer",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkOrderMaster_Users2",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkOrderMaster_Users1",
                        column: x => x.ModifiedBy,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkOrderMaster_RecordStatuses",
                        column: x => x.RecordStatusId,
                        principalTable: "RecordStatus",
                        principalColumn: "RecordStatusId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Invoice",
                columns: table => new
                {
                    InvoiceId = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    WorkOrderMasterId = table.Column<Guid>(nullable: false),
                    SenderNo = table.Column<string>(maxLength: 30, nullable: true),
                    SenderName = table.Column<string>(maxLength: 250, nullable: true),
                    SenderAddress = table.Column<string>(maxLength: 2000, nullable: true),
                    SenderCity = table.Column<string>(maxLength: 60, nullable: true),
                    SenderCountry = table.Column<string>(maxLength: 25, nullable: true),
                    ConsgnName = table.Column<string>(maxLength: 250, nullable: true),
                    ConsgnAddress = table.Column<string>(maxLength: 2000, nullable: true),
                    ConsgnCity = table.Column<string>(maxLength: 60, nullable: true),
                    ConsgnCountry = table.Column<string>(maxLength: 25, nullable: true),
                    TransptrName = table.Column<string>(maxLength: 25, nullable: true),
                    VesselName = table.Column<string>(maxLength: 250, nullable: true),
                    AgentName = table.Column<string>(maxLength: 250, nullable: true),
                    PlateNo = table.Column<string>(maxLength: 50, nullable: true),
                    AwbNo = table.Column<string>(maxLength: 30, nullable: true),
                    BLNo = table.Column<string>(maxLength: 25, nullable: true),
                    Incoterms = table.Column<string>(maxLength: 25, nullable: true),
                    DeliveryLocation = table.Column<string>(maxLength: 250, nullable: true),
                    InvoiceAmount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    InvoiceCurrency = table.Column<string>(maxLength: 6, nullable: false),
                    FreightAmount = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    FreightCurrency = table.Column<string>(maxLength: 6, nullable: true),
                    InsuranceAmount = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    InsuranceCurrency = table.Column<string>(maxLength: 6, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoice", x => x.InvoiceId);
                    table.ForeignKey(
                        name: "FK_Invoices_WorkOrderMasters",
                        column: x => x.WorkOrderMasterId,
                        principalTable: "WorkOrderMaster",
                        principalColumn: "WorkOrderMasterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceDetail",
                columns: table => new
                {
                    InvoiceDetailId = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    InvoiceId = table.Column<Guid>(nullable: false),
                    HsCode = table.Column<string>(maxLength: 16, nullable: true),
                    DescGoods = table.Column<string>(maxLength: 250, nullable: false),
                    ProductNo = table.Column<string>(maxLength: 50, nullable: false),
                    CountryOfOrigin = table.Column<string>(maxLength: 50, nullable: true),
                    Uom = table.Column<string>(maxLength: 3, nullable: false),
                    ActualQuantity = table.Column<double>(nullable: false),
                    InvoiceQuantity = table.Column<double>(nullable: false),
                    GrossWeight = table.Column<double>(nullable: true),
                    NetWeight = table.Column<double>(nullable: false),
                    IntrnlAgmt = table.Column<string>(maxLength: 10, nullable: true),
                    InvoiceNo = table.Column<string>(maxLength: 50, nullable: false),
                    InvoiceDate = table.Column<DateTime>(type: "date", nullable: false),
                    InvoiceAmount = table.Column<double>(nullable: false),
                    PkgType = table.Column<string>(maxLength: 10, nullable: false),
                    CommclDesc = table.Column<string>(maxLength: 240, nullable: true),
                    NumberOfPackages = table.Column<int>(nullable: false),
                    RecordStatusId = table.Column<byte>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<int>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceDetail", x => x.InvoiceDetailId);
                    table.ForeignKey(
                        name: "FK_InvoiceDetails_Invoices",
                        column: x => x.InvoiceId,
                        principalTable: "Invoice",
                        principalColumn: "InvoiceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Company_CreatedBy",
                table: "Company",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Company_DeletedBy",
                table: "Company",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Company_ModifiedBy",
                table: "Company",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Company_RecordStatusId",
                table: "Company",
                column: "RecordStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_CreatedBy",
                table: "Customer",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_DeletedBy",
                table: "Customer",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_ModifiedBy",
                table: "Customer",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_RecordStatusId",
                table: "Customer",
                column: "RecordStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_WorkOrderMasterId",
                table: "Invoice",
                column: "WorkOrderMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceDetail_InvoiceId",
                table: "InvoiceDetail",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_CreatedBy",
                table: "Product",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Product_CustomerId",
                table: "Product",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_DeletedBy",
                table: "Product",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ModifiedBy",
                table: "Product",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Product_RecordStatusId",
                table: "Product",
                column: "RecordStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_User_CreatedBy",
                table: "User",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_User_DeletedBy",
                table: "User",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_User_ModifiedBy",
                table: "User",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_User_RecordStatusId",
                table: "User",
                column: "RecordStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrderMaster_CreatedBy",
                table: "WorkOrderMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrderMaster_CustomerId",
                table: "WorkOrderMaster",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrderMaster_DeletedBy",
                table: "WorkOrderMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrderMaster_ModifiedBy",
                table: "WorkOrderMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrderMaster_RecordStatusId",
                table: "WorkOrderMaster",
                column: "RecordStatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropTable(
                name: "InvoiceDetail");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Invoice");

            migrationBuilder.DropTable(
                name: "WorkOrderMaster");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "RecordStatus");
        }
    }
}
