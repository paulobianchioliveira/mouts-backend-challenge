# Database SQL Scripts

This folder contains PostgreSQL SQL scripts for the Ambev Developer Evaluation database.

## Scripts

### 01_create_tables.sql
Creates all database tables with proper constraints, indexes, and relationships:
- Users
- Products
- Customers
- Branches
- Sales
- SaleItems

**Usage:**

## Execution Order

1. First run `01_create_tables.sql` to create the schema
2. Then run `02_seed_data.sql` to populate with sample data (optional)
3. Only run `03_drop_tables.sql` when you need to clean up everything

## Connection String

Make sure your `appsettings.json` has the correct connection string:

## Notes

- All tables use UUID (GUID) as primary keys
- Foreign key constraints use RESTRICT for data integrity
- Indexes are created for frequently queried columns
- The pgcrypto extension is required for `gen_random_uuid()`