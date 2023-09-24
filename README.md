# StatusMonitor-Simple

This is a simple web-application for sensor-data. It consists of a background-service running MQTTnet that 
subscribes to a list of mqtt-topics, and stores them in a sqlite-database, and a web-frontend that displays 
current and historic data. 

It needs access to two config-files at startup.

 - loggSettings.config -> contains log-settings for .net
 - projectSettings.config -> contains project-information and sensor-configuration

Examples of these files are in the /src/WebUI.Razor/Config-folder of this repository

## The program takes two arguments at startup. 
 
    The first is the folder-path of the folder that contains both the loggSettings.config and the
    projectSettings.config files. If these are not provided it will use the two example files 
    located in the /src/WebUI.Razor/Config-folder of this repository. 

    The second is a string that tells the program what to do with the database when it starts up,
    and it has three options:
    
        - "fresh database" -> deletes the entire database and recreates it
        - "clear readings" -> deletes the existing sensor-readings for all sensors
        - "*" -> Any other string (or no string at all) only ensures that the database exists.

## Explanation of the projectSettings-file
```
{
    "ProjectSettings": {
        "projectName" : "StatusMonitor",                            -> displayed in the header
        "mqttBroker" : "broker.hivemq.com",                         -> url of the mqtt-broker
        "mqttPort" : 8883,                                          -> port for the mqtt-broker
        "mqttUser" : "",                                            -> username for the mqtt-broker
        "mqttPassword" : "",                                        -> password for the mqtt-broker
        "mqttClientId" : "statusmonitor-simple-Id-asdf12123as"      -> unique Client-ID
    },
    "Sensors": [
        {
            "Id" : "1",                                             -> unique ID for the sensor in the database
            "Name": "Sensor 1",                                     -> name of the sensor
            "Description": "Sensor 1 description",                  -> description of the sensor
            "MqttTopic": "asdasdasd/statusmonitor/example/sensor1", -> mqtt-topic for the sensor
            "MinReading": 0,                                        -> minimum reading for the sensor, values below this will not be recorded
            "MaxReading": 500,                                      -> maximum reading for the sensor, values above this will not be recorded
            "Measurement": "cm",                                    -> measurement-type for the sensor readings. cm, kg, bar etc. only used in the web-UI
            "PageSize": 24,                                         -> page size used for pagination of data. determines the number of readings displayed in the chart
            "Group": "group 1",                                     -> group-name. sensors with the same group is displayed together in the web-UI under the group-name
            "Divider": 1                                            -> used to convert a raw sensor-reading to a scaled reading. The incomming message is divided by this number it 
        },(...)                                                        is stored in the database. If this is handles by the mqtt-publisher, simply set it to 1
    ]
}
```