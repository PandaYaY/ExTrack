--liquibase formatted sql

--changeset agalimianov:1
create index if not exists idx_users_email on public.users (email);
--rollback ;
