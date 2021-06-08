# wind-forecast-api

I've created a very basic Web API I use to obtain notifications when there is a risk of rain or strong wind gust. I use it to be notified so I can remove outside sails when there is a significant risk.

I made it for my own usage so if you want to use it you'll have to adapt code a little bit. For instance, the forecast concerns only tomorrow in Montpellier (France) city.

I have written an article on [dev.to](https://dev.to/___bn___/free-certified-ssl-certificate-in-asp-net-5-kestrel-application-kgn) which explains how to set it up.

If you want to test the live site, navigate to https://boncocotier.duckdns.org

If you're only interested in push notifications handling you should have a look to classes in [services folder](./Services) 

## Credits

Special thanks to [Igor Chubin](https://github.com/chubin/wttr.in) for providing a wonderful free weather API I'm relying on in this project.
