﻿ALTER DATABASE [$(DatabaseName)]
    ADD FILE (NAME = [Students], FILENAME = 'E:\DB\Students.mdf', SIZE = 72704 KB, FILEGROWTH = 1024 KB) TO FILEGROUP [PRIMARY];

