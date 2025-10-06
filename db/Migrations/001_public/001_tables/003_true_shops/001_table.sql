--liquibase formatted sql

--changeset agalimianov:1
create table if not exists public.true_shops
(
    id   integer primary key not null,
    name text unique         not null
);
--rollback ;

--changeset agalimianov:2
create sequence if not exists public.true_shops_id_seq;
alter table public.true_shops
    alter column id set default nextval('public.true_shops_id_seq');
alter sequence public.true_shops_id_seq owned by public.true_shops.id;
--rollback ;
