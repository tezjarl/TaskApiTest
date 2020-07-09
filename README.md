# TaskApiTest

Для запуска тестовой БД в докере необходимо выполнить
> docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=P@ssw0rd" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-CU3-ubuntu-18.04
# TODO(Nice to have)
* Добавить интеграционных тестов для апи
* Перевести на Net Core 3.1(после апгрейда ОС)
* Добавить docker-compose и dockerfile для сервиса
