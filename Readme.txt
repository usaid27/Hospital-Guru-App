Use Package manager to run following command

1. select default project : HMS.DataContext then run this 
	- Add-Migration InitialCreate -context AuthDbContext -OutputDir Migrations

	- Add-Migration InitialCreate -context AppDbContext -OutputDir Migrations/App

	- Update-Database
	note : make sure to install "Microsoft.EntityFrameworkCore.Design" and
		   "Microsoft.EntityFrameworkCore.SqlServer"
		   in HMS.DataContext project.
		   also check ConnectionString in appsetting.json file

