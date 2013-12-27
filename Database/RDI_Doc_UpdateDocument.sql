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