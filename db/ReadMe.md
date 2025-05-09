# База данных

___
**СУБД:** [PostgreSQL](https://www.postgresql.org/)
___

## Миграции

Расположены по пути `./Migrations`

Для миграций (далее changeSet-ы) требуется ПО [liquibase](https://docs.liquibase.com/home.html)

Чтобы liquibase было понятно где и как применять changeSet-ы,
в директории должен находиться файл `liquibase.properties`

```yaml
# Enter the path for your changelog file.
changeLogFile=db-changelog.yaml

# Enter the format for your changelog file.
liquibase.command.format=yaml

#### Enter the Target database 'url' information  ####
liquibase.command.url=jdbc:postgresql://localhost:5432/db_name

# Enter the username for your Target database.
liquibase.command.username=your_user

# Enter the password for your Target database.
liquibase.command.password=password

# For remote project locations, do not delete temporary project files
liquibase.command.keepTempFiles=true

# Logging Configuration
# logLevel controls the amount of logging information generated. If not set, the default logLevel is INFO.
# Valid values, from least amount of logging to most, are:
#   OFF, ERROR, WARN, INFO, DEBUG, TRACE, ALL
# If you are having problems, setting the logLevel to DEBUG and re-running the command can be helpful.
#logLevel=DEBUG

# The logFile property controls where logging messages are sent. If this is not set, then logging messages are
# displayed on the console. If this is set, then messages will be sent to a file with the given name.
# logFile: liquibase.log
```

Для запуска миграций требуется находиться в папке
`[projectDir]/db/Migrations`и выполнить команду

```bash
liquibase update
```

### Структура миграций
```
db/
└── Migrations/
    └── [changesets]
├── db-changelog.yaml
├── liquibase.properties
└── ReadMe.md
```

---
