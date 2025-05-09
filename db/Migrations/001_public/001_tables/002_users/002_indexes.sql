--liquibase formatted sql

--changeset agalimianov:1
create index if not exists idx_users_login on public.users (login);
--rollback drop index if exists idx_users_login;
