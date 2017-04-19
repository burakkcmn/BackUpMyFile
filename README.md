# BackupMyFile - Dosya Yedekleme Programý
BackupMyFile yedekleme iþlerinizi kolay ve hýzlý þekilde tamamlamanýzý saðlayan bir yazýlýmdýr. Bu yazýlýmla verilerinizi bilgisayarýn depolama aygýtlarýndan belirleyeceðiniz herhangi birisine veya USB depolama aygýtýnýza kaydedebilirsiniz.

# Özellikler:
 - Manual backup
 - Auto backup
   - Dosya deðiþtiðinde backup
   - Periyodik backup
 - Backup hýzý (0-20 arasý, 0 hýzlý)
	
# Koddan exe dosyasýný elde etme(Build iþlemi):
  - Zip dosyasýný açýn ve BackUpMyFile.sln'i Visual Studio 2015 ile çalýþtýrýn. 
  - Solution Explorer penceresinden projenin üzerine sað tuþ týklayarak build seçeneðine týklayýn.
  - ".exe" uzantýlý dosyayý ...\BackUpMyFile\BackUpMyFile\bin\(Debug veya Release) klasörünün içerisinde bulacaksýnýz.
 
# Kullanýmý:
  - Yedekleme iþlemini anlýk olarak yapmak istiyorsanýz yapmanýz gereken “Source” kýsmýna çift týklayarak yedeðini almak istediðiniz klasörün yolunu ve “Destination” kýsmýna çift týklayarak yedeði kopyalamak istediðiniz klasörün yolunu belirtmeniz. Daha sonra “Backup” butonuna týklamanýz yeterli olacaktýr.
  - Auto backup özelliðini açmak için taskbardan(saðatin olduðu yer) programýn simgesine sað tuþ týklayýn ve "Options" seçeneðini seçin. Açýlan pencerede "Auto Backup" kutucuðunu seçili hale getirin.
  - Yedekleme iþlemini belirli zaman aralýklarýyla yapmak istiyorsanýz “Source” ve “Destination” kýsmýný yukarýdaki gibi yaptýktan sonra “Backup Method” kutucuðundan "Periodically" seçeneðini seçerek saðýnda görünecek kutuya saniye cinsinden zamaný girmeniz ve "Add" butonuna týklayarak kaydetmeniz gerekiyor.
  - Yedekleme iþlemini her hangi bir dosya deðiþtiðinde yapmak istiyorsanýz “Source” ve “Destination” kýsmýný yukarýdaki gibi yaptýktan sonra “Backup Method” kutucuðundan "When file changed" seçeneðini seçmeniz ve "Add" butonuna týklayarak kaydetmeniz gerekiyor.
  - Daha önce kaydettiðiniz bir backup üzerinde deðiþiklik yapmak için, "Archive" butonuna týklayýnýz. Açýlan pencerede deðiþiklik yapmak istediðiniz backup'ý seçin. Silmek için "Delete" butonuna týklayýn. Deðiþtirmek için ise "Select" butonuna týklayarak deðiþiklikleri yapýn ve "Add butonuna týklayýn."
  - Copyalama hýzýný belirlemek için taskbardan(saðatin olduðu yer) programýn simgesine sað tuþ týklayýn ve "Options" seçeneðini seçin. Açýlan pencerede "Speed choice" kutucuðundan hýzýný belirleyebilirsiniz.

[burakkocaman.com](http://burakkocaman.com/dosya-yedekleme-programibackupmyfile/)

[github/Burak-CP](https://github.com/Burak-CP/BackUpMyFile/)