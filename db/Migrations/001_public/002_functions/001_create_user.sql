--liquibase formatted sql

--changeset agalimianov:1 runOnChange:true
drop function if exists create_user(text, text, smallint);
create or replace function public.create_user(p_email text, p_password_hash text, p_name text,
                                              p_role_id smallint default 2) returns integer as
'
    declare
        user_id integer;
    begin
        -- Вставляем нового пользователя и возвращаем его ID
        insert into public.users (role_id, email, hash_password, name)
        values (p_role_id, p_email, p_password_hash, p_name)
        returning id into user_id;

        return user_id;
    exception
        when unique_violation then
            raise exception ''Пользователь с логином "%" уже существует'', p_email;
        when foreign_key_violation then
            raise exception ''Указанная роль (%) не существует'', p_role_id;
    end;
' language plpgsql;
--rollback drop function if exists public.create_user(text, text, smallint);
