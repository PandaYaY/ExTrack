--liquibase formatted sql

--changeset agalimianov:1
create table if not exists public.checks
(
    id           serial primary key not null,
    user_id      integer            not null,
    date         timestamp          not null,
    shop_id      integer            not null,
    shop_address text               not null,
    sum          double precision   not null,

    constraint fk_check_user foreign key (user_id) references public.users (id) on delete cascade on update cascade,
    constraint fk_check_shop foreign key (shop_id) references public.shops (id) on delete cascade on update cascade
);
--rollback ;
