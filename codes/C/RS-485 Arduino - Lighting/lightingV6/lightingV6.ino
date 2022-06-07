#include <ModbusRtu.h>
#include <RBDdimmer.h>

#define TXEN  3 
#define nbACD 2

String valueLdr ;
int pinLdr = A0;
int cflValue = 0;
int ledValue = 0;
int pinCFL = 7;

const int zeroCrossPin = 2;
const int acdPin[nbACD] ={5};
int MIN_POWER[nbACD] ={0,0};
int MAX_POWER[nbACD] ={80,80};
int POWER_STEP[nbACD] ={1,1};

int power[nbACD] ={0,0};
dimmerLamp acd[nbACD] ={dimmerLamp(acdPin[0]),dimmerLamp(acdPin[1])};

// data array for modbus network sharing
uint16_t au16data[16] = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

Modbus slave(10, Serial, TXEN); // this is slave @1 and RS-485

void setup() {
  pinMode(pinCFL, OUTPUT);
  Serial.begin(19200);
  acd[0].begin(NORMAL_MODE, ON);
  AdjustDim(50);
  slave.start();
}

void loop() {
  slave.poll( au16data, 16 );
  if (cflValue != au16data[2]) {
    cflValue = au16data[2];
    if ( cflValue == 0) {
      digitalWrite( pinCFL, HIGH);
    }
    else {
      digitalWrite( pinCFL , LOW);
    }
  }
  au16data[3] = Light(analogRead(pinLdr));

  if(ledValue != au16data[1]) {
    Serial.println("changed");
      ledValue = au16data[1];
      AdjustDim(ledValue);
  }
  


}

int Light (int readLdr) {
  double Vout = readLdr * 0.0048828125;
  //int lux=500/(10*((5-Vout)/Vout));//use this equation if the LDR is in the upper part of the divider
  int lux = (2500 / Vout - 500) / 10;
  return lux;
}
void AdjustDim(int dimValue){
acd[0].setPower(dimValue);
Serial.print(acd[0].getPower());
Serial.println(F("%"));
}
