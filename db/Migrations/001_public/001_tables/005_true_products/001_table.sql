--liquibase formatted sql

--changeset agalimianov:1
create table if not exists public.true_products
(
    id   integer primary key not null,
    name text unique         not null
);
--rollback ;

--changeset agalimianov:2
create sequence if not exists public.true_products_id_seq;
alter table public.true_products
    alter column id set default nextval('public.true_products_id_seq');
alter sequence public.true_products_id_seq owned by public.true_products.id;
--rollback ;
