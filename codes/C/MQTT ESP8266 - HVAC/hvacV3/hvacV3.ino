/***************************************************
  NodeMCU
****************************************************/
#include <ESP8266WiFi.h>
#include "Adafruit_MQTT.h"
#include "Adafruit_MQTT_Client.h"
#include <dht11.h>
#include "MQ135.h"
#define pinMQ135 A0
/************************* WiFi Access Point *********************************/
#define WLAN_SSID      "HUAWEI" // "SamsungA7" //"HUAWEI" 
#define WLAN_PASS       "mehmet708"
#define MQTT_SERVER      "192.168.43.36" // static ip address
#define MQTT_PORT         1883
#define MQTT_USERNAME    ""
#define MQTT_PASSWORD         ""
dht11 dht_sensor; // dht_sensor adında bir DHT11 nesnesi oluşturduk.
MQ135 gasSensor = MQ135(pinMQ135);
const int pinFan = 5;
const int pinDht11 = 4;
float airTemperature, airHumidity, ppm, ppmbalanced, rzero;
/************ Global State ******************/
// Create an ESP8266 WiFiClient class to connect to the MQTT server.
WiFiClient client;
// Setup the MQTT client class by passing in the WiFi client and MQTT server and login details.
Adafruit_MQTT_Client mqtt(&client, MQTT_SERVER, MQTT_PORT, MQTT_USERNAME, MQTT_PASSWORD);
/****************************** Feeds ***************************************/
// Setup a feed calfan 'pi_fan' for publishing.
// Notice MQTT paths for AIO follow the form: <username>/feeds/<feedname>
Adafruit_MQTT_Publish pi_fan = Adafruit_MQTT_Publish(&mqtt, MQTT_USERNAME "/fan/pi");
// Setup a feed calfan 'esp8266_fan' for subscribing to changes.
Adafruit_MQTT_Subscribe esp8266_fan = Adafruit_MQTT_Subscribe(&mqtt, MQTT_USERNAME "/fan/esp8266");
/*************************** Sketch Code ************************************/
void MQTT_connect();
void setup() {
  Serial.begin(115200);
  delay(10);
  pinMode(pinFan, OUTPUT);
  digitalWrite(pinFan, HIGH);


  Serial.println(F("RPi-ESP-MQTT"));
  // Connect to WiFi access point.
  Serial.println(); Serial.println();
  Serial.print("Connecting to ");
  Serial.println(WLAN_SSID);
  WiFi.begin(WLAN_SSID, WLAN_PASS);
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  Serial.println();
  Serial.println("WiFi connected");
  Serial.println("IP address: "); Serial.println(WiFi.localIP());
  // Setup MQTT subscription for esp8266_fan feed.
  mqtt.subscribe(&esp8266_fan);
}
uint32_t x = 0;
void loop() {

  MQTT_connect();

  Adafruit_MQTT_Subscribe *subscription;
  while ((subscription = mqtt.readSubscription())) {
    if (subscription == &esp8266_fan) {
      char *message = (char *)esp8266_fan.lastread;
      Serial.print(F("Got: "));
      Serial.println(message);
      // Check if the message was ON, OFF, or TOGGLE.
      if (strncmp(message, "ON", 2) == 0) {
        Serial.println(F("FAN CALISIYOR "));

        digitalWrite(pinFan, HIGH);
      }
      else  {
        // Turn the fan off.
        digitalWrite(pinFan, LOW);
        Serial.println(F("FAN CALISMIYOR !!! "));
      }
      int chk = dht_sensor.read(pinDht11);
      Serial.print("Nem Orani (%): ");
      Serial.println((float)dht_sensor.humidity, 2);

      Serial.print("Sicaklik (Celcius): ");
      Serial.println((float)dht_sensor.temperature, 2);

      airTemperature = (float)dht_sensor.temperature;
      airHumidity = (float)dht_sensor.humidity;
      rzero = gasSensor.getRZero(); //this to get the rzero value, uncomment this to get ppm value
      Serial.print("RZero=");
      Serial.println(rzero); // this to display the rzero value continuously, uncomment this to get ppm value

      ppm = gasSensor.getPPM(); // this to get ppm value, uncomment this to get rzero value
      Serial.print("PPM=");
      Serial.println(ppm); // this to display the ppm value continuously, uncomment this to get rzero value

      ppmbalanced = gasSensor.getCorrectedPPM(airTemperature, airHumidity); // this to get ppm value, uncomment this to get rzero value
      Serial.print("PPM Corrected=");
      Serial.println(ppmbalanced); // this to display the ppm value continuously, uncomment this to get rzero value
      Serial.println("------------------");


      String valueDHT11Temp = String(airTemperature, 2);
      String valueDHT11Hum = String(airHumidity, 2);
      String dataCO2 = "CO2: " + String(ppmbalanced);
      String dataDHT11Temp = "TEMPERATURE:" + valueDHT11Temp;
      String dataDHT11Hum = "HUMIDITY:" + valueDHT11Hum;

      String integratedData = dataCO2 + "*" + dataDHT11Temp + "; " +  dataDHT11Hum  ;
      pi_fan.publish(integratedData.c_str());
    }
  }
  delay(20);

}
// Function to connect and reconnect as necessary to the MQTT server.
void MQTT_connect() {
  int8_t ret;
  // Stop if already connected.
  if (mqtt.connected()) {
    return;
  }
  Serial.print("Connecting to MQTT... ");
  uint8_t retries = 3;
  while ((ret = mqtt.connect()) != 0) { // connect will return 0 for connected
    Serial.println(mqtt.connectErrorString(ret));
    Serial.println("Retrying MQTT connection in 5 seconds...");
    mqtt.disconnect();
    delay(5000);  // wait 5 seconds
    retries--;
    if (retries == 0) {
      // basically die and wait for WDT to reset me
      while (1);
    }
  }
  Serial.println("MQTT Connected!");
}
