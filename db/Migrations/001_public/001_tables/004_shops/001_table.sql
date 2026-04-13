--liquibase formatted sql

--changeset agalimianov:1
create table if not exists public.shops
(
    id           serial primary key not null,
    name         text unique        not null,
    true_shop_id integer,

    constraint fk_true_shops_names foreign key (true_shop_id) references public.true_shops (id) on delete set null on update cascade
);
--rollback ;
