﻿ALTER DATABASE [$(DatabaseName)]
    ADD LOG FILE (NAME = [Catbert3_log], FILENAME = 'F:\Logs\Catbert3_log.ldf', SIZE = 1024 KB, MAXSIZE = 2097152 MB, FILEGROWTH = 10 %);

