# AKILLI BİNALARDA WEB TEKNOLOJİLERİNİN KULLANIMI

 ![MULogo](https://github.com/msensoy/MarmaraUniversityMasterThesis/blob/master/images/MULogo.PNG) </br>
 
Marmara Üniversitesi Yüksek Lisans Tezi kapsamında Web of Things (WoT) mimarisinde .NET Core ile geliştirilen uygulamalar hakkında bilgileri içermektedir.

 ![WebSite](https://github.com/msensoy/MarmaraUniversityMasterThesis/blob/master/images/WebSite.PNG) </br>
 
Tüm yazılımlar Rasbian işletim sistemi yüklü bir Raspberry Pi 4 cihazında çalışmaktadır. Kullanılan yazılım ve donanım gereksinimleri aşağıda belirtilmiştir.
## Proje Gereksinimleri
  ### Yazılım Çerçeveleri Sistemi
	 [Microsoft .NET 5.0](https://www.petecodes.co.uk/install-and-use-microsoft-dot-net-5-with-the-raspberry-pi/) </br>
	 [Python 3.8](https://projects.raspberrypi.org/en/projects/generic-python-install-python3) </br>
	 [Minimalmodbus (RS-485 için](https://pypi.org/project/minimalmodbus/) </br>
	 [Mosquitto (MQTT için](https://randomnerdtutorials.com/how-to-install-mosquitto-broker-on-raspberry-pi/) </br>
	 [Blueman (Bluetooth için](https://howchoo.com/pi/bluetooth-raspberry-pi) </br>
	 
  ### Donanım
     Raspberry Pi 4
	 NodeMCU ESP8266
	 Arduino Uno (x2)
	 HC-06 Bluetooth modülü
	 MAX485 - TTL RS485
	 USB to RS485
	 Röle (x2)
	 AC Dimmer Modül
	 Buzzer
     Sensör - DHT11 
	 Sensör - MQ2 
	 Sensör - MQ135 
	 Sensör - LDR
	 Sensör - Ses
	 Sensör - Alev

Proje prototipi aşağıda gösterilmiştir.

 ![Prototype](https://github.com/msensoy/MarmaraUniversityMasterThesis/blob/master/images/Prototype.PNG) </br>

## Uygulama

Raspberry Pi kartına gerekli kütüphane yüklemeleri ve donanım bağlantıları yapıldıktan sonra .NET Core yazılımları artık çalıştırılabilir. Öncelikle kök dizin içerisine "Marmara" adında klasör açılır. Daha sonra klasör içerisine aşağıda gösterildiği gibi alt klasörler açılır. Bu alt klasörlerin her biri farklı bir amaç için oluşturulmuştur.
 ![Folders](https://github.com/msensoy/MarmaraUniversityMasterThesis/blob/master/images/Folders.PNG) </br>
 
### API
`cd Marmara/API/` komutu ile klasörün içerisine giriniz. Daha sonra `./Marmara.API` komutunu terminalde çalıştırdığınızda aşağıdaki gibi görüntü elde edeceksiniz.

 ![API](https://github.com/msensoy/MarmaraUniversityMasterThesis/blob/master/images/RunAPI.PNG) </br>


### Worker Service
`cd Marmara/WorkerService/` komutu ile klasörün içerisine giriniz. Daha sonra `./Marmara.WorkerService` komutunu terminalde çalıştırdığınızda aşağıdaki gibi görüntü elde edeceksiniz.

 ![WorkerService](https://github.com/msensoy/MarmaraUniversityMasterThesis/blob/master/images/RunWS.PNG) </br>

### MVC

### Python Yazılımları

`cd Marmara/PythonCodes/` komutu ile python dosyalarının bulunduğu klasörün içerisine giriniz. `python3 lighting.py` komutunu ile aydıntlatma sistemi için kodlar çalışacaktır. `python3 hvac.py` komutunu ile aydıntlatma sistemi için kodlar çalışacaktır. `python3 safety.py` komutunu ile aydıntlatma sistemi için kodlar çalışacaktır.





