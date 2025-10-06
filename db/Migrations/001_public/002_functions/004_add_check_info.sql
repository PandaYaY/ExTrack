--liquibase formatted sql

--changeset agalimianov:1 runOnChange:true
create or replace function public.add_check_info(in_user_id int, in_obj jsonb) returns integer as
'
    declare
        -- check
        out_check_id           integer;
        in_shop_name           text;
        in_shop_id             integer;
        in_shop_address        text;
        in_check_date          timestamp;
        in_total_sum           double precision;
        in_products            jsonb;
        -- product
        in_product             jsonb;
        in_product_name        text;
        in_product_id          integer;
        in_product_price       double precision;
        in_product_quantity    double precision;
        in_product_total_price double precision;
    begin
        in_shop_name = in_obj ->> ''shop_name'';
        in_shop_address = in_obj ->> ''shop_address'';
        in_check_date = (in_obj ->> ''check_date'')::timestamp;
        in_total_sum = (in_obj ->> ''total_sum'')::double precision;
        in_products = in_obj -> ''products'';

        in_shop_id = public.upsert_shop(in_shop_name);
        insert into public.checks (user_id, date, shop_id, shop_address, sum)
        values (in_user_id, in_check_date, in_shop_id, in_shop_address, in_total_sum)
        returning id into out_check_id;

        for in_product in select *
                          from jsonb_array_elements(in_products)
            loop
                in_product_name = in_product ->> ''name'';
                in_product_price = (in_product ->> ''price'')::double precision;
                in_product_quantity = (in_product ->> ''quantity'')::double precision;
                in_product_total_price = (in_product ->> ''total_price'')::double precision;

                in_product_id = public.upsert_product(in_product_name);

                insert into public.check_products (check_id, product_id, price, quantity, total_price)
                values (out_check_id, in_product_id, in_product_price, in_product_quantity, in_product_total_price);
            end loop;

        return out_check_id;
    end;
' language plpgsql;
--rollback drop function if exists public.upsert_shop(text);
