USE HangfireExample;


IF OBJECT_ID('Hangfire.[Counter]', 'U') IS NOT NULL DROP TABLE Hangfire.[Counter];
IF OBJECT_ID('Hangfire.[AggregatedCounter]', 'U') IS NOT NULL DROP TABLE Hangfire.[AggregatedCounter];
IF OBJECT_ID('Hangfire.[Hash]', 'U') IS NOT NULL DROP TABLE Hangfire.[Hash];
IF OBJECT_ID('Hangfire.[State]', 'U') IS NOT NULL DROP TABLE Hangfire.[State];
IF OBJECT_ID('Hangfire.[Set]', 'U') IS NOT NULL DROP TABLE Hangfire.[Set];
IF OBJECT_ID('Hangfire.[Server]', 'U') IS NOT NULL DROP TABLE Hangfire.[Server];
IF OBJECT_ID('Hangfire.[Schema]', 'U') IS NOT NULL DROP TABLE Hangfire.[Schema];
IF OBJECT_ID('Hangfire.[List]', 'U') IS NOT NULL DROP TABLE Hangfire.[List];
IF OBJECT_ID('Hangfire.[JobQueue]', 'U') IS NOT NULL DROP TABLE Hangfire.[JobQueue];
IF OBJECT_ID('Hangfire.[JobParameter]', 'U') IS NOT NULL DROP TABLE Hangfire.[JobParameter];
IF OBJECT_ID('Hangfire.[Job]', 'U') IS NOT NULL DROP TABLE Hangfire.[Job];