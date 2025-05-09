--liquibase formatted sql

--changeset agalimianov:1
create table if not exists public.true_products
(
    id   integer primary key not null,
    name text unique         not null
);
--rollback drop table if exists public.true_products;
