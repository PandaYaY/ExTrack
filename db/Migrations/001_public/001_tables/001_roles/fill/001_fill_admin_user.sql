--liquibase formatted sql

--changeset agalimianov:1
insert into public.roles (id, name)
values (1, 'Admin'),
       (2, 'User')
on conflict (id) do nothing;
--rollback ;
