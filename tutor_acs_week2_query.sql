--use db_tokoroti

-- dapetin nama karyawan, jabatannya & jumlah transaksi yang pernah dilakukan 

-- isnull kalo di oracle nvl buat ganti nilai null jadi value yang diinginkan
-- convert cara ngubah tipe data di sql server. bisa diisi varchar, int, datetime, dll.
-- 										v query panjang ini pengganti initcap karena sql server tdk kenal initcap jadi pakai substring
-- LEFT / RIGHT buat pengganti lpad
SELECT LEFT(K.NAMA,25) "NAMA KARYAWAN", RIGHT(upper(substring(J.NAMA_JABATAN,1,1))+lower(substring(J.NAMA_JABATAN, 2, len(J.NAMA_JABATAN)-1)),10) "JABATAN", LEFT(ISNULL(CONVERT(varchar,TAB1.JML),'0'),20) "PELAYANAN TRANSAKSI"
FROM KARYAWAN K 
	JOIN JABATAN J ON K.FK_JABATAN = J.ID 
	LEFT JOIN (
		SELECT FK_KARYAWAN "CODE", COUNT(FK_KARYAWAN) "JML"
		FROM H_TRANS
		GROUP BY FK_KARYAWAN) TAB1 ON K.ID = TAB1.CODE
ORDER BY 3 DESC
;

-- isi sub query no 1
-- cari kode karyawan & jumlah transaksi yang pernah dilakukan sama orang itu
-- SELECT FK_KARYAWAN "CODE", COUNT(FK_KARYAWAN) "JML"
-- 		FROM H_TRANS
-- 		GROUP BY FK_KARYAWAN


-- dapetin nama jenis roti & berapa kali jenis itu di beli
SELECT J.NAMA_JENIS "NAMA JENIS" , TAB2.JML "JUMLAH PEMBELIAN"
FROM JENIS_ROTI J 
JOIN (
	SELECT J.ID "CODE", COUNT(J.ID) "JML"
	FROM D_TRANS D 
		JOIN ROTI R ON D.FK_ROTI = R.ID
		JOIN JENIS_ROTI J ON J.ID = R.JENIS_ROTI
	GROUP BY J.ID) TAB2 ON J.ID = TAB2.CODE
;

-- dapetin data user & status membershipnya
-- jangan lupa kalau pakai case diakhiri dengan END
SELECT P.KODE "KODE", P.NAMA "NAMA LENGKAP", CASE WHEN P.JENIS_KELAMIN = 'L' THEN 'Laki - Laki' ELSE 'Perempuan' END AS "JENIS KELAMIN", P.EMAIL "ALAMAT EMAIL", CASE WHEN ISNULL(MEMBERSHIP_ID,0) = 0 THEN 'NO' ELSE 'YES' END AS "MEMBERSHIP"
FROM PELANGGAN P
;


-- ini contoh buat align center kalau di sql server
SELECT kode "KODE", LEFT(REPLICATE(' ',((25-LEN(NAMA))/2))+NAMA+REPLICATE(' ',((25-LEN(NAMA))/2)),25) "NAMA KARYAWAN"
FROM KARYAWAN
;

-- ini kalau mau initcap tapi 2 kata
-- patindex itu mirip sama instring di oracle
SELECT upper(substring(NAMA_JENIS,1,1))+ CASE WHEN PATINDEX('% %', NAMA_JENIS) = 0 THEN lower(substring(NAMA_JENIS, 2, len(NAMA_JENIS)-1)) ELSE lower(substring(NAMA_JENIS, 2, PATINDEX('% %', NAMA_JENIS)-1)) + upper(substring(NAMA_JENIS,PATINDEX('% %', NAMA_JENIS)+1,1))+lower(substring(NAMA_JENIS, PATINDEX('% %', NAMA_JENIS)+2, len(NAMA_JENIS)-PATINDEX('% %', NAMA_JENIS)-1)) END "Jenis Bahan"
FROM JENIS_BAHAN
;

-- penggunaan SUM & AVERAGE jangan lupa di group by
SELECT METODE_PEMBAYARAN , SUM(TOTAL) "Total Pendapatan", AVG(TOTAL) "AVG Pembayaran"
FROM H_TRANS where STATUS = 1 OR STATUS = 2
GROUP BY METODE_PEMBAYARAN
;