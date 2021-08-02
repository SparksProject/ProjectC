﻿CREATE view [dbo].[vw_WsWorkOrderMaster]
as
select 
	cikis.StokCikisId,cikis.InvoiceId as WorkOrderMasterId,cikis.ReferansNo as WorkOrderNo,'EX' as DeclarationType,c.UserNameWs,c.PasswordWs
from ChepStokCikis cikis
JOIN Customer c on cikis.IhracatciFirma=c.CustomerId