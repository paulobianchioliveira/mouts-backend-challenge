-- =====================================================
-- Drop Tables Script for PostgreSQL
-- Ambev Developer Evaluation - Backend Challenge
-- WARNING: This will delete ALL data!
-- =====================================================

-- Drop tables in reverse order of dependencies
DROP TABLE IF EXISTS "SaleItems" CASCADE;
DROP TABLE IF EXISTS "Sales" CASCADE;
DROP TABLE IF EXISTS "Branches" CASCADE;
DROP TABLE IF EXISTS "Customers" CASCADE;
DROP TABLE IF EXISTS "Products" CASCADE;
DROP TABLE IF EXISTS "Users" CASCADE;

-- =====================================================
-- Tables Dropped Successfully
-- =====================================================