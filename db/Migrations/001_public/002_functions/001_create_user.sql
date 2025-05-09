--liquibase formatted sql

--changeset agalimianov:1
create or replace function public.create_user(p_login text, p_password_hash text, p_role_id smallint default 2) returns integer as
'
declare
    user_id integer;
begin
    -- Вставляем нового пользователя и возвращаем его ID
    insert into public.users (role_id, login, hash_password)
    values (p_role_id, p_login, p_password_hash)
    returning id into user_id;

    return user_id;
exception
    when unique_violation then
        raise exception ''Пользователь с логином "%" уже существует'', p_login;
    when foreign_key_violation then
        raise exception ''Указанная роль (%) не существует'', p_role_id;
end;
' language plpgsql;
--rollback drop function if exists public.create_user(text, text, smallint);
