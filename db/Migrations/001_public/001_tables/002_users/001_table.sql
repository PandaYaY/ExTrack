--liquibase formatted sql

--changeset agalimianov:1
create table if not exists public.users
(
    id            serial primary key not null,
    role_id       smallint           not null default 2,
    name          text               not null,
    email         text unique        not null,
    hash_password text               not null,

    constraint fk_users_roles foreign key (role_id) references public.roles (id) on delete set default on update cascade
);
--rollback ;
