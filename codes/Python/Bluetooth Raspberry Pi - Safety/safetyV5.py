#!/usr/bin/python
# -*- coding: utf-8 -*- 
import serial
import time

bluetoothSerial = serial.Serial("/dev/rfcomm0",baudrate=9600)
print("Bluetooth connected")
oldAlarmStatus = "ON"
oldAlarmSensorStatus = "OFF"
oldFlameStatus = "0"
oldSoundStatus = "0"
oldPIRStatus = "0"
oldMQ2Values = "0"

try:
	while 1:
		data = ""
		data = bluetoothSerial.readline()
		print(data)
		splitedData = data.split('*')
		if len(splitedData)>4 :
        
		  if(splitedData[3] == oldAlarmSensorStatus):
		    print("splitedData[3] : " + splitedData[3])     
               
		  else:
		    oldAlarmSensorStatus = splitedData[3] 
		    wf3 = open("/home/pi/Marmara/DataFiles/DataSensor/ALARMSENSOR.txt", "wb")
		    wf3.write(splitedData[3])
		    wf3.close()
		    time.sleep(0.1) 
		    print("NEW splitedData[3] : " + splitedData[3])  
            
		  if(splitedData[2] == oldFlameStatus):
		    print("splitedData[2] : " + splitedData[2])  
		  else:            
		    oldFlameStatus = splitedData[2] 
		    wf2 = open("/home/pi/Marmara/DataFiles/DataSensor/FLAME.txt", "wb")
		    wf2.write(splitedData[2])
		    wf2.close()
		    time.sleep(0.1) 
		    print("NEW splitedData[2] : " + splitedData[2])            

		  if(splitedData[0] == oldPIRStatus):
		    print("splitedData[0] : " + splitedData[0])  
		  else:       
		    oldPIRStatus = splitedData[0]           
		    wf = open("/home/pi/Marmara/DataFiles/DataSensor/PIR.txt", "wb")
		    wf.write(splitedData[0])
		    wf.close()
		    time.sleep(0.1) 
		    print("NEW splitedData[0] : " + splitedData[0])

		  if(splitedData[1] == oldSoundStatus):
		    print("splitedData[1] : " + splitedData[1])  
		  else:       
		    oldSoundStatus = splitedData[1]           
		    wf1 = open("/home/pi/Marmara/DataFiles/DataSensor/SOUND.txt", "wb")
		    wf1.write(splitedData[1])
		    wf1.close()
		    time.sleep(0.1)
		    print("NEW splitedData[1] : " + splitedData[1])

		  if(splitedData[4] == oldMQ2Values):
		    print("splitedData[4] : " + splitedData[4])      
		  else:
		    oldMQ2Values = splitedData[4]            
		    wf4 = open("/home/pi/Marmara/DataFiles/DataSensor/MQ2.txt", "wb")
		    wf4.write(splitedData[4])
		    wf.close()
		    time.sleep(0.1)
		    print("NEW splitedData[4] : " + splitedData[4])

		else:
		  pass
		rf = open("/home/pi/Marmara/DataFiles/DataActuator/ALARM.txt", "r+")
		text = rf.read().splitlines()
		alarmStatus =text[0].split(':')[1]
		rf.close()
		if oldAlarmStatus == alarmStatus:
		  pass
		else:
		  oldAlarmStatus = alarmStatus
		  print("New Alarm Status From Web : " + alarmStatus)
		  bluetoothSerial.write(alarmStatus)
		time.sleep(0.5)
except KeyboardInterrupt:
	print("Quit")