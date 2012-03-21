ALTER DATABASE [$(DatabaseName)]
    ADD LOG FILE (NAME = [Commencement_log], FILENAME = 'F:\Logs\Commencement_log.ldf', SIZE = 10944 KB, MAXSIZE = 2097152 MB, FILEGROWTH = 10 %);

