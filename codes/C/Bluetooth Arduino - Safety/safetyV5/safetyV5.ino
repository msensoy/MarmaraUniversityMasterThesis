#include <MQ2.h>

String newAlarmStatus;
String valueAlarm = "OFF";
int pinAlarm = 13;   
int valueMQ2;       
int valueLimitMQ2 = 400;    
int pinMQ2 = A0;
int lpg, co, smoke;
MQ2 mq2(pinMQ2);

int valueSound ; 
int pinSoundSensor = 8;

int pinFlame = 9;
int valueFlame;

int pinPIR = 4;         
int valuePIR;
float valueMQ2LPG,valueMQ2CO,valueMQ2Smoke;

void setup() {
  pinMode(pinAlarm, OUTPUT);      
  pinMode(pinSoundSensor, INPUT);
  pinMode(pinFlame, INPUT);
  pinMode(pinPIR, INPUT); 
  digitalWrite(pinAlarm, HIGH);
  Serial.begin(9600);
  mq2.begin();

}

void loop() {
  valueMQ2LPG =  mq2.readLPG();
  valueMQ2CO = mq2.readCO();
  valueMQ2Smoke =  mq2.readSmoke();

      
  if (valueMQ2 > valueLimitMQ2) {}//valueAlarm = "ON"; 

  valueSound = digitalRead(pinSoundSensor);
  if ( valueSound != 0 ) {} //valueAlarm = "ON"; 
 
  valueFlame = digitalRead(pinFlame);
  if (valueFlame == 0) {valueAlarm = "ON"; }
  else {valueAlarm = "OFF";}
 
  valuePIR =  digitalRead(pinPIR);
  if (valuePIR == 1) {} //valueAlarm = "ON"; 

  if (Serial.available() > 0) {
  newAlarmStatus = Serial.readString();
  // Serial.println(newAlarmStatus);
 }

  if (newAlarmStatus == "ON" || valueAlarm == "ON") {
    digitalWrite(pinAlarm, HIGH);
  }
  else {
    digitalWrite(pinAlarm, LOW);
  }
  
  String dataPIR = "PIR : " + String(valuePIR);
  String dataSOUND = "SOUND : " + String(valueSound);
  String dataFLAME = "FLAME : " + String(valueFlame);
  String dataALARM = "ALARMSENSOR : " + valueAlarm;
  
  String dataMQ2CO =  "CO : " + String(valueMQ2CO);
  String dataMQ2LPG =  " ; LPG :" + String(valueMQ2LPG);
  String dataMQ2SMOKE = +" ; SMOKE :" + String(valueMQ2Smoke);
  String dataMQ2 = dataMQ2CO + dataMQ2LPG + dataMQ2SMOKE;

  String integratedData = dataPIR + "*" + dataSOUND +"*" +dataFLAME +"*"+ dataALARM +"*" + dataMQ2;
  Serial.println(integratedData);
  delay(700);
}
