--liquibase formatted sql

--changeset agalimianov:1
create table if not exists public.shops
(
    id           integer primary key not null,
    name         text unique         not null,
    true_shop_id integer,
    
    constraint fk_true_shops_names foreign key (true_shop_id) references public.true_shops (id)
);
--rollback drop table if exists public.shops;

--changeset agalimianov:2
create sequence if not exists public.shops_id_seq;
alter table public.shops alter column id set default nextval('public.shops_id_seq');
alter sequence public.shops_id_seq owned by public.shops.id;
--rollback drop sequence public.shops_id_seq cascade;
