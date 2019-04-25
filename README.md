FL2019-Server


To do migrations in package manager run:

`Add-Migration name`

Then run
`Update-Database`


To update the database to the current set run:

`Scaffold-DbContext "Server=fl2019.database.windows.net;Database=FLDB;Trusted_Connection=False;Encrypt=True;uid=FLDbLogin;password=P7QrZ)s#xnZTE(q7" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models`
