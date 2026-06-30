-- =====================================================
-- Database Schema Creation Script for PostgreSQL
-- Ambev Developer Evaluation - Backend Challenge
-- =====================================================

-- Enable UUID extension if not already enabled
CREATE EXTENSION IF NOT EXISTS "pgcrypto";

-- =====================================================
-- Table: Users
-- Description: Stores user information
-- =====================================================
CREATE TABLE IF NOT EXISTS "Users" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "Username" VARCHAR(50) NOT NULL,
    "Password" VARCHAR(100) NOT NULL,
    "Phone" VARCHAR(20) NOT NULL,
    "Email" VARCHAR(100) NOT NULL,
    "Status" VARCHAR(20) NOT NULL,
    "Role" VARCHAR(20) NOT NULL,
    "UpdatedAt" Date,
	"CreatedAt" Date
);

-- =====================================================
-- Table: Products
-- Description: Stores product information
-- =====================================================
CREATE TABLE IF NOT EXISTS "Products" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "Name" VARCHAR(200) NOT NULL,
    "Description" VARCHAR(1000),
    "UnitPrice" DECIMAL(18,2) NOT NULL,
    "Category" VARCHAR(100) NOT NULL,
    "IsActive" BOOLEAN NOT NULL DEFAULT true
);

-- =====================================================
-- Table: Customers
-- Description: Stores customer information
-- =====================================================
CREATE TABLE IF NOT EXISTS "Customers" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "Name" VARCHAR(200) NOT NULL,
    "Email" VARCHAR(100) NOT NULL,
    "Phone" VARCHAR(20) NOT NULL,
    "Document" VARCHAR(20) NOT NULL,
    "Address" VARCHAR(500),
    "IsActive" BOOLEAN NOT NULL DEFAULT true,
    CONSTRAINT "UQ_Customers_Document" UNIQUE ("Document")
);

-- =====================================================
-- Table: Branches
-- Description: Stores branch/store information
-- =====================================================
CREATE TABLE IF NOT EXISTS "Branches" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "Name" VARCHAR(200) NOT NULL,
    "Code" VARCHAR(50) NOT NULL,
    "Address" VARCHAR(500) NOT NULL,
    "City" VARCHAR(100) NOT NULL,
    "State" VARCHAR(50) NOT NULL,
    "PostalCode" VARCHAR(20) NOT NULL,
    "ManagerName" VARCHAR(200),
    "IsActive" BOOLEAN NOT NULL DEFAULT true,
    CONSTRAINT "UQ_Branches_Code" UNIQUE ("Code")
);

-- =====================================================
-- Table: Sales
-- Description: Stores sale transactions
-- =====================================================
CREATE TABLE IF NOT EXISTS "Sales" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "SaleNumber" VARCHAR(100) NOT NULL,
    "SaleDate" TIMESTAMP NOT NULL,
    "CustomerId" UUID NOT NULL,
    "CustomerName" VARCHAR(200) NOT NULL,
    "BranchId" UUID NOT NULL,
    "BranchName" VARCHAR(200) NOT NULL,
    "TotalAmount" DECIMAL(18,2) NOT NULL,
    "TotalDiscount" DECIMAL(18,2) NOT NULL,
    "Status" VARCHAR(20) NOT NULL,
    "CancellationReason" VARCHAR(500),
    "CancelledAt" TIMESTAMP,
    CONSTRAINT "FK_Sales_Customers" FOREIGN KEY ("CustomerId") REFERENCES "Customers"("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Sales_Branches" FOREIGN KEY ("BranchId") REFERENCES "Branches"("Id") ON DELETE RESTRICT,
    CONSTRAINT "UQ_Sales_SaleNumber" UNIQUE ("SaleNumber")
);

-- =====================================================
-- Table: SaleItems
-- Description: Stores individual items within sales
-- =====================================================
CREATE TABLE IF NOT EXISTS "SaleItems" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "SaleId" UUID NOT NULL,
    "ProductId" UUID NOT NULL,
    "ProductName" VARCHAR(200) NOT NULL,
    "Quantity" INTEGER NOT NULL,
    "UnitPrice" DECIMAL(18,2) NOT NULL,
    "DiscountPercentage" DECIMAL(5,2) NOT NULL,
    "DiscountAmount" DECIMAL(18,2) NOT NULL,
    "TotalAmount" DECIMAL(18,2) NOT NULL,
    "IsCancelled" BOOLEAN NOT NULL DEFAULT false,
    "CancellationReason" VARCHAR(500),
    "CancelledAt" TIMESTAMP,
    CONSTRAINT "FK_SaleItems_Sales" FOREIGN KEY ("SaleId") REFERENCES "Sales"("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_SaleItems_Products" FOREIGN KEY ("ProductId") REFERENCES "Products"("Id") ON DELETE RESTRICT
);

-- =====================================================
-- Indexes for Performance Optimization
-- =====================================================

-- Users indexes
CREATE INDEX IF NOT EXISTS "IX_Users_Email" ON "Users"("Email");
CREATE INDEX IF NOT EXISTS "IX_Users_Username" ON "Users"("Username");

-- Customers indexes
CREATE INDEX IF NOT EXISTS "IX_Customers_Email" ON "Customers"("Email");

-- Sales indexes
CREATE INDEX IF NOT EXISTS "IX_Sales_SaleDate" ON "Sales"("SaleDate");
CREATE INDEX IF NOT EXISTS "IX_Sales_CustomerId" ON "Sales"("CustomerId");
CREATE INDEX IF NOT EXISTS "IX_Sales_BranchId" ON "Sales"("BranchId");
CREATE INDEX IF NOT EXISTS "IX_Sales_Status" ON "Sales"("Status");

-- SaleItems indexes
CREATE INDEX IF NOT EXISTS "IX_SaleItems_SaleId" ON "SaleItems"("SaleId");
CREATE INDEX IF NOT EXISTS "IX_SaleItems_ProductId" ON "SaleItems"("ProductId");

-- =====================================================
-- Comments for Documentation
-- =====================================================

COMMENT ON TABLE "Users" IS 'Stores user authentication and profile information';
COMMENT ON TABLE "Products" IS 'Product catalog with pricing and category information';
COMMENT ON TABLE "Customers" IS 'Customer master data following External Identities pattern';
COMMENT ON TABLE "Branches" IS 'Branch/Store locations information';
COMMENT ON TABLE "Sales" IS 'Sale transaction aggregate root with denormalized customer and branch data';
COMMENT ON TABLE "SaleItems" IS 'Individual line items within a sale with discount calculations';

COMMENT ON COLUMN "Sales"."SaleNumber" IS 'Unique business identifier for the sale (Format: SALE-YYYYMMDD-HHMMSS-GUID)';
COMMENT ON COLUMN "SaleItems"."DiscountPercentage" IS 'Applied discount percentage based on quantity rules: 4-9 items=10%, 10-20 items=20%';
COMMENT ON COLUMN "SaleItems"."Quantity" IS 'Item quantity (Valid range: 1-20 items per product)';

-- =====================================================
-- Script Execution Complete
-- =====================================================