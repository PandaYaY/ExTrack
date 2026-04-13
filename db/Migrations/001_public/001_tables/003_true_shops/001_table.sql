--liquibase formatted sql

--changeset agalimianov:1
create table if not exists public.true_shops
(
    id   serial primary key not null,
    name text unique        not null
);
--rollback ;
