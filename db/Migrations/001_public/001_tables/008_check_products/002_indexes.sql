--liquibase formatted sql

--changeset agalimianov:1
create index if not exists idx_check_products_check_id on public.check_products (check_id);
--rollback drop index if exists idx_check_products_check_id;

--changeset agalimianov:2
create index if not exists idx_check_products_product_id on public.check_products (product_id);
--rollback drop index if exists idx_check_products_product_id;
