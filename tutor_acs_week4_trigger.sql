-- trigger ->  prosedur yang secara otomatis akan dieksekusi akibat suatu tindakan pada sebuah table dalam database
-- contoh penggunaan trigger, misal mau nambah data transaksi baru, kemudian buat trigger otomatis untuk ngurangi stok barang terkait
--trigger bisa ditempelkan pada table berdasarkan perubahan data yang diinginkan (insert, update, delete)

--trigger di sql server ada 2 jenis, instead of dan after 
-- bedanya, kalau instead of dijalankan SEBELUM syntax yang dituju dijalankan
-- kalau after dijalankan sesudah syntax tersebut dijalankan 
-- cara mudah hafalinnya, instead of itu kyk before, after ya after :)

-- untuk instead of, syntax asli tujuan yang mau dijalankan akan dibatalkan dan diganti dengan menjalankan code trigger
 
--untuk buat trigger, ada 2 cara yaitu lewat management studio dan lewat query

-- 1. lewat management studio
-- buka management studio, pilih database, pilih tabel yang akan ditambahkan trigger 
-- expand tabel tersebut, pilih triggers, pilih new trigger  
-- ini nanti si management studio akan membuat template untuk triggernya 

--2. pakai query buat sendiri 
CREATE OR ALTER TRIGGER [nama_trigger] 
ON [nama_tabel]
[INSTEAD OF, AFTER]  [INSERT, UPDATE, DELETE]  
AS 
BEGIN 
-- di sini akan diisi codingan trigger yang akan dijalankan
END

--contoh pakai db_tokoroti

-- di sini akan dibuat trigger baru untuk insert pada tabel voucher. Kita akan memberikan beberapa pengecekkan lewat trigger 
--1. Nama tidak boleh kembar dengan nama voucher yang lain
--2. jenis hanya 'POTONGAN' atau 'DISKON', 
--3. waktu_exp minimal 1 dan maksimal 12 
--dan  kita juga akan men-generate id otomatis dari data yang akan ditambahkan 

use db_tokoroti;
CREATE OR ALTER TRIGGER TriggerNewVoucher
ON VOUCHER 
INSTEAD OF INSERT 
AS 
BEGIN
    SET NOCOUNT ON;
	DECLARE @newId int = 0; 
	DECLARE @exceptionMessage varchar(40) = '';
	DECLARE @cekJenis varchar(15) = '';
	DECLARE @cekNama varchar(15) = '';
	DECLARE @cekExpired int = 0;
	DECLARE @namaFounded int = 0;


	--inserted itu untuk mengambil data yang akan diinsertkan
	SET @cekJenis = (SELECT JENIS from inserted)
	SET @cekExpired = (SELECT WAKTU_EXP from inserted)
	SET @cekNama = (SELECT NAMA from inserted)  

	--cek nama kembar
	SET @namaFounded = (SELECT COUNT(*) FROM VOUCHER WHERE NAMA=@cekNama);

	IF @namaFounded = 0 
		BEGIN 
			--cek jenisnya
			IF @cekJenis = 'DISKON' OR @cekJenis = 'POTONGAN'
				BEGIN 
					--cek waktu expired 
					IF @cekExpired > 0 AND @cekExpired<13 
						BEGIN
							-- set id baru 
							SET @newId = (SELECT COUNT(*)+1 from VOUCHER);
							--insert
							INSERT INTO VOUCHER VALUES(@newId, (SELECT NAMA FROM inserted),
							@cekJenis,(select POTONGAN from inserted),(select WAKTU_EXP from inserted));
						END 
					ELSE
						BEGIN
							RAISERROR('Waktu expired tidak sesuai',16,1); 
						END
				END 
			ELSE 
				BEGIN
						RAISERROR('Jenis tidak sesuai! ',16,1); 
				END
		END
	ELSE 
		BEGIN 

			SET @exceptionMessage = CONCAT('Nama ',@cekNama,' sudah pernah digunakan!');

			RAISERROR(@exceptionMessage,16,1); 
		END  
