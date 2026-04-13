--liquibase formatted sql

--changeset agalimianov:1
create table if not exists public.operation_types
(
    id   smallint primary key not null,
    name text unique          not null
);
--rollback drop table if exists public.operation_types;
