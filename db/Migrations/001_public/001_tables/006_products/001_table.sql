--liquibase formatted sql

--changeset agalimianov:1
create table if not exists public.products
(
    id              integer primary key not null,
    name            text unique         not null,
    true_product_id integer,

    constraint fk_products_true_product_id foreign key (true_product_id) references public.true_products (id)
);
--rollback drop table if exists public.products;
