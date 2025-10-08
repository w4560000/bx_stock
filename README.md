# BX_Stock

> BX_Stock project


- 建立developer帳號
```
USE [master];

CREATE LOGIN [leo] WITH PASSWORD = '123456';
CREATE USER [leo] FOR LOGIN [leo];
ALTER SERVER ROLE [sysadmin] ADD MEMBER [leo];
```

- 安裝 ef tool (僅首次需安裝)

```
dotnet tool install --global dotnet-ef

dotnet ef database update
```





- 當DB Table 更新回Model

```
dotnet ef dbcontext scaffold "Server=.;Database=Stock;User ID=leo;Password=123456;" Microsoft.EntityFrameworkCore.SqlServer -o "Models/Entity" -f
```