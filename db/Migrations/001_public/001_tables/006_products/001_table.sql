--liquibase formatted sql

--changeset agalimianov:1
create table if not exists public.products
(
    id              integer primary key not null,
    name            text unique         not null,
    true_product_id integer,

    constraint fk_products_true_product_id foreign key (true_product_id) references public.true_products (id) on delete set null on update cascade
);
--rollback ;

--changeset agalimianov:2
create sequence if not exists public.products_id_seq;
alter table public.products
    alter column id set default nextval('public.products_id_seq');
alter sequence products_id_seq owned by public.products.id;
--rollback ;
