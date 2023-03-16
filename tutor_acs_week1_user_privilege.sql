-- pastikan bisa login ke system pakai sql server authentication
-- klik kanan di database engine > properties > security > centang SQL Server and Windows Authentication mode


-- sebelum membuat user, kita perlu membuat login terlebih dahulu
--syntaxnya 
CREATE LOGIN namalogin
WITH PASSWORD = 'password';



--ada 2 jenis login di sql server
--1. windows authentication
--2. SQL Server authentication

--kita bisa cek di SQL Server Management Studio (SSMS) login yang baru kita buat
-- di folder Security > Logins


--tiap user terasosiasi ke masing-masing database, 
--jadi kalau login X punya user di database toko roti, 
--belum tentu dia punya user di toko buku dalam satu server yg sama

-- minimal jika kita bermain dengan create user dan login akan ada 2 database yang kita pakai
-- yaitu master dan database yang kita tuju


--nama user harus sama dengan nama loginnya

--contoh mau buat user a1 dan a2 untuk db_tokoroti
CREATE LOGIN a1
WITH PASSWORD = 'a1';

CREATE LOGIN a2
WITH PASSWORD = 'a2';

--kita bisa membuat user yang passwordnya expired (harus diganti ketika dia pertama kali login)
CREATE LOGIN namauser
WITH PASSWORD = 'passworduser' MUST_CHANGE, CHECK_EXPIRATION = ON;

--jangan lupa use db untuk menandakan query di bawahnya di jalankan di database apa
USE db_tokoroti;

CREATE USER a1
FOR LOGIN a1;

CREATE USER a2
FOR LOGIN a2;

--untuk memberikan hak akses, ada 2 cara :
-- 1. Pakai syntax grant untuk hak akses masing2
GRANT namaprivilege ON namatabel to namauser
--contoh
GRANT SELECT ON BAHAN TO a1; 
--bisa INSERT, UPDATE, DELETE 

-- kita bisa buat user itu menurunkan privilege (memberikan privilege ke user lain)
--menggunakan WITH GRANT OPTION di akhir syntax

--misal 
GRANT INSERT ON BAHAN TO a1 WITH GRANT OPTION;
--nanti user a1 dapat memberikan hak akses insert ke tabel bahan ke user yang ada di database

--kita bisa memberikan privilege agar user agar dapat membuat login baru untuk user lain 
GRANT CREATE LOGIN TO namauser;

-- kita perlu menambahkan role db_owner ke user tersebut
-- ada 2 cara :
--1. pakai syntax ini
USE [namadb];
ALTER ROLE [db_owner] ADD MEMBER [namauser];
--2. dari SSMS, klik kanan di nama user > properties > membership > pilih role nya > ok

--untuk memberikan authorization pada user, kita bisa memberikan hak akses 
USE [namadb];
ALTER AUTHORIZATION ON SCHEMA::[db_owner] TO [namauser];
--atau lewat SSMS dengan cara klik kanan di nama user > properties > owned schemas > pilih skema nya > ok

--baru kita bisa memberikan hak aksesnya untuk membuat user
GRANT CREATE USER TO namauser;

--kita juga bisa buat user itu mengubah data dari sebuah login
--pertama kita harus menggunakan master
use master;
grant alter any login to namauser; 


--ada cara yang lebih powerful tapi berbahaya untuk dilakukan, yaitu grant sysadmin
--sysadmin adalah hak akses tertinggi 
--ALTER SERVER ROLE [sysadmin] ADD MEMBER [namauser];


-- 2. menggunakan function built in untuk add role
exec sp_addrolemember 'db_datareader','a1';

--untuk tarik privilege dari sebuah user, pakai keyword REVOKE
REVOKE SELECT ON namatabel TO namauser;

--untuk menghapus role
exec sp_droprolemember 'db_datareader','a1'

 
--untuk hapus user, kita bisa pakai drop user
--jangan lupa untuk menggunakan database yg sesuai
use db_tokoroti;
drop user a2;

--untuk hapus login, bisa pakai drop login
--PASTIKAN login tersebut sudah tidak memiliki user di database manapun dalam database engine
--use master untuk drop login
use master;
drop login a2;

--user juga bisa mengganti password milik user lain, dengan syarat sudah diberikan privilege
ALTER LOGIN namalogin WITH PASSWORD='password';


--buat cek error log (misal ada error yang terjadi), bisa cek di sysadmin dengan command
EXEC sp_readerrorlog;

--goodluck :D