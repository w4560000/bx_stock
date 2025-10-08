# BX_Stock

> BX_Stock project


- �إ�developer�b��
```
USE [master];

CREATE LOGIN [leo] WITH PASSWORD = '123456';
CREATE USER [leo] FOR LOGIN [leo];
ALTER SERVER ROLE [sysadmin] ADD MEMBER [leo];
```

- �w�� ef tool (�ȭ����ݦw��)

```
dotnet tool install --global dotnet-ef

dotnet ef database update
```





- ��DB Table ��s�^Model

```
dotnet ef dbcontext scaffold "Server=.;Database=Stock;User ID=leo;Password=123456;" Microsoft.EntityFrameworkCore.SqlServer -o "Models/Entity" -f
```