USE [University-WikiSite]

/* Creating Roles */
INSERT INTO [Roles] (Id, Name)
	VALUES ('d905f4f9-4ff6-4164-b12d-9c5ce0b5b561','������������');
INSERT INTO [Roles] (Id, Name)
	VALUES ('4fc3a8a4-10f4-43c5-b4c8-c374768e8269','���������');
INSERT INTO [Roles] (Id, Name)
	VALUES ('b7b4c011-96ac-4be8-9466-e72bb8cf41e7','�������������');

/* Default user (if user gets deleted, article's editor becomes default user) */
INSERT INTO [Credentials] (Id, [Login], Password_Hash)
	VALUES ('b5b3d84c-f57d-4b66-aaa5-d82f78a6077d', '404', HASHBYTES('SHA2_512', 'plkubgfcs'));
INSERT INTO [Users] (Id, Credentials_Id, Nickname, About, Role_Id)
	VALUES ('61bbaea5-e7ea-432f-87a8-ccda7f3a842b', 'b5b3d84c-f57d-4b66-aaa5-d82f78a6077d', 
	        '���������� ��������', '�����-�� �� ��� ������� �������������, ���� ��� �� �������', 
			'd905f4f9-4ff6-4164-b12d-9c5ce0b5b561');