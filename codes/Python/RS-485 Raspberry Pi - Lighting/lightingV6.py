#!/usr/bin/python
import minimalmodbus
import serial
import time
oldLdrValue=0

instrument = minimalmodbus.Instrument('/dev/ttyUSB0', 10) 

try:
	while 1:
		rl = open("/home/pi/Marmara/DataFiles/DataActuator/LEDdim.txt", "r+")
		ledValue = rl.read().split(':')[1]
		instrument.write_register(1, int(ledValue), 0)

		rc = open("/home/pi/Marmara/DataFiles/DataActuator/CFL.txt", "r+")
		cflValueText = rc.read().split(':')[1]
		cflValue=0
		if cflValueText == "ON":
		  cflValue=1
		instrument.write_register(2, int(cflValue), 0)  
        
		ldrValue = instrument.read_register(3, 0) 
		if(oldLdrValue != ldrValue):
		  oldLdrValue = ldrValue
		  wl = open("/home/pi/Marmara/DataFiles/DataSensor/LDR.txt", "wb")
		  ldrString = "LDR:" + str(ldrValue)
	
		  wl.write(ldrString.encode())
		  wl.close()
		  time.sleep(0.1) 

          
		print("LED value:" + str(ledValue))
		print("CFL value:" + str(cflValue))
		print("LDR value:" + str(ldrValue))
		
		time.sleep(1)
except KeyboardInterrupt:
	print("Quit")