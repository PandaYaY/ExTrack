--liquibase formatted sql

--changeset agalimianov:1
create or replace function public.upsert_product(p_name text) returns integer as
'
    declare
        product_id integer;
    begin
        select id into product_id from public.products p
        where p.name = p_name;
        if not found then
            insert into public.products (name)
            values (p_name) returning id into product_id;
        end if;
        return product_id;
    end;
' language plpgsql;
--rollback drop function if exists public.upsert_product(text);
