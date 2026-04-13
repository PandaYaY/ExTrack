--liquibase formatted sql

--changeset agalimianov:1
create table if not exists public.check_products
(
    id          serial primary key not null,
    check_id    integer            not null,
    product_id  integer            not null,
    price       double precision   not null,
    quantity    double precision   not null default 1,
    total_price double precision   not null,

    constraint fk_check_products_check foreign key (check_id) references public.checks (id) on delete cascade on update cascade,
    constraint fk_check_products_product foreign key (product_id) references public.products (id) on delete cascade on update cascade
);
--rollback ;
