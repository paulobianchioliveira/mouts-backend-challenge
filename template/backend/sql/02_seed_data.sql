-- =====================================================
-- Sample Data Seeding Script for PostgreSQL
-- Ambev Developer Evaluation - Backend Challenge
-- =====================================================

-- =====================================================
-- Seed Sample Products
-- =====================================================
INSERT INTO "Products" ("Id", "Name", "Description", "UnitPrice", "Category", "IsActive")
VALUES 
    (gen_random_uuid(), 'Brahma Chopp 350ml', 'Cerveja Brahma Chopp Lata 350ml', 3.50, 'Cervejas', true),
    (gen_random_uuid(), 'Antarctica Sub Zero 350ml', 'Cerveja Antarctica Sub Zero Lata 350ml', 3.80, 'Cervejas', true),
    (gen_random_uuid(), 'Skol Beats 350ml', 'Skol Beats Senses Lata 350ml', 4.20, 'Cervejas', true),
    (gen_random_uuid(), 'Guaraná Antarctica 2L', 'Refrigerante Guaraná Antarctica 2 Litros', 6.50, 'Refrigerantes', true),
    (gen_random_uuid(), 'Pepsi 2L', 'Refrigerante Pepsi Cola 2 Litros', 5.90, 'Refrigerantes', true),
    (gen_random_uuid(), 'H2OH Limão 500ml', 'Bebida H2OH Limão 500ml', 3.20, 'Sucos', true),
    (gen_random_uuid(), 'Água Tônica Antarctica 350ml', 'Água Tônica Antarctica Lata 350ml', 3.00, 'Outros', true),
    (gen_random_uuid(), 'Stella Artois 330ml', 'Cerveja Stella Artois Long Neck 330ml', 5.50, 'Cervejas', true),
    (gen_random_uuid(), 'Corona Extra 355ml', 'Cerveja Corona Extra Long Neck 355ml', 6.80, 'Cervejas', true),
    (gen_random_uuid(), 'Budweiser 350ml', 'Cerveja Budweiser Lata 350ml', 4.50, 'Cervejas', true)
ON CONFLICT DO NOTHING;

-- =====================================================
-- Seed Sample Customers
-- =====================================================
INSERT INTO "Customers" ("Id", "Name", "Email", "Phone", "Document", "Address", "IsActive")
VALUES 
    (gen_random_uuid(), 'João Silva', 'joao.silva@email.com', '11987654321', '12345678901', 'Rua das Flores, 123 - São Paulo/SP', true),
    (gen_random_uuid(), 'Maria Santos', 'maria.santos@email.com', '11976543210', '23456789012', 'Av. Paulista, 1000 - São Paulo/SP', true),
    (gen_random_uuid(), 'Pedro Oliveira', 'pedro.oliveira@email.com', '21987654321', '34567890123', 'Rua do Comércio, 456 - Rio de Janeiro/RJ', true),
    (gen_random_uuid(), 'Ana Costa', 'ana.costa@email.com', '31987654321', '45678901234', 'Av. Afonso Pena, 789 - Belo Horizonte/MG', true),
    (gen_random_uuid(), 'Carlos Souza', 'carlos.souza@email.com', '41987654321', '56789012345', 'Rua XV de Novembro, 321 - Curitiba/PR', true)
ON CONFLICT ("Document") DO NOTHING;

-- =====================================================
-- Seed Sample Branches
-- =====================================================
INSERT INTO "Branches" ("Id", "Name", "Code", "Address", "City", "State", "PostalCode", "ManagerName", "IsActive")
VALUES 
    (gen_random_uuid(), 'Filial São Paulo Centro', 'SP-001', 'Av. São João, 500', 'São Paulo', 'SP', '01035-000', 'Roberto Almeida', true),
    (gen_random_uuid(), 'Filial São Paulo Zona Sul', 'SP-002', 'Av. Santo Amaro, 1200', 'São Paulo', 'SP', '04505-002', 'Fernanda Lima', true),
    (gen_random_uuid(), 'Filial Rio de Janeiro', 'RJ-001', 'Av. Rio Branco, 200', 'Rio de Janeiro', 'RJ', '20040-009', 'Marcelo Santos', true),
    (gen_random_uuid(), 'Filial Belo Horizonte', 'MG-001', 'Av. Amazonas, 1500', 'Belo Horizonte', 'MG', '30180-001', 'Juliana Rodrigues', true),
    (gen_random_uuid(), 'Filial Curitiba', 'PR-001', 'Rua Marechal Deodoro, 630', 'Curitiba', 'PR', '80010-010', 'André Pereira', true)
ON CONFLICT ("Code") DO NOTHING;

-- =====================================================
-- Seed Sample User (for testing)
-- Note: Password should be hashed in production
-- =====================================================
INSERT INTO "Users" ("Id", "Username", "Password", "Phone", "Email", "Status", "Role")
VALUES 
    (gen_random_uuid(), 'admin', 'hashed_password_here', '11999999999', 'admin@ambev.com', 'Active', 'Admin'),
    (gen_random_uuid(), 'manager', 'hashed_password_here', '11988888888', 'manager@ambev.com', 'Active', 'Manager'),
    (gen_random_uuid(), 'user', 'hashed_password_here', '11977777777', 'user@ambev.com', 'Active', 'User')
ON CONFLICT DO NOTHING;

-- =====================================================
-- Seed Data Complete
-- =====================================================    