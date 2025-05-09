--liquibase formatted sql

--changeset agalimianov:1
create table if not exists public.checks
(
    id      integer primary key not null,
    user_id integer             not null,
    date    timestamp           not null,
    shop_id integer             not null,
    sum     double precision    not null,

    constraint fk_check_user foreign key (user_id) references public.users (id),
    constraint fk_check_shop foreign key (shop_id) references public.shops (id)
);
--rollback drop table if exists public.checks;
