ALTER DATABASE [$(DatabaseName)]
    ADD LOG FILE (NAME = [Students_log], FILENAME = 'F:\Logs\Students_log.ldf', SIZE = 88896 KB, MAXSIZE = 2097152 MB, FILEGROWTH = 10 %);

