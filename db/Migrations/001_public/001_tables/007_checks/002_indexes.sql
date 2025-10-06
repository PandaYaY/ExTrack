--liquibase formatted sql

--changeset agalimianov:1
create index if not exists idx_check_user_id on public.checks (user_id);
create index if not exists idx_check_date on public.checks (date);
create index if not exists idx_check_check_shop_id on public.checks (shop_id);
--rollback ;
