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
--rollback drop table if exists public.checks;

--changeset agalimianov:2
alter table public.checks add column fiscal_storage_device_number bigint not null;
alter table public.checks add column fiscal_document_number bigint not null;
alter table public.checks add column document_fiscal_attribute bigint not null;
alter table public.checks add column operation_type smallint not null;
--rollback ;

--changeset agalimianov:3
alter table public.checks add constraint fk_checks_operation_type foreign key (operation_type) references public.operation_types (id);
--rollback alter table public.checks drop constraint fk_checks_operation_type;
