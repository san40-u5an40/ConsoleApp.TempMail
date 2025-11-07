# Структура решения ConsoleApp.TempMail
- [TempMailClient](https://github.com/san40-u5an40/ConsoleApp.TempMail/tree/main/src#tempmailclient)
    - [Назначение](https://github.com/san40-u5an40/ConsoleApp.TempMail/tree/main/src#%D0%BD%D0%B0%D0%B7%D0%BD%D0%B0%D1%87%D0%B5%D0%BD%D0%B8%D0%B5)
    - [Структура](https://github.com/san40-u5an40/ConsoleApp.TempMail/tree/main/src#%D1%81%D1%82%D1%80%D1%83%D0%BA%D1%82%D1%83%D1%80%D0%B0)
    - [Примеры использования](https://github.com/san40-u5an40/ConsoleApp.TempMail/tree/main/src#%D0%BF%D1%80%D0%B8%D0%BC%D0%B5%D1%80%D1%8B-%D0%B8%D1%81%D0%BF%D0%BE%D0%BB%D1%8C%D0%B7%D0%BE%D0%B2%D0%B0%D0%BD%D0%B8%D1%8F)
    - [Используемые NuGet-пакеты](https://github.com/san40-u5an40/ConsoleApp.TempMail/tree/main/src#%D0%B8%D1%81%D0%BF%D0%BE%D0%BB%D1%8C%D0%B7%D1%83%D0%B5%D0%BC%D1%8B%D0%B5-nuget-%D0%BF%D0%B0%D0%BA%D0%B5%D1%82%D1%8B)
- [TempMailApp](https://github.com/san40-u5an40/ConsoleApp.TempMail/tree/main/src#tempmailapp)
    - [Назначение](https://github.com/san40-u5an40/ConsoleApp.TempMail/tree/main/src#%D0%BD%D0%B0%D0%B7%D0%BD%D0%B0%D1%87%D0%B5%D0%BD%D0%B8%D0%B5-1)
    - [Структура](https://github.com/san40-u5an40/ConsoleApp.TempMail/tree/main/src#%D1%81%D1%82%D1%80%D1%83%D0%BA%D1%82%D1%83%D1%80%D0%B0-1)

## TempMailClient

### Назначение
Библиотека классов, предназначенная для взаимодействия с API сервиса `mail.gw`.

### Структура
Аргументы конструктора:
 - HttpClientHandler (optional) - Для установления дополнительной логики обмена пакетами между клиентом и сервером.
 - Bool (optional) - Указывает необходимо ли освободить ресурсы объекта HttpClientHandler, после освобождения ресурсов клиента.

Свойства (возвращают данные, если был произведён вход):
 - Token - Возвращает токен пользователя.
 - AccountInfo - Возвращает информацию об аккаунте.
 - MessageList - Возвращает список сообщений. Для его обновления необходимо использовать соответствующий метод.

Публичные методы:
 - CreateAccountAsync(string address, string password) - Создаёт аккаунт временной почты. Возвращает кортеж с индикатором успеха и сообщением об ошибке (при неудаче).
 - DeleteAccountAsync() - Удаляет аккаунт временной почты, в который был осуществлён вход. Возвращает кортеж с индикатором успеха и сообщением об ошибке (при неудаче).
 - DeleteMessageAsync(string id) - Удаляет сообщение по id, если был произведён вход в аккаунт. Возвращает кортеж с индикатором успеха и сообщением об ошибке (при неудаче).
 - GetDomainsAsync() - Получает по API список доступных доменов для создания учётной записи пользователя. Возвращает кортеж с индикатором успеха, коллекцией доменов (при успехе) и сообщением об ошибке (при неудаче).
 - LoginAsync(string address, string password) - Осуществляет вход в учётную запись. Возвращает кортеж с индикатором успеха и сообщением об ошибке (при неудаче).
 - Logout() - Выходит из учётной записи.
 - UpdateMessagesAsync() - Обновляет список сообщений, если был произведён вход в аккаунт. Возвращает кортеж с индикатором успеха и сообщением об ошибке (при неудаче).

Все классы, хранимые и возвращаемые клиентом временной почты определены в пространстве имён `TempMailClient.ApiData` и находятся в соответствующей папке.

### Примеры использования
```C#
var resultGetDomains = await mailClient.GetDomainsAsync();
if (!resultGetDomains.IsSuccess)
{
    Console.WriteLine(resultGetDomains.ErrorMessage!);
    return;
}
```

### Используемые NuGet-пакеты
В данном проекте используются авторские пакеты:
 - [san40_u5an40.ConsoleDisplayFramework](https://www.nuget.org/packages/san40_u5an40.ConsoleDisplayFramework) - Фреймворк для быстрой разработки консольных дисплеев.
 - [san40_u5an40.ExtraLib](https://www.nuget.org/packages/san40_u5an40.ExtraLib) - Библиотека дополнительных классов, полезных в широком круге задач.
 
## TempMailApp

### Назначение
Приложение, определяющее логику взаимодействия пользователя с клиентом временной почты посредством консольного интерфейса.

### Структура
В пространстве имён `TempMailApp.FrameworkControllersExtension` определены дополнительные контроллеры для вывода пользовательского дисплея:
 - ControllerMessagesMenu - Контроллер, определяющий взаимодействие пользователя со списком сообщений.
 - ControllerMessageOpened - Контроллер, определяющий взаимодействие пользователя с открытым сообщением (принцип работы схож с `ControllerPressEnter`).

В пространстве имён `TempMailApp.Core` в классе `App` определен основной ход программы:
 - App - Определение конструктора и основных полей класса.
 - StartAsync - Точка входа программы, здесь определена логика проверки аргументов и соответствующих особенностей старта программы.
 - StartMenuAsync - Стартовое меню, которое показывается при запуске программы без аргументов. Данное меню содержит пункты "Войти в существующий аккаунт", "Создать новый" и "Выйти из приложения".
 - CreateAccountMenuAsync - Меню создания нового аккаунта. Для этого пользователю необходимо выбрать доступный домен, ввести имя пользователя и пароль.
 - LoginMenuAsync - Меню входа в существующий аккаунт.
 - MessagesMenuAsync - Меню сообщений.
 - MessageOpenedAsync - Меню одного открытого сообщения.
 - InfoAsync - Информационное окно, необходимое для уведомления пользователя об ошибках выполнения программы.