dotnet publish --configuration Release
ssh pi@192.168.1.25 sudo systemctl stop windforecast-api
scp -r ./bin/Release/net5.0/* pi@192.168.1.25:/home/pi/windforecast-api/
ssh pi@192.168.1.25 sudo systemctl start windforecast-api