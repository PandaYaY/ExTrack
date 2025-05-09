--liquibase formatted sql

--changeset agalimianov:1
create index if not exists idx_check_user_id on public.checks (user_id);
--rollback drop index if exists idx_check_user_id;

--changeset agalimianov:2
create index if not exists idx_check_date on public.checks (date);
--rollback drop index if exists idx_check_date;

--changeset agalimianov:3
create index if not exists idx_check_check_shop_id on public.checks (shop_id);
--rollback drop index if exists idx_check_check_shop_id;
