USE CS2016B_8_Company;
GO

IF (OBJECT_ID('usp_CreateDepartment') IS NOT NULL)
  DROP PROCEDURE [usp_CreateDepartment]
GO

IF (OBJECT_ID('usp_UpdateDepartmentName') IS NOT NULL)
  DROP PROCEDURE [usp_UpdateDepartmentName]
GO

IF (OBJECT_ID('usp_DeleteDepartment') IS NOT NULL)
  DROP PROCEDURE [usp_DeleteDepartment]
GO

IF (OBJECT_ID('usp_GetDepartment') IS NOT NULL)
  DROP PROCEDURE [usp_GetDepartment]
GO

IF (OBJECT_ID('usp_GetAllDepartments') IS NOT NULL)
  DROP PROCEDURE [usp_GetAllDepartments]
GO

IF (OBJECT_ID('usp_UpdateDepartmentManager') IS NOT NULL)
  DROP PROCEDURE [usp_UpdateDepartmentManager]
GO

IF (OBJECT_ID('getEmpCount') IS NOT NULL)
  DROP FUNCTION [getEmpCount]
GO

--First Procedure CREATE A DEPARTMENT (DNAME, MGRSSN)
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [usp_CreateDepartment] 
	@DName nvarchar(50), 
	@MgrSSN numeric(9,0)

AS
IF exists(SELECT 'True' FROM Department WHERE MgrSSN = @MgrSSN)
BEGIN
THROW 50002, 'The Manager is already in assigned to other Dept.', 1;
END
ELSE IF exists(SELECT 'True' FROM Department WHERE DName = @Dname)
BEGIN 
THROW 50001, 'This Name is already exist.', 1;
END
ELSE
BEGIN

SET NOCOUNT ON;

	DECLARE @LastChangeDate datetime = GetDate() 
	DECLARE @DNumber int
	DECLARE @EmpCount int

	BEGIN TRANSACTION; 

	SET @DNumber = (SELECT COALESCE(MAX(DNumber), 0) + 1
       FROM Department WITH (UPDLOCK));
   
	SET @EmpCount = dbo.getEmpCount(@DNumber);

   --INSERTING AND CREATING A NEW DEPARTMENT
	INSERT INTO Department
	VALUES (@DName, @DNumber, @MgrSSN, @LastChangeDate, @EmpCount)

	COMMIT TRANSACTION;
END

GO

--A NEW PROCEDURE
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [usp_UpdateDepartmentName] 
	@DNumber int, 
	@DName nvarchar(50)

AS
--QUERY TO UPDATE DE NAME OF DEPARTMENT
BEGIN
	UPDATE Department
	SET DName = @DName
	WHERE DNumber = @DNumber
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--NEW PROCEDURE TO UPDATE DE MANAGER
CREATE PROCEDURE [usp_UpdateDepartmentManager] 
	@DNumber int, 
	@MgrSSN numeric(9,0)

AS

--IN ORDER TO CATCH THE EXCEPTION WE CREATED THIS CONDITIONS
IF exists(SELECT 'True' FROM Department WHERE MgrSSN = @MgrSSN)
BEGIN 
THROW 50004, 'manager was registered to nother department.', 1;
END
ELSE
--ELSE QUERY
BEGIN
	SET NOCOUNT ON;
	DECLARE @currentMgrSSN numeric(9,0)

	SELECT @currentMgrSSN = MgrSSN FROM Department 
	WHERE DNumber = @DNumber

	DECLARE @LastChangeDate datetime = getDate()
END

IF exists(SELECT 'True' FROM Employee WHERE SSN = @MgrSSN)
BEGIN TRY
	UPDATE Department SET MgrSSN = @MgrSSN, MgrStartDate
	= @LastChangeDate WHERE DNumber = @DNumber
	END TRY
	BEGIN CATCH
		THROW 50010, 'Something wrong occoured', 1;
	END CATCH
ELSE 
	THROW 50005, 'There isn´t a person with this SSN', 1;

BEGIN
IF exists(SELECT 'True' FROM Employee 
			WHERE Employee.SSN != @MgrSSN AND Dno = @DNumber 
			AND Employee.SuperSSN = @currentMgrSSN)

    UPDATE Employee SET SuperSSN = @MgrSSN 
	WHERE Employee.SuperSSN = @currentMgrSSN AND Dno = @DNumber
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--NEW PROCEDURE DELETE
CREATE PROCEDURE [usp_DeleteDepartment] 
	@DNumber int

AS
--THE EMPLOYEES OF A COMPANY DO NOT DISAPPEAR, SO THEN THEY'RE UPDATED TO NULL

BEGIN
UPDATE Employee SET Dno = null WHERE Employee.Dno = @DNumber 
END

BEGIN


--REST IS DELETED
	DELETE Works_on WHERE pno = (
		SELECT Pno FROM Project WHERE Project.DNum = @DNumber
	)
	DELETE Project WHERE DNum = @DNumber
	DELETE Dept_Locations WHERE DNUmber = @DNumber
	DELETE Department WHERE DNumber = @DNumber


END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--GET DEPARTMENT PROCEDURE
CREATE PROCEDURE [usp_GetDepartment] 
	@DNumber int

AS
--EMPLOYEES THAT ARE WORKING ON EACH DEPARTMENT 
SET NOCOUNT ON;
BEGIN
	SELECT DName, DNumber, EmpCount 
	FROM Department WHERE DNumber = @DNumber
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--GET ALL PROCEDURE
CREATE PROCEDURE [usp_GetAllDepartments]

AS
SET NOCOUNT ON;
--A PROCEDURE WITHOUT CONDITION (WHERE)
BEGIN
	SELECT DName, DNumber, EmpCount FROM Department
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--COUNTS EMPLOYEES
CREATE FUNCTION [getEmpCount]
(	
	@DNumber int --PARAM
)

RETURNS int
 
AS
--RETURNS QUERY
BEGIN
RETURN 
((SELECT count(SSN) from Department JOIN Employee on DNumber = Dno where Dno = @DNumber))
END

