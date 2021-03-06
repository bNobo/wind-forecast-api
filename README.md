# wind-forecast-api

I've created a very basic Web API I use to obtain notifications when there is a risk of rain or strong wind gust. I use it to be notified so I can remove outside sails when there is a significant risk.

I made it for my own usage so if you want to use it you'll have to adapt code a little bit. For instance, the forecast concerns only tomorrow in Montpellier (France) city.

I have written an article on [dev.to](https://dev.to/___bn___/free-certified-ssl-certificate-in-asp-net-5-kestrel-application-kgn) which explains how to set it up.

If you want to test the live site, navigate to https://boncocotier.duckdns.org

If you're only interested in push notifications handling you should have a look to classes in [services folder](./Services) 

## Net core / Net 5 installation on Raspberry PI

Installing *.Net 5* with *apt-get* won't work on Debian Stretch 9. The easiest method is to use [dotnet-install.sh](https://docs.microsoft.com/en-us/dotnet/core/install/linux-scripted-manual#scripted-install) script provided by Microsoft.

```bash
./dotnet-install.sh -c 5.0 --runtime aspnetcore
```

After installation, set dotnet environment variables in *.bashrc*:

```ini
# export .NET runtime path
export DOTNET_ROOT=$HOME/.dotnet
export PATH=$PATH:$DOTNET_ROOT
```

Set the *ExecStart* in service file:

```ini
ExecStart=/home/pi/.dotnet/dotnet /home/pi/windforecast-api/wind-forecast-api.dll
```

## Credits

Special thanks to [Igor Chubin](https://github.com/chubin/wttr.in) for providing a wonderful free weather API I'm relying on for this project.
