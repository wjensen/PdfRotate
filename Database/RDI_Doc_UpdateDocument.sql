USE [RDI_Development]
GO
/****** Object:  StoredProcedure [dbo].[RDI_Doc_UpdateDocument]    Script Date: 12/27/2013 11:43:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[RDI_Doc_UpdateDocument]
       @Doc varbinary(max),
       @Id	 BIGINT
       
AS

DECLARE @DocID    int

BEGIN

  UPDATE DOCS SET DOC = @DOC WHERE DOC_ID = @Id
 
  
END

use [RDI_Development]
GO
GRANT EXECUTE ON [dbo].[RDI_Doc_UpdateDocument] TO [RDI_Admin]
GO
use [RDI_Development]
GO
GRANT EXECUTE ON [dbo].[RDI_Doc_UpdateDocument] TO [RDI_Branch_Manager]
GO
use [RDI_Development]
GO
GRANT EXECUTE ON [dbo].[RDI_Doc_UpdateDocument] TO [RDI_Clients]
GO
use [RDI_Development]
GO
GRANT EXECUTE ON [dbo].[RDI_Doc_UpdateDocument] TO [RDI_Employee]
GO
use [RDI_Development]
GO
GRANT EXECUTE ON [dbo].[RDI_Doc_UpdateDocument] TO [RDI_Project_Manager]
GO
