--liquibase formatted sql

--changeset agalimianov:1
create table if not exists public.check_products
(
    id         integer primary key not null,
    check_id   integer             not null,
    product_id integer             not null,
    price      double precision    not null,
    quantity   double precision    not null default 1,
    total_price      double precision    not null,

    constraint fk_check_products_check foreign key (check_id) references public.checks (id) on delete cascade on update cascade,
    constraint fk_check_products_product foreign key (product_id) references public.products (id) on delete cascade on update cascade
);
--rollback ;

--changeset agalimianov:2
create sequence if not exists public.check_products_id_seq;
alter table public.check_products
    alter column id set default nextval('public.check_products_id_seq');
alter sequence check_products_id_seq owned by public.check_products.id;
--rollback ;
