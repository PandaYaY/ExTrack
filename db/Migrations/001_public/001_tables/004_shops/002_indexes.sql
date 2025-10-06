--liquibase formatted sql

--changeset agalimianov:1
create index if not exists idx_shops_name on public.shops (name);
create index if not exists idx_shops_true_shop_id on public.shops (true_shop_id);
--rollback ;
