#!/usr/bin/python3
# -*- coding: utf-8 -*- 
import sys
import serial
import time
import paho.mqtt.client as mqtt 


def on_connect(client, userdata, flags, rc): 
   print("Connected with result code " + str(rc)) 
   # Subscribing in on_connect() means that if we lose the connection and 
   # reconnect then subscriptions will be renewed. 
   client.subscribe("/fan/pi") 
# The callback for when a PUBLISH message is received from the server. 
def on_message(client, userdata, msg): 
   #print(msg.topic+" "+str( msg.payload)) 

   data = str( msg.payload).replace("b'", "").replace("'", "")
   print(data) 
   splitedData = data.split('*')
   if len(splitedData)>1 :

     wf0 = open("/home/pi/Marmara/DataFiles/DataSensor/MQ135.txt", "wb")
     wf0.write(splitedData[0].encode())
     wf0.close()
     time.sleep(0.1) 
     print("splitedData[0] : " + splitedData[0])  
          

     wf0 = open("/home/pi/Marmara/DataFiles/DataSensor/DHT11.txt", "wb")
     wf0.write(splitedData[1].encode())
     wf0.close()
     time.sleep(0.1) 
     print("splitedData[1] : " + splitedData[1])  
   # Check if this is a message for the Pi LED. 

client = mqtt.Client() 
client.on_connect = on_connect 
client.on_message = on_message 
client.connect('localhost', 1883, 60) 
# Connect to the MQTT server and process messages in a background thread. 
client.loop_start() 
# Main loop to listen for button presses. 

while True: 

   rc = open("/home/pi/Marmara/DataFiles/DataActuator/FAN.txt", "r+")
   fanValueText = rc.read().split(':')[1]
   client.publish('/fan/esp8266', fanValueText) 
   print('fanValueText :' + fanValueText) 
   
   time.sleep(1)
   
