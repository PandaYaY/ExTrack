--liquibase formatted sql

--changeset agalimianov:1
insert into public.operation_types(id, name)
values (1, 'Приход'),
       (2, 'Возврат прихода'),
       (3, 'Расход'),
       (4, 'Возврат расхода');
--rollback delete from public.operation_types where id in (1, 2, 3, 4);