END 

--contoh insert berhasil 
INSERT INTO VOUCHER VALUES(null, 'Voucher1','POTONGAN',4000,5);
--meskipun id diset null, dia bisa generate otomatis pakai trigger 

--contoh insert gagal karena nama kembar
INSERT INTO VOUCHER VALUES(null, 'HEMATDULUBOS','POTONGAN',4000,5);

--nanti dia muncul error seperti 
-- Msg 50000, Level 16, State 1, Procedure TriggerNewVoucher, Line 52 [Batch Start Line 0]
-- Nama HEMATDULUBOS sudah pernah digunakan

--contoh insert gagal karena jenis salah
INSERT INTO VOUCHER VALUES(null, 'Voucher2','HELO',4000,5);



--syntax SET NOCOUNT ON mencegah pengiriman pesan DONEINPROC ke klien untuk setiap pernyataan dalam prosedur tersimpan

-- syntax RAISERROR digunakan untuk memberikan custom error message untuk exception dalam sql server. Dia membutuhkan 3 parameter yaitu messagenya, severity level dan state level (silahkan cari sendiri ini apa wkwk)

--FROM inserted adalah syntax untuk mengambil data yang "maunya" diinsertkan ke dalam tabel
-- nanti ada juga from deleted, untuk mengambil data yang maunya dihapus 


--contoh lain misal membuat trigger untuk update qty stok bahan, apabila di set 0, maka status dari bahan itu akan diubah menjadi 0, selain itu statusnya akan diubah menjadi 1

CREATE OR ALTER TRIGGER TriggerUpdateStokBahan
ON BAHAN
AFTER UPDATE
AS 
BEGIN 
	SET NOCOUNT ON;
	DECLARE @idBahan int = 0;
	SET @idBahan = (SELECT ID FROM inserted);
	IF (SELECT QTY_STOK FROM inserted) = 0 
		BEGIN 
			UPDATE BAHAN SET STATUS = 0 WHERE ID = @idBahan; 
		END
	ELSE 
	 	BEGIN 
			UPDATE BAHAN SET STATUS = 1 WHERE ID = @idBahan;
		END
END

--CONTOH BERHASIL 
UPDATE BAHAN SET QTY_STOK = 0 WHERE ID = 1;

UPDATE BAHAN SET QTY_STOK = 15 WHERE ID = 1;

--untuk update, bisa from inserted atau from deleted


--contoh trigger untuk cek sebelum menghapus bahan, apakah bahan yang dihapus memiliki stock atau tidak, jika ada stock, maka berikan error 
CREATE OR ALTER TRIGGER TriggerDeleteBahan
ON BAHAN 
INSTEAD OF DELETE 
AS
BEGIN 
	SET NOCOUNT ON;
	DECLARE @id int = 0;
	DECLARE @qty int = 0;
	DECLARE @msgError varchar(50) = '';

	SET @id = (SELECT ID FROM deleted);
	SET @qty = (SELECT QTY_STOK FROM deleted);
	IF @qty > 0  
		BEGIN 
			SET @msgError = CONCAT('Bahan ',(SELECT MERK FROM deleted),' memiliki stok!')
			RAISERROR(@msgError,16,1)
		END 
	ELSE 
	BEGIN 
		DELETE FROM BAHAN WHERE ID=@id;
	END
END  

--contoh diinsert dulu bahan baru
insert into bahan values(17,'ABCD00001','ABCD',0,10000,'GRAM',2,1);

--contoh delete error
DELETE FROM BAHAN WHERE ID=15
--contoh delete berhasil
DELETE FROM BAHAN WHERE ID=17



--untuk drop trigger bisa pakai
DROP TRIGGER namaTrigger;


--PERHATIKAN 
-- TRIGGER TERIKAT PADA TABLE DB
-- jadi kalau table didrop, triggernya akan terdrop juga


--goodluck :)