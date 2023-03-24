create table ClientsInfo
(
	id int identity (1,1) not null,
	surname nvarchar (20) not null,
	[name] nvarchar (10) not null,
	patronymic nvarchar (20) not null,
	phonenumber float,
	email nvarchar (30) not null
)

insert into ClientsInfo ([surname],[name],[patronymic],[phonenumber],[email]) values (
						'Ivanov', 'Ivan', 'Ivanovich', '9261454312', 'ivanivanov@mail.ru')
						
insert into ClientsInfo ([surname],[name],[patronymic],[phonenumber],[email]) values (
						'Petrov', 'Petr', 'Petrovich', '9295428946', 'petrpetrov@yandex.ru')

insert into ClientsInfo ([surname],[name],[patronymic],[phonenumber],[email]) values (
						'Sergeev', 'Sergey', 'Sergeevich', '9266542398', 'sergeysergeev@google.com')