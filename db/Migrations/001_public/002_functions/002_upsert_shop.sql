--liquibase formatted sql

--changeset agalimianov:1
create or replace function public.upsert_shop(p_name text) returns integer as
'
    declare
        shop_id integer;
    begin
        select id into shop_id from public.shops s
        where s.name = p_name;
        if not found then
            insert into public.shops (name)
            values (p_name) returning id into shop_id;
        end if;
        return shop_id;
    end;
' language plpgsql;
--rollback drop function if exists public.upsert_shop(text);
