-- Procedure
-- jangan lupa setiap variabel di sql server menggunakan @
CREATE OR ALTER PROCEDURE dbo.perbandinganAngka(
	@angka1 INT,
	@angka2 INT
)
AS
BEGIN
	-- selection 
	IF @angka1 > @angka2
		-- melakukan print
		PRINT 'Angka 1 lebih besar dari Angka 2'
	ELSE IF @angka1 < @angka2
		PRINT 'Angka 2 lebih besar dari Angka 1'
	ELSE 
		PRINT 'Angka 1 sama dengan Angka 2'
END 

-- pemanggilan procedure dengan EXEC dbo.<nama_procedure> <parameter>
EXEC dbo.perbandinganAngka 2,2
EXEC dbo.perbandinganAngka 1,3
EXEC dbo.perbandinganAngka 3,1

-- Function
CREATE OR ALTER FUNCTION dbo.jumlahTransaksiDariPelanggan(
	@fk_pelanggan INT
)  
-- returns disini fungsinya memberi tahu tipe data apa yang menjadi returnnya
RETURNS VARCHAR(255)
AS 
BEGIN
	-- setiap variabel harus dilakukan declare terlebih dahulu baru bisa digunakan
	DECLARE @jumlah INT,
			@output VARCHAR(255)

	-- untuk mengganti nilai variabel harus menggunakan set
	SET @jumlah = (
						SELECT COUNT(NOMOR_NOTA)
						FROM h_trans
						WHERE FK_PELANGGAN = @fk_pelanggan
	);
	IF @jumlah <= 0
		SET @output = 'Pelanggan tidak pernah membeli'
	ELSE
		-- convert digunakan untuk mengubah tipe data 
		SET @output = 'Pelanggan telah membeli sebanyak ' + CONVERT(varchar(10),@jumlah) + 'kali';
	RETURN @output;
END

-- pemanggilan function dengan menggunakan select
SELECT dbo.jumlahTransaksiDariPelanggan(1);
	
-- Tampilkan semua roti berdasarkan jenis rotinya
CREATE OR ALTER PROCEDURE dbo.tampilkanRoti
(
	@jenis varchar(50)
)
AS
BEGIN
	DECLARE 
			@nama varchar(50),
			@i int,
			@deskripsi varchar(50),
			@output varchar(3000) = ''
	
	-- buat cursor
	DECLARE cursor2 CURSOR FOR
	SELECT nama, DESKRIPSI
	FROM JENIS_ROTI jr
	JOIN ROTI r ON r.JENIS_ROTI = jr.id
	WHERE UPPER(jr.NAMA_JENIS) = UPPER(@jenis)
	 
	-- buka cursor
	OPEN cursor2;

	FETCH NEXT FROM cursor2 INTO @nama, @deskripsi;

	-- fetch_status menandakan declare cursornya berhasil
	WHILE @@FETCH_STATUS = 0
		BEGIN
			SET @output = CONCAT(@output, @nama, ': ', @deskripsi, CHAR(13))
			-- fetchnya ditambah
			FETCH NEXT FROM cursor2 INTO @nama, @deskripsi;
		END;
	-- buat cursor
	CLOSE cursor2;

	-- bersihkan cursornya, agar variabel cursor2 bisa dipakai lagi sebagai cursor
	DEALLOCATE cursor2;

	-- menampilkan hasilnya
	SELECT @output AS 'Roti yang terdaftar';
END

EXEC dbo.tampilkanRoti 'Bread'

