--liquibase formatted sql

--changeset agalimianov:1
create index if not exists idx_products_name on public.products (name);
create index if not exists idx_products_true_product_id on public.products (true_product_id);
--rollback ;
