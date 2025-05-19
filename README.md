# azure-iot-device-config

This repository is for IoT device configuration/provisioning for real-time device to cloud messaging, cloud to device commands, and integration with Microsoft Azure IoT Central. 

 .NET 6.0 (6.0.25) - SDK (v6.0.417) Linux ARM64 Binary  
 Raspberry Pi OS 64-bit

## Run GitHub Repository on Raspberry Pi:

1. SSH into Raspberry Pi: `ssh user@raspberrypi.local`
2. Log in to GitHub CLI: `gh auth login`

`? What account do you want to log into?` `GitHub.com`\
`? What is your preferred protocol for Git operations on this host?` `HTTPS`\
`? Authenticate Git with your GitHub credentials?` `Yes`\
`? How would you like to authenticate GitHub CLI?` `Paste an authentication token`\
`Tip: you can generate a Personal Access Token here 	https://github.com/settings/tokens The minimum required scopes are 'repo', 'read:org', 'workflow'.`\
`? Paste your authentication token:` `****************************************`

3. <ins>For First Time ONLY:</ins> `gh repo clone JeffreyLaederach/azure-iot-device-config`
4. After that, cd into repo directory using `cd ~/azure-iot-device-config` and then run `gh repo sync`
5. `dotnet build`
6. `dotnet publish --runtime linux-arm64 --self-contained`
7. `cd /home/user/azure-iot-device-config/bin/Debug/net6.0/linux-arm64/publish/`
8. Input the following one line at a time:\
   `export IOT_DEVICE_MODEL_ID="dtmi:company:<unique-model-identifier>;<model-version-number>"`\
   `export IOTHUB_DEVICE_DPS_DEVICE_ID="**********"`\
   `export IOTHUB_DEVICE_DPS_DEVICE_KEY="*********************************"`\
   `export IOTHUB_DEVICE_DPS_ENDPOINT="global.azure-devices-provisioning.net"`\
`export IOTHUB_DEVICE_DPS_ID_SCOPE="***********"`\
`export MOCK_DATA=False`
10. Run using `./azure-iot-device-config`

Note:\
Use "Ctrl+C" to stop the program in the terminal.\
Type "logout" to end SSH session between computer and microcontroller. 