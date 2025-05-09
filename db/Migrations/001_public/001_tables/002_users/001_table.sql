--liquibase formatted sql

--changeset agalimianov:1
create table if not exists public.users
(
    id            int primary key not null,
    role_id       smallint default 2,
    login         text unique     not null,
    hash_password text            not null,

    constraint fk_users_roles foreign key (role_id) references public.roles (id) on delete set default on update cascade
);
--rollback drop table if exists public.users;

--changeset agalimianov:2
create sequence if not exists public.users_id_seq;
alter table public.users alter column id set default nextval('public.users_id_seq');
alter sequence users_id_seq owned by public.users.id;
--rollback drop sequence public.users_id_seq cascade;
