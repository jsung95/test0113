Oracle.ManagedDataAccess.Core NuGet Package 2.19.130 README
===========================================================
Release Notes: Oracle Data Provider for .NET Core

October 2021

This document provides information that supplements the Oracle Data Provider for .NET (ODP.NET) documentation.

You have downloaded Oracle Data Provider for .NET. The license agreement is available here:
https://www.oracle.com/downloads/licenses/distribution-license.html


New Features
============
One-way TLS/SSL with Built-in Trustpoints (Walletless)
  ODP.NET core and managed providers now support one-way Transport Layer Security/Secure Sockets Layer 
  without wallets.  Not having to provide a wallet can simplify database connectivity, 
  such as with Oracle Autonomous Database.


Bug Fixes since Oracle.ManagedDataAccess.Core NuGet Package 2.19.120
====================================================================
Bug 30750461 - ORACLELOGICALTRANSACTION OBJECTS ACCUMULATE
Bug 32190103 - ORACLEBLOB.READ() CAUSES "SOURCE ARRAY WAS NOT LONG ENOUGH" EXCEPTION
Bug 33117071 - ORA-01013 USER CANCEL REQUEST ERROR OCCURS WHEN USING ORACLEBULKCOPY
Bug 32853612 - ARRAY BINDING INSERT INTO NVARCHAR2 AND NCLOB COLUMNS CAUESES MISSING DATA
Bug 31643024 - ORACLECOMMANDBUILDER.DERIVEPARAMETERS THROW INDEXOUTOFRANGEEXCEPTION
Bug 32702522 - BULKCOPY INSERTS UNICODE DATA AS INVALID DATA INTO THE NCHAR / NVARCHAR2 COLUMN
Bug 33402710 - INTEGER BASED PARTS OF GUID/RAW(16) ARE INCORRECTLY REVERSED

 Copyright (c) 2021, Oracle and/or its affiliates. 
