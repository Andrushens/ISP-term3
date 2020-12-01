# 3 лаба
## Описание 
  3 Лаба представляет из себя класс для мониторинга директорий(2 лаба) + переносимая/изменяемая система конфигурации сервиса в формате XML И Json.

## Архитектура
  1) Класс FileWatcher - класс для отслеживания добавления новых файлов в директорию для их дальнейшего шифрования/архивирования/разархивирования/расшифрования.
  
  2) Класс FileWatcherOptions - Представляет класс общих настроек для FileWatcher'а и содержит в себе объекты всех других классов конфигураций(StorageOptions - информация о Source/Target папках и CryptographyOptions - информация для шифрования).
  
  3) Класс ConfigManager - на основе выбранного парсера(XmlParser или JsonParser, реализующие один интерфейс IConfigParser) парсит .xml или .json файлы соответственно. Имеет только один метод T GetOptions<<T>T>(configFilePath). Если в итоге от него был получен нулевой указатель, настройки устанавливаются по умолчанию.
 
  4) Классы XmlParser и JsonParser - для десериализации файлов config.xml или appsettings.json соответственно.
  
 ## Дополнительно
 Реализована возможность доставать конфигурацию кусками, получая отдельный класс только выбранной конфигурации. Также имеется возможность валидации с использованием config.xsd.
